﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VAPI;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using System.Diagnostics;
using Krypton;
using Krypton.Lights;
using SteamAge.Generators;
using SteamAge.TileEntities;

namespace SteamAge
{
    public class GameWorld
    {
        public const int ChunksX = 5; // Amount of Chunks being loaded near of player
        public const int ChunksY = 4; // up

        public KryptonEngine LightSystem; // Kryptong lightning engine

        public Dictionary<Vector2, Chunk> Chunks; 

        public Vector2 CameraPos; // Position of camera
        public WorldGenerator Generator; // Main world generator, register every generator BEFORE calling Generator.Generate()
        public World PhysicalWorld; // Farseer physical world
        Vector2[,] ChunksToUpdate = new Vector2[ChunksX, ChunksY]; // Array of Chunks needed to load / draw in current frame
        Player TestPlayer; // TEMP
        public Color SkyColor; // Current sky color, its being calculated every frame from LightPath, do not try to modify this, modify world LightColorPath instead
        public List<Entity> Entities; // List of all entieties // TODO: Change to dictionary

        public List<KeyValuePair<int, Color>> LightColorPath = new List<KeyValuePair<int, Color>>(); // Contains every color of sky, format KVP<duration, sky color>
        int CurrentLightTime = 0; // Current countdown to change sky color
        int CurrentColorId=0; // Current sky color id from LightColorPath, its being changed to next id after CurrentLightTime countdown finish (Cyclic)

        //FluidSimulation Fluid;
        Light2D PlayerLight; // Light generated by player (TODO: remove)
        Light2D SunLight; // Light generated by sun

        int Time = 21000; // Hour * 1000

        public WorldScreen ParentScreen; // pointer to parent screen, needed by some GUI methods

        #region Initalizing
        //====================================
        //========== INITALIZING =============
        //====================================

        public GameWorld(WorldScreen WorldScreen)
        {
            LightSystem = new KryptonEngine(GeneralManager.Game, "Effects/KryptonEffect");
            LightSystem.Initialize();
            SetLightColorPath();
            

            Chunks = new Dictionary<Vector2, Chunk>();
            Entities = new List<Entity>();
            Generator = new WorldGenerator(this);


            ParentScreen = WorldScreen;

            PhysicalWorld = new World(new Vector2(0, 16 * 9.81f));

            InitLightSystem();

            TestPlayer = new Player(this, true);


            SetBlock(new Vector2(4, 4), new TileEntities.TorchTE(this, new Vector2(128, 128)));
            SetBlock(new Vector2(4, 3), new TileEntities.TreeTE(this, new Vector2(128, 96)));
            Entities.Add(new DynamicBody(this, new CircleShape(10.5f, 0.5f), new Vector2(95, 100), "Textures/DynamicBodies/Barrel1"));

            Debug.WriteLine("World Initialized");

        }


        private static void RegisterRecipes(GameWorld World)
        {
            Crafting.CraftingRecipe TestRecipe = new Crafting.CraftingRecipe(World);
            TestRecipe.Output = new ItemStack(Block.GetBlock(1000), 1);

            Crafting.CraftingRecipePart CRP1 = new Crafting.CraftingRecipePart();
            CRP1.AddPart(new ItemStack(Block.GetBlock(1), 1));
            TestRecipe.RecipeParts[0, 0] = CRP1;

            Crafting.CraftingRecipePart CRP2 = new Crafting.CraftingRecipePart();
            CRP2.AddPart(new ItemStack(Block.GetBlock(1), 1));
            TestRecipe.RecipeParts[0, 1] = CRP2;

            Crafting.CraftingRecipe.RegisterRecipe(TestRecipe);
        }

        private void SetLightColorPath()
        {
            LightColorPath.Add(new KeyValuePair<int, Color>(1000, Color.MidnightBlue * 0.5f));
            LightColorPath.Add(new KeyValuePair<int, Color>(8000, Color.Cyan * 0.5f));
            LightColorPath.Add(new KeyValuePair<int, Color>(4000, Color.LightBlue));
            LightColorPath.Add(new KeyValuePair<int, Color>(1000, Color.LightBlue));
            LightColorPath.Add(new KeyValuePair<int, Color>(3000, Color.OrangeRed));
            LightColorPath.Add(new KeyValuePair<int, Color>(7000, Color.MidnightBlue * 0.5f));
        }

        private void InitLightSystem()
        {
            
            LightSystem.AmbientColor = new Color(0, 0, 0, 0.5f);

            Texture2D LightTexture = LightTextureBuilder.CreatePointLight(GeneralManager.GDevice, 512);

            PlayerLight = new Light2D()
            {
                Texture = LightTexture,
                Range = (float)(0.5f),
                Color = new Color(0.8f, 0.8f, 0.8f, 1f),
                Intensity = 1f,
                Angle = MathHelper.TwoPi * 2f,
                X = (float)(0),
                Y = (float)(0),
            };
            SunLight = new Light2D()
            {
                Texture = LightTexture,
                Range = (float)(1500f),
                Color = new Color(0.9f, 0.9f, 0.9f, 1f),
                Intensity = 1f,
                Angle = MathHelper.TwoPi * (float)1,
                X = (float)(0),
                Y = (float)(100),
            };
            LightSystem.Lights.Add(PlayerLight);
            LightSystem.Lights.Add(SunLight);

        }

        public void PostInit()
        {
            Generator.RegisterGenerator(new BasicTerrainGenerator(this));

            RegisterRecipes(this);

            this.GetCurrentPlayer().AddToInv(new ItemStack(Block.GetBlock(5), 64));
            this.GetCurrentPlayer().AddToInv(new ItemStack(Block.GetBlock(1001), 64));

        }

        #endregion
        #region Updating
        //====================================
        //========== UPDATING ================
        //====================================

        public void Update(GameTime GameTime)
        {
            HandleWorldTime();
            LightSystem.Update(GameTime);

            
            // Chunks to render 
            // 10 chunks x axis
            // 8 chunks y axis
            PhysicalWorld.Step(0.033333f);

            TestPlayer.Update(GameTime, CameraPos);

            // Calc camera - chunks vector
            Vector2 CameraChunks = new Vector2((int)(CameraPos.X / (TileWidth * ChunkSize)), (int)(CameraPos.Y / (TileHeight * ChunkSize))) - new Vector2(1, 1);

            for (int y = 0; y < ChunksY; y++)
            {
                for (int x = 0; x < ChunksX; x++)
                {
                    ChunksToUpdate[x, y] = new Vector2(x , y ) + CameraChunks;
                }
            }


            foreach (Vector2 V in ChunksToUpdate)
            {
                if (!Chunks.ContainsKey(V))
                {
                    Generator.Generate(V);
                    Chunks[V].IsLoaded = true;
                }
                else
                {
                    if (!Chunks[V].IsLoaded)
                    {
                        Chunks[V].Load();
                    }
                    Chunks[V].IsLoaded = true;
                    if (Chunks[V].UpdateRequested)
                    {
                        Chunks[V].Update(GameTime);
                        Chunks[V].UpdateRequested = false;
                    }
                    // Update
                }
            }
            //Unloading

            foreach (KeyValuePair<Vector2, Chunk> C in Chunks)
            {
                if (!C.Value.IsLoaded)
                {
                    if (C.Value.WasLoaded)
                    {
                        //C.Value.Unload();
                    }
                }
            }


            foreach (KeyValuePair<Vector2, Chunk> C in Chunks)
            {
                C.Value.WasLoaded = C.Value.IsLoaded;
                C.Value.IsLoaded = false;
            }

            foreach (Entity E in Entities)
            {
                E.Update(GameTime);
            }

            //Fluid.update(CameraPos);
        }

        private void HandleWorldTime()
        {
            int TimeStep = 10;// Change dat Shyt

            Time += TimeStep;
            Time = Time % 24000;

            var ColorVal = MathHelper.Clamp((float)Math.Sin((Time * Math.PI) / 12000) + 0.2f, -1, 1);
            SunLight.Intensity = MathHelper.Clamp(ColorVal, 0, 1) + 0.3f;

            if (CurrentLightTime <= 0)
            {
                CurrentColorId = (CurrentColorId + 1) % LightColorPath.Count;
                CurrentLightTime = LightColorPath[CurrentColorId].Key;
            }
            CurrentLightTime -= TimeStep;
            float Difference = (float)CurrentLightTime / (float)LightColorPath[CurrentColorId].Key;

            SkyColor = Helper.AddColors(LightColorPath[CurrentColorId].Value * Difference, LightColorPath[(CurrentColorId + 1) % LightColorPath.Count].Value * (1 - Difference));
        }



        public void UpdateBlock(Vector2 Pos)
        {
            Vector2 ChunkV = new Vector2((int)Math.Floor(Pos.X / ChunkSize), (int)Math.Floor((Pos.Y / ChunkSize)));


            if (this.Chunks.ContainsKey(ChunkV))
            {
                int PosX = (int)Math.Floor(Pos.X) - (int)ChunkV.X * ChunkSize;
                int PosY = (int)Math.Floor(Pos.Y) - (int)ChunkV.Y * ChunkSize;
                if (PosX < 0) PosX = 15 - PosX;
                if (PosY < 0) PosY = 15 - PosY;

                Chunks[ChunkV].CheckEdges(PosX, PosY);
                Chunks[ChunkV].SetPhysics(PosX, PosY);
            }
            else
            {
            }
        }

        public void UpdateBackgroundBlock(Vector2 Pos)
        {
            Vector2 ChunkV = new Vector2((int)Math.Floor(Pos.X / ChunkSize), (int)Math.Floor((Pos.Y / ChunkSize)));


            if (this.Chunks.ContainsKey(ChunkV))
            {
                int PosX = (int)Math.Floor(Pos.X) - (int)ChunkV.X * ChunkSize;
                int PosY = (int)Math.Floor(Pos.Y) - (int)ChunkV.Y * ChunkSize;
                if (PosX < 0) PosX = 15 - PosX;
                if (PosY < 0) PosY = 15 - PosY;

                Chunks[ChunkV].CheckEdgesBackground(PosX, PosY);
            }
            else
            {
            }
        }

        public void UpdateBlock(int x, int y)
        {
            UpdateBlock(new Vector2(x, y));
        }

        public void UpdateBackgroundBlock(int x, int y)
        {
            UpdateBackgroundBlock(new Vector2(x, y));
        }

        #endregion
        #region Drawing
        //====================================
        //========== DRAW ====================
        //====================================

        public void Draw(SpriteBatch SpriteBatch, GameTime GameTime)
        {
            CameraPos = TestPlayer.PlayerChar.Position - GeneralManager.GetPartialVector(0.5f, 0.5f);

            LightSystem.LightMapPrepare();
            SpriteBatch.Draw(GeneralManager.Textures["Textures/SkyGradient"], GeneralManager.GetPartialRect(0,0,1,1), SkyColor);
            SpriteBatch.Draw(GeneralManager.Textures["Textures/Sun"], GeneralManager.GetPartialVector(0.4f, -(float)Math.Sin((Time * Math.PI)/12000)) + new Vector2(0, - CameraPos.Y - 600), Color.White);
            foreach (Vector2 V in ChunksToUpdate)
            {
                if (Chunks.ContainsKey(V))
                {
                    Chunks[V].DrawBackgroundBlocks(SpriteBatch, V * new Vector2(TileWidth * ChunkSize, TileHeight * ChunkSize) - CameraPos);
                }
            }


            //SpriteBatch.GraphicsDevice.Viewport.Project(Vector3.Zero, Matrix.Identity, Matrix.Identity, Matrix.Identity);
            TestPlayer.Draw(SpriteBatch, CameraPos);

            foreach (Vector2 V in ChunksToUpdate)
            {
                if (Chunks.ContainsKey(V))
                {
                    Chunks[V].Draw(SpriteBatch, V * new Vector2(TileWidth * ChunkSize, TileHeight * ChunkSize) - CameraPos);
                }
            }



            foreach (Entity E in Entities)
            {
                E.Draw(SpriteBatch, CameraPos);
            }

            //Fluid.draw(SpriteBatch, CameraPos);

            Matrix view = Matrix.CreateTranslation(new Vector3(-2 *CameraPos.X / GeneralManager.ScreenX, 2 * CameraPos.Y / GeneralManager.ScreenY, 0) );

            PlayerLight.Position = new Vector2(2 * GetCurrentPlayer().Position.X / GeneralManager.ScreenX - 1,1 -( 2 * GetCurrentPlayer().Position.Y - 30) / GeneralManager.ScreenY);//2 * GetCurrentPlayer().Position / new Vector2(GeneralManager.ScreenX,GeneralManager.ScreenY);
            SunLight.Position = new Vector2(PlayerLight.Position.X, 100);


            // Assign the matrix and pre-render the lightmap.
            // Make sure not to change the position of any lights or shadow hulls after this call, as it won't take effect till the next frame!
            LightSystem.Matrix = view;
            LightSystem.Bluriness = 3;
            LightSystem.Draw(GameTime);


            GeneralManager.Fonts["Fonts/SteamWreck"].DrawText(SpriteBatch, 10, 10, GetCurrentPlayer().Position.X + " : " + GetCurrentPlayer().Position.Y);
        }

        #endregion
        #region WorldAcces
        //====================================
        //========== WORLD ACCESS ============
        //====================================

        public Block GetBlock(int x, int y)
        {
            Vector2 ChunkV = WorldHelper.GetChunkPos(new Vector2(x, y));

            if (this.Chunks.ContainsKey(ChunkV))
            {
                Vector2 PosInChunk = WorldHelper.GetBlockInChunkPos(new Vector2(x, y));
                return Chunks[ChunkV].Blocks[(int)PosInChunk.X, (int)PosInChunk.Y];
            }
            else
            {
                return Block.GetBlock(0);
            }
        }


        public TileEntity GetBlockTE(int x, int y)
        {

            Vector2 ChunkV = WorldHelper.GetChunkPos(new Vector2(x, y));

            if (this.Chunks.ContainsKey(ChunkV))
            {
                Vector2 PosInChunk = WorldHelper.GetBlockInChunkPos(new Vector2(x, y));
                return Chunks[ChunkV].TileEntities[(int)PosInChunk.X, (int)PosInChunk.Y];
            }
            else
            {
                return null;
            }
        }

        public TileEntity GetBlockTE(Vector2 Vec)
        {
            return GetBlockTE((int)Vec.X, (int)Vec.Y);
        }

        public Block GetBackgroundBlock(int x, int y)
        {
            Vector2 ChunkV = WorldHelper.GetChunkPos(new Vector2(x, y));

            if (this.Chunks.ContainsKey(ChunkV))
            {
                return Chunks[ChunkV].BackgroundBlocks[x - (int)ChunkV.X * ChunkSize, y - (int)ChunkV.Y * ChunkSize];
            }
            else
            {
                return Block.GetBlock(0);
            }
        }

        public Block GetBlock(Vector2 V)
        {
            return GetBlock((int)Math.Floor(V.X), (int)Math.Floor(V.Y));
        }

        public bool CheckTileCollision(Vector2 Vect)
        {
            return GetBlock((Vect)/ new Vector2(TileWidth, TileHeight)).IsSolid;
        }

        public void SetBlock(Vector2 Vect, Block B)
        {
            Vector2 ChunkV = WorldHelper.GetChunkPos(Vect);

            if (this.Chunks.ContainsKey(ChunkV))
            {
                int PosX = (int)Math.Floor(Vect.X) - (int)ChunkV.X * ChunkSize;
                int PosY = (int)Math.Floor(Vect.Y) - (int)ChunkV.Y * ChunkSize;
                if (PosX < 0) PosX = 15 - PosX;
                if (PosY < 0) PosY = 15 - PosY;
                
                Chunks[ChunkV].Blocks[PosX,PosY] = B;
                Chunks[ChunkV].BlockState[PosX, PosY] = 0;
                Chunks[ChunkV].RemoveShadow(PosX, PosY);
                if (Chunks[ChunkV].TileEntities[PosX, PosY] != null)
                {
                    Chunks[ChunkV].TileEntities[PosX, PosY].Kill();
                }


                if (B.IsSolid)
                {
                    Chunks[ChunkV].CreateShadow(PosX, PosY);
                }

                UpdateBlock((int)Math.Floor(Vect.X), (int) Math.Floor(Vect.Y));
                UpdateBlock((int)Math.Floor(Vect.X) - 1, (int) Math.Floor(Vect.Y));
                UpdateBlock((int)Math.Floor(Vect.X) + 1, (int)Math.Floor(Vect.Y));
                UpdateBlock((int)Math.Floor(Vect.X), (int)Math.Floor(Vect.Y) - 1);
                UpdateBlock((int)Math.Floor(Vect.X), (int)Math.Floor(Vect.Y) + 1);

                Chunks[ChunkV].CalcLights();
            }
            else
            {
                //NOPE
            }
        }


        public void SetBlock(Vector2 Vect, TileEntity TE)
        {

            Vector2 ChunkV = WorldHelper.GetChunkPos(Vect);

            if (this.Chunks.ContainsKey(ChunkV))
            {
                TE.Position = Vect;

                if (TE is IMultiBlockTE) // if TE is multiblock entity
                {
                    MultiBlockDef MultiBlock = (TE as IMultiBlockTE).GetMultiBlockDef();
                    //Vector2 Center = (TE as IMultiBlockTE).GetCenterCoord();
                    if ((TE as IMultiBlockTE).CanBePlaced(Vect))
                    {
                        for (int y = 0; y < MultiBlock.Size.Y; y++)
                        {
                            for (int x = 0; x < MultiBlock.Size.X; x++)
                            {
                                SetBlock(Vect + new Vector2(x, y), MultiBlock.BlockTable[x, y]);
                                SetTileEntity(Vect + new Vector2(x, y), TE);

                                //Chunks[ChunkV].TileEntities[(int)PosInChunk.X + x, (int)PosInChunk.Y + y] = TE;
                            }
                        }
                    }
                }
                else // if TE is single block
                {
                    SetBlock(Vect, TE.TileBlock);
                    Vector2 PosInChunk = WorldHelper.GetBlockInChunkPos(Vect);

                    Chunks[ChunkV].TileEntities[(int)PosInChunk.X, (int)PosInChunk.Y] = TE;
                }
            }
        }

        public void SetTileEntity(Vector2 Position, TileEntity TE)
        {
            Vector2 ChunkV = WorldHelper.GetChunkPos(Position);

            if (Chunks.ContainsKey(ChunkV))
            {
                Vector2 TEPos = WorldHelper.GetBlockInChunkPos(Position);
                Chunks[ChunkV].TileEntities[(int)TEPos.X, (int) TEPos.Y] = TE;
            }
        }

        public void SetBackgroundBlock(Vector2 Vect, Block B)
        {
            Vector2 ChunkV = new Vector2((int)Math.Floor((double)Vect.X / ChunkSize), (int)Math.Floor((double)Vect.Y / ChunkSize));

            if (this.Chunks.ContainsKey(ChunkV))
            {
                int PosX = (int)Math.Floor(Vect.X) - (int)ChunkV.X * ChunkSize;
                int PosY = (int)Math.Floor(Vect.Y) - (int)ChunkV.Y * ChunkSize;
                if (PosX < 0) PosX = 15 - PosX;
                if (PosY < 0) PosY = 15 - PosY;
                Chunks[ChunkV].BackgroundBlocks[PosX, PosY] = B;
                Chunks[ChunkV].BackgroundBlockState[PosX, PosY] = 0;

                UpdateBackgroundBlock((int)Math.Floor(Vect.X), (int)Math.Floor(Vect.Y));
                UpdateBackgroundBlock((int)Math.Floor(Vect.X) - 1, (int)Math.Floor(Vect.Y));
                UpdateBackgroundBlock((int)Math.Floor(Vect.X) + 1, (int)Math.Floor(Vect.Y));
                UpdateBackgroundBlock((int)Math.Floor(Vect.X), (int)Math.Floor(Vect.Y) - 1);
                UpdateBackgroundBlock((int)Math.Floor(Vect.X), (int)Math.Floor(Vect.Y) + 1);
            }
            else
            {
                //NOPE
            }
        }

        public Rectangle? CheckTileCollisionIntersect(Vector2 Vec, Rectangle Rect)
        {

            Vec.X = (int)Vec.X;
            Vec.Y = (int)Vec.Y;

            if (Vec.X < 0) Vec.X -= 32;
            if (Vec.Y < 0) Vec.Y -= 32;

            Vector2 tilePosition = new Vector2((int)(Vec.X / TileWidth), (int)(Vec.Y / TileHeight));

            Block collisionTile = GetBlock((int)tilePosition.X, (int)tilePosition.Y);

            if (!collisionTile.IsSolid)
                return null;
            else return Rectangle.Intersect(Rect, new Rectangle((int)tilePosition.X * TileWidth , (int)tilePosition.Y * TileHeight , TileWidth, TileHeight));
            
        }


        #endregion
        #region Constants

        //====================================
        //========== CONSTANTS ===============
        //====================================

        public static int TileWidth
        {
            get
            {
                return 32;
            }
        }

        public static int TileHeight
        {
            get
            {
                return 32;
            }
        }

        public static int ChunkSize
        {
            get
            {
                return 16;
            }
        }

        

        public bool RaycastAny(Vector2 Point1, Vector2 Point2)
        {
            return false;
        }

        public Player GetCurrentPlayer()
        {
            //foreach (Player P in Players)
            //{
            //}
            return TestPlayer;
        }

        #endregion
        #region HandleInput

        public bool HandleInput()
        {
            bool Value = false;
            Value = TestPlayer.HandleInput();
            return Value;
        }

        #endregion
    }
}
