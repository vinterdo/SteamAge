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
    class TorchTE : TileEntity
    {
        public TorchTE(GameWorld GameWorld, Vector2 Position)
            : base(GameWorld, "Torch")
        {
            this.Entity = new TorchEntity(GameWorld, Position);
            this.Position = Position;

            this.TileBlock = Block.GetBlock(4);
            
            this.Name = "Textures/TileEntities/Torch";
        }


        public override void Kill()
        {
            World.LightSystem.Lights.Remove((this.Entity as TorchEntity).TorchLight);
        }
    }


    public class TorchEntity : Entity
    {
        public Light2D TorchLight;

        public TorchEntity(GameWorld GameWorld, Vector2 Position)
            : base(GameWorld)
        {

            Texture2D LightTexture = LightTextureBuilder.CreatePointLight(GeneralManager.GDevice, 512);
            TorchLight = new Light2D()
            {
                
                Texture = LightTexture,
                Range = (float)(0.5f),
                Color = new Color(1f, 0.5f, 0.1f, 1f),
                Intensity = 1f,
                Angle = MathHelper.TwoPi * (float)1,
                X = 2 * (Position.X + GameWorld.TileWidth/2) / GeneralManager.ScreenX - 1,
                Y = 1 - 2 * (Position.Y + GameWorld.TileHeight/2) / GeneralManager.ScreenY,
            };
            GameWorld.LightSystem.Lights.Add(TorchLight);
        }

    }

    public class TorchBlock : Block, IEntityBlock
    {
        public TorchBlock()
            : base()
        {

            IsSolid = false;
            Family = "Torch";
            Tex = "Textures/TileEntities/Torch";
            Id = 1000;

            Textures[0] = new KeyValuePair<Block.BlockState, string>((Block.BlockState)0, "Textures/TileEntities/Torch");


            Drop.AddDrop(1, 0, this);
        }

        public TileEntity GetNewTE(GameWorld World, Vector2 Position)
        {
            return new TorchTE(World, Position);
        }
    }
}
