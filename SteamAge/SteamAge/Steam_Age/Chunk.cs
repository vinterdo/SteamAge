using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
using FarseerPhysics.Common.PolygonManipulation;
using Krypton;
using Krypton.Lights;

namespace SteamAge
{
    public class Chunk
    {
        public Block[,] Blocks = new Block[GameWorld.ChunkSize, GameWorld.ChunkSize];
        public Block[,] BackgroundBlocks = new Block[GameWorld.ChunkSize, GameWorld.ChunkSize];
        public Fixture[,] BlockFixtures = new Fixture[GameWorld.ChunkSize, GameWorld.ChunkSize];
        public ShadowHull[,] ShadowHulls = new ShadowHull[GameWorld.ChunkSize, GameWorld.ChunkSize];
        public float[,] LightValues = new float[GameWorld.ChunkSize, GameWorld.ChunkSize];
        public TileEntity[,] TileEntities = new TileEntity[GameWorld.ChunkSize, GameWorld.ChunkSize];
        //public List<Fixture> Fixtures = new List<Fixture>();

        public Vector2 Position;
        public bool IsLoaded;
        public bool WasLoaded = false;
        public int[,] BlockState;
        public int[,] BackgroundBlockState;
        GameWorld World;
        public bool UpdateRequested = true;

        public Texture2D BackgroundTex;

        public Chunk(GameWorld World, Vector2 Pos)
        {
            this.Position = Pos;
            for (int y = 0; y < GameWorld.ChunkSize; y++)
            {
                for (int x = 0; x < GameWorld.ChunkSize; x++)
                {
                    Blocks[x,y] = Block.GetBlock(2);
                    BackgroundBlocks[x, y] = (Position.Y < 0 )? Block.GetBlock(0) : Block.GetBlock(2);
                    
                }
            }
            this.World = World;
            BlockState = new int[GameWorld.ChunkSize, GameWorld.ChunkSize];
            BackgroundBlockState = new int[GameWorld.ChunkSize, GameWorld.ChunkSize];

        }

        public void Draw(SpriteBatch SpriteBatch, Vector2 Position)
        {
            for (int y = 0; y < GameWorld.ChunkSize; y++)
            {
                for (int x = 0; x < GameWorld.ChunkSize; x++)
                {
                    SpriteBatch.Draw(GeneralManager.Textures[Blocks[x, y].Textures[BlockState[x, y]].Value], new Rectangle((int)Position.X + GameWorld.TileWidth * x, (int)Position.Y + GameWorld.TileHeight * y, GameWorld.TileWidth, GameWorld.TileHeight)/*Position + new Vector2(GameWorld.TileWidth * x, GameWorld.TileHeight * y)*/, new Color(LightValues[x, y], LightValues[x, y], LightValues[x, y]));
                }
            }
        }

        public void CalcLights()
        {
            for (int y = 0; y < GameWorld.ChunkSize; y++)
            {
                for (int x = 0; x < GameWorld.ChunkSize; x++)
                {
                    if (Blocks[x, y].IsSolid)
                    {
                        CalcLightRadius(y, x);
                    }
                    else
                    {
                        LightValues[x, y] = 1f;
                    }

                }
            }
        }

        private void CalcLightRadius(int y, int x)
        {
            int Radius = 1;
            LightValues[x, y] = 0.3f;
            while (Radius <= 3)
            {
                for (int x1 = -Radius; x1 < Radius; x1++)
                {
                    for (int y1 = -Radius; y1 < Radius; y1++)
                    {
                        if (x1 == 0 && y1 == 0)
                        {
                            continue;
                        }
                        else
                        {
                            if (!World.GetBlock(x + x1 + GameWorld.ChunkSize * (int)Position.X, y + y1 + GameWorld.ChunkSize * (int)Position.Y).IsSolid)
                            {
                                LightValues[x, y] += 0.2f / (Radius * Radius);
                                LightValues[x, y] = MathHelper.Clamp(LightValues[x, y], 0.3f, 0.5f);
                            }
                        }
                    }
                }
                Radius++;
            }
        }

        public void DrawBackgroundBlocks(SpriteBatch SpriteBatch, Vector2 Position)
        {
            if (BackgroundTex != null)
                SpriteBatch.Draw(BackgroundTex, Position, Color.White);

            for (int y = 0; y < GameWorld.ChunkSize; y++)
            {
                for (int x = 0; x < GameWorld.ChunkSize; x++)
                {
                    if (!Blocks[x, y].IsSolid)
                    {
                        SpriteBatch.Draw(GeneralManager.Textures[BackgroundBlocks[x, y].Textures[BackgroundBlockState[x, y]].Value], new Rectangle((int)Position.X + GameWorld.TileWidth * x, (int)Position.Y + GameWorld.TileHeight * y, GameWorld.TileWidth, GameWorld.TileHeight)/*Position + new Vector2(GameWorld.TileWidth * x, GameWorld.TileHeight * y)*/, Color.DarkGray);
                    }
                }
            }
        }

        public void Update(GameTime GameTime)
        {
            //Edges

            for (int y = 0; y < GameWorld.ChunkSize; y++)
            {
                for (int x = 0; x < GameWorld.ChunkSize; x++)
                {
                    CheckEdges(x, y);
                    CheckEdgesBackground(x, y);

                    SetPhysics(x, y);
                    //SetPhysics(x, y);
                }
            }

            GenerateLightHulls();
            CalcLights();
        }

        public void CheckEdges( int x, int y)
        {
            if (this.Blocks[x, y].IsSolid)
            {
                int State = 0;

                //if (!C.Blocks[x, y - 1].IsSolid)
                if (World.GetBlock(x + (int)this.Position.X * GameWorld.ChunkSize, y - 1 + (int)this.Position.Y * GameWorld.ChunkSize).Family != Blocks[x, y].Family)
                {
                    State += 1;
                }

                //if (!this.Blocks[x, y + 1].IsSolid)
                if (World.GetBlock(x + (int)this.Position.X * GameWorld.ChunkSize, y + 1 + (int)this.Position.Y * GameWorld.ChunkSize).Family != Blocks[x, y].Family)
                {
                    State += 4;
                }

                //if (!this.Blocks[x + 1, y].IsSolid)
                if (World.GetBlock(x + 1 + (int)this.Position.X * GameWorld.ChunkSize, y + (int)this.Position.Y * GameWorld.ChunkSize).Family != Blocks[x, y].Family)
                {
                    State += 2;
                }

                //if (!this.Blocks[x - 1, y].IsSolid)
                if (World.GetBlock(x - 1 + (int)this.Position.X * GameWorld.ChunkSize, y + (int)this.Position.Y * GameWorld.ChunkSize).Family != Blocks[x, y].Family)
                {
                    State += 8;
                }

                //C.Blocks[x, y].State = (Block.BlockState)State; // Głupota miesiąca
                this.BlockState[x, y] = State;
            }
        }

        public void GenerateLightHulls()
        {
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    if (Blocks[x, y].IsSolid)
                    {
                        RemoveShadow(x, y);
                        CreateShadow(x, y);
                    }
                }
            }
        }

        public void CreateShadow(int x, int y)
        {
            ShadowHull Hull = GetHull(new Vector2(x * GameWorld.TileWidth, -y * GameWorld.TileHeight) + new Vector2(Position.X * GameWorld.TileWidth * GameWorld.ChunkSize, -Position.Y * GameWorld.TileHeight * GameWorld.ChunkSize) + new Vector2(-GeneralManager.ScreenX / 2, GeneralManager.ScreenY / 2) - new Vector2(-GameWorld.TileWidth / 2f, GameWorld.TileHeight / 2f));
            ShadowHulls[x, y] = Hull;
            World.LightSystem.Hulls.Add(Hull);
        }

        

        public ShadowHull GetHull(Vector2 Position)
        {
            var Hull = ShadowHull.CreateRectangle(new Vector2((2f / GameWorld.TileWidth* ((float)GeneralManager.ScreenY/(float)GeneralManager.ScreenX)), 2f / GameWorld.TileHeight));
            Hull.Position = 2 * Position/ GeneralManager.GetPartialVector(1,1);
            Hull.Visible = true;
            return Hull;
        }

        public void CheckEdgesBackground(int x, int y)
        {
            if (this.BackgroundBlocks[x, y].IsSolid)
            {
                int State = 0;

                if (!World.GetBackgroundBlock(x + (int)this.Position.X * GameWorld.ChunkSize, y - 1 + (int)this.Position.Y * GameWorld.ChunkSize).IsSolid)
                {
                    State += 1;
                }

                if (!World.GetBackgroundBlock(x + (int)this.Position.X * GameWorld.ChunkSize, y + 1 + (int)this.Position.Y * GameWorld.ChunkSize).IsSolid)
                {
                    State += 4;
                }

                if (!World.GetBackgroundBlock(x + 1 + (int)this.Position.X * GameWorld.ChunkSize, y + (int)this.Position.Y * GameWorld.ChunkSize).IsSolid)
                {
                    State += 2;
                }

                //if (!this.Blocks[x - 1, y].IsSolid)
                if (!World.GetBackgroundBlock(x - 1 + (int)this.Position.X * GameWorld.ChunkSize, y + (int)this.Position.Y * GameWorld.ChunkSize).IsSolid)
                {
                    State += 8;
                }

                //C.Blocks[x, y].State = (Block.BlockState)State; // Głupota miesiąca
                this.BackgroundBlockState[x, y] = State;
            }
        }

        public void RemoveShadow(int x, int y)
        {
            World.LightSystem.Hulls.Remove(ShadowHulls[x, y]);
        }

        public void Unload()
        {
            
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    /*
                    if (BlockFixtures[x, y] != null)
                    {
                        World.PhysicalWorld.RemoveBody(BlockFixtures[x, y].Body);
                        BlockFixtures[x, y] = null;
                    }*/
                }
            }
        }

        public void Load()
        {
            IsLoaded = true;
        }


        public void SetPhysics(int x, int y)
        {
            if (BlockFixtures[x, y] != null)
            {
                World.PhysicalWorld.RemoveBody(BlockFixtures[x, y].Body);
                BlockFixtures[x, y] = null;
            }
            if (Blocks[x, y].IsSolid)
            {
                Body BodyDec = new Body(World.PhysicalWorld);
                BodyDec.BodyType = BodyType.Static;

                BlockFixtures[x, y] = BodyDec.CreateFixture(Blocks[x, y].Shape);
                BlockFixtures[x, y].Body.Position = this.Position * new Vector2(GameWorld.TileWidth, GameWorld.TileHeight) * GameWorld.ChunkSize + new Vector2(x, y) * new Vector2(GameWorld.TileWidth, GameWorld.TileHeight);
            }
        }
    }
}