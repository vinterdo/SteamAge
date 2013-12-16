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

            MultiBlockDef = new MultiBlockDef(new Vector2(3, 10), Position);

        }

        public Vector2 GetCenterCoord()
        {
            return Position;
        }

        public MultiBlockDef GetMultiBlockDef()
        {
            return this.MultiBlockDef;
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
