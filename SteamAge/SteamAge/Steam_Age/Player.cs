using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Dynamics.Joints;
using VAPI;

namespace SteamAge
{
    public class Player
    {
        public Vector2 Position
        {
            get
            {
                if (PlayerChar != null)
                {
                    return this.PlayerChar.Position;
                }
                else
                {
                    return Vector2.Zero;
                }
            }
            set
            {
                if (PlayerChar != null)
                {
                    this.PlayerChar.Position = value;
                }
            }
        }

        //public string TexName;
        public Character PlayerChar;
        GameWorld World;
        Vector2 Camera;
        public ItemSlot[] PlayerInv;
        public ItemSlot[] PlayerHotbar;
        public ItemStack HoldingStack;
        bool IsCurrentPlayer = false;
        public int SelectedStack = 0;
        

        public Player(GameWorld World, bool IsCurrent)
        {
            Position = new Vector2(100, - 1050);
            PlayerChar = new Character(Position, World);
            this.World = World;
            PlayerChar.LoadContent(GeneralManager.Content, GeneralManager.GDevice);

            PlayerInv = new ItemSlot[40];
            PlayerHotbar = new ItemSlot[10];

            this.IsCurrentPlayer = IsCurrent;

            if (IsCurrentPlayer)
            {
                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 10; x++)
                    {
                        PlayerInv[y * 10 + x] = new ItemSlot(World);
                        PlayerInv[y * 10 + x].Position = GeneralManager.GetPartialRect(0.3f + x * 0.04f, 0.3f + y * 0.04f, 0.037f,0.037f);
                        PlayerInv[y * 10 + x].Visible = true;
                        World.ParentScreen.EqWindow.AddGUI(PlayerInv[y * 10 + x]);
                    }
                }

                for (int x = 0; x < 10; x++)
                {
                    PlayerHotbar[x] = new ItemSlot(World);
                    PlayerHotbar[x].Position = GeneralManager.GetPartialRect(0.3f + x * 0.04f, 0.95f, 0.037f, 0.037f);
                    PlayerHotbar[x].Visible = true;
                    World.ParentScreen.AddGUI(PlayerHotbar[x]);
                }
            }


            PlayerInv[0].ItemStack = new ItemStack(Block.GetBlock(1), 12);
        }

        public void Update(GameTime GameTime, Vector2 Camera)
        {
            PlayerChar.Update(GameTime);
            this.Camera = Camera;
        }

        public void Draw(SpriteBatch SpriteBatch, Vector2 Camera)
        {
            PlayerChar.Draw(SpriteBatch, Matrix.CreateTranslation(new Vector3(-Camera.X, -Camera.Y, 0)));

        }

        public bool HandleInput()
        {
            bool Value = false;
            if (GeneralManager.CheckKey(Keys.W))
            {
                PlayerChar.Jump();
                Value = true;
            }
            if (GeneralManager.CheckKey(Keys.A))
            {
                PlayerChar.MoveLeftRight(-1);
                Value = true;
            }
            if (GeneralManager.CheckKey(Keys.D))
            {
                PlayerChar.MoveLeftRight(1);
                Value = true;
            }
            if (GeneralManager.CheckKey(Keys.LeftShift))
            {
                PlayerChar.Crouch();
                Value = true;
            }
            if (GeneralManager.IsLMBClicked())
            {
                PlayerChar.DestroyBlock((GeneralManager.MousePos + Camera)/ 32, this);
                Value = true;
            }
            if (GeneralManager.IsRMBClicked())
            {
                if (PlayerHotbar[SelectedStack].ItemStack != null && PlayerHotbar[SelectedStack].ItemStack.Item is Block)
                {
                    PlaceBlock();
                    Value = true;
                }
                else if (World.GetBlockTE(WorldHelper.GetBlockPos(GeneralManager.MousePos + Camera)) != null)
                {
                    UseBlock(WorldHelper.GetBlockPos(GeneralManager.MousePos + Camera));
                    Value = true;
                }
                else
                {
                    CatchBody();
                    Value = true;
                }
            }
            else
            {
                if (PlayerChar.CatchingJoint != null)
                {
                    World.PhysicalWorld.RemoveJoint(PlayerChar.CatchingJoint);
                    PlayerChar.CatchingJoint = null;
                    PlayerChar.CatchedFixture = null;
                }
            }
            return Value;
        }

        private void PlaceBlock()
        {
            if (PlayerChar.PlaceBlock((PlayerHotbar[SelectedStack].ItemStack.Item as Block), new Vector2((int)((GeneralManager.MousePos + Camera).X / 32), (int)((GeneralManager.MousePos + Camera).Y / 32))))
            {
                PlayerHotbar[SelectedStack].ItemStack.Count--;
                if (PlayerHotbar[SelectedStack].ItemStack.Count == 0)
                {
                    PlayerHotbar[SelectedStack].ItemStack = null;
                }
            }
        }

        private void CatchBody()
        {
            if (PlayerChar.CatchingJoint == null)
            {
                //Catching a body
                Fixture Fix = World.PhysicalWorld.TestPoint(GeneralManager.MousePos + Camera);
                if (Fix != null && Fix.Body.BodyType == BodyType.Dynamic)
                {
                    PlayerChar.CatchedFixture = Fix;
                    Fix.Body.Awake = true;
                    PlayerChar.CatchingJoint = JointFactory.CreateFixedDistanceJoint(World.PhysicalWorld, Fix.Body, Vector2.Zero, GeneralManager.MousePos + Camera);
                    (PlayerChar.CatchingJoint as FixedDistanceJoint).Length = 0f;
                    (PlayerChar.CatchingJoint as FixedDistanceJoint).DampingRatio = 2f;
                    (PlayerChar.CatchingJoint as FixedDistanceJoint).Frequency = 200;
                }
            }
            else
            {
                (PlayerChar.CatchingJoint as FixedDistanceJoint).WorldAnchorB = GeneralManager.MousePos + Camera;
            }
        }

        private void UseBlock(Vector2 BlockPos)
        {
            TileEntity TE = World.GetBlockTE(BlockPos);
            if (TE is TileEntities.IInventoryTE)
            {
                (TE as TileEntities.IInventoryTE).OpenGUI();
            }
        }

        private void UseBlock(int x, int y)
        {
            UseBlock(new Vector2(x, y));
        }

        public void AddToInv(ItemStack AddStack)
        {
            foreach (ItemSlot S in PlayerHotbar)
            {
                if (S.ItemStack == null)
                {
                    S.ItemStack = AddStack;
                    AddStack = null;
                }
                else // if there is something in slot
                {

                    if (AddStack.Item == S.ItemStack.Item)
                    {
                        if (AddStack.Count + S.ItemStack.Count <= ItemStack.MAX_STACK)
                        {
                            S.ItemStack.Count = AddStack.Count + S.ItemStack.Count;
                            AddStack = null;
                        }
                        else
                        {
                            AddStack.Count = AddStack.Count + S.ItemStack.Count - ItemStack.MAX_STACK;
                            S.ItemStack.Count = ItemStack.MAX_STACK;
                        }
                    }
                }

                if (AddStack == null || AddStack.Count == 0) break;
            }

            if (AddStack == null || AddStack.Count == 0) return;

            foreach (ItemSlot S in PlayerInv)
            {
                if (S.ItemStack == null)
                {
                    S.ItemStack = AddStack;
                    AddStack = null;
                    break;
                }
                else // if there is something in slot
                {

                    if (AddStack.Item == S.ItemStack.Item)
                    {
                        if (AddStack.Count + S.ItemStack.Count <= ItemStack.MAX_STACK)
                        {
                            S.ItemStack.Count = AddStack.Count + S.ItemStack.Count;
                            AddStack = null;
                        }
                        else
                        {
                            AddStack.Count = AddStack.Count + S.ItemStack.Count - ItemStack.MAX_STACK;
                            S.ItemStack.Count = ItemStack.MAX_STACK;
                        }
                    }
                }

                if (AddStack == null || AddStack.Count == 0) break;
            }
        }
    }
}
