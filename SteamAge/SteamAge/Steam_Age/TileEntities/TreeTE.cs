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
using VAPI;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using System.Diagnostics;
using VAPI.FluidSim;
using Krypton;
using Krypton.Lights;

namespace SteamAge.TileEntities
{
    public class TreeTE : TileEntity, IMultiBlockTE
    {
        MultiBlockDef MultiBlockDef;

        public TreeTE(GameWorld GameWorld, Vector2 Position)
            : base(GameWorld, "Tree")
        {
            this.Entity = new TreeEntity(GameWorld);
            this.TileBlock = Block.GetBlock(1002); 
            this.Name = "Textures/TileEntities/Tree";
            this.Position = Position;

            MultiBlockDef = new MultiBlockDef(new Vector2(3, 10), new Vector2(1, 9));
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    MultiBlockDef.SetBlock(x, y, this.TileBlock);
                }
            }
        }

        public Vector2 GetCenterCoord()
        {
            return Position;
        }

        public MultiBlockDef GetMultiBlockDef()
        {
            return this.MultiBlockDef;
        }

        public bool CanBePlaced(Vector2 BlockPos)
        {
            for (int y = 0; y < MultiBlockDef.Size.Y; y++)
            {
                for (int x = 0; x < MultiBlockDef.Size.X; x++)
                {
                    if (World.GetBlock(new Vector2(x, y) + Position) != Block.GetBlock(0))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public override void Kill()
        {
            for (int y = 0; y < MultiBlockDef.Size.Y; y++)
            {
                for (int x = 0; x < MultiBlockDef.Size.X; x++)
                {
                    World.SetTileEntity(new Vector2(x, y) + Position, null);
                    World.SetBlock(new Vector2(x ,y) + Position,  Block.GetBlock(0));
                }
            }

            base.Kill();
        }
    }

    public class TreeBlock : Block, IEntityBlock
    {
        public TreeBlock()
            : base()
        {
            IsSolid = false;
            Family = "Tree";
            Tex = "Textures/TileEntities/Tree";
            Id = 1002;
            Textures[0] = new KeyValuePair<Block.BlockState, string>((Block.BlockState)0, "Textures/TileEntities/Tree");

            Drop.AddDrop(1, 0, this);
        }

        public TileEntity GetNewTE(GameWorld World, Vector2 Position)
        {
            return new TreeTE(World, Position);
        }

        public override void Draw(SpriteBatch SpriteBatch, Vector2 Position, Color Color, int State)
        {
            SpriteBatch.Draw(GeneralManager.Textures[this.Tex], Position - new Vector2(32,32), Color);
            //base.Draw(SpriteBatch, Position);
        }
    }

    public class TreeEntity : Entity
    {
        public TreeEntity(GameWorld GameWorld)
            : base(GameWorld)
        {
        }
    }
}
