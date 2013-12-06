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
using VAPI;

namespace SteamAge
{
    public class Block:Item
    {

        public static List<KeyValuePair<int, Block>> BlockRegistry = new List<KeyValuePair<int, Block>>();
        public string Name;
        public bool IsSolid;
        public BlockState State = BlockState.NoEdge;
        public Vertices ShapeVertices;
        public Shape Shape;
        public string Family;

        public KeyValuePair<BlockState, string>[] Textures;

        public DropDefinition Drop;

        public Block()
        {
            Textures = new KeyValuePair<BlockState, string>[16];
            ShapeVertices = new Vertices();
            Drop = new DropDefinition();
            
        }

        public enum BlockState
        {
            NoEdge = 0,
            Top = 1,
            Right = 2,
            Left = 8,
            Bot = 4,
            TopRight = 3,
            RightBot = 6,
            BotLeft = 12,
            LeftTop = 9,
            TopBot = 5,
            LeftRight = 10,
            RightBotLeft = 14,
            BotLeftTop = 13,
            LeftTopRight = 11,
            TopRightBot = 7,
            Full = 15
        }

        public static void RegisterBlock(Block B)
        {
            B.LoadTextures();
            B.CreateShape();
            BlockRegistry.Add(new KeyValuePair<int, Block>(B.Id, B));
        }

        public static void RegisterBlock(TileEntity TE)
        {
            TE.TileBlock.LoadTextures();
            TE.TileBlock.CreateShape();
            BlockRegistry.Add(new KeyValuePair<int, Block>(TE.TileBlock.Id, TE.TileBlock));
        }

        public override void DrawIcon(SpriteBatch SpriteBatch, Rectangle Position)
        {
            SpriteBatch.Draw(GeneralManager.Textures[this.Tex], new Rectangle(Position.X + 6, Position.Y + 6, Position.Width - 12, Position.Height - 12), Color.White);
        }

        public virtual void Draw(SpriteBatch SpriteBatch, Vector2 Position)
        {
            
            SpriteBatch.Draw(GeneralManager.Textures[this.Textures[(int)State].Value], Position, Color.White);
        }

        public void CreateShape()
        {
            if (ShapeVertices.Count > 2)
            {
                Shape = new PolygonShape(ShapeVertices, 1);
            }
        }

        public void LoadTextures()
        {
            foreach (KeyValuePair<BlockState, string> Tex in Textures)
            {
                if (Tex.Value != null)
                {
                    GeneralManager.LoadTex(Tex.Value);
                }
            }
        }
    }
}
