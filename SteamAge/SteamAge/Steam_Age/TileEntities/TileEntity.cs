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

namespace SteamAge
{
    public class TileEntity
    {
        public Entity Entity;
        public Block TileBlock;
        public Vector2 Position;
        protected GameWorld World;

        public TileEntity(GameWorld GameWorld, string Name)
            : base()
        {
            this.Name = Name;
            this.World = GameWorld;
        }

        public virtual void Initalize()
        {
            if (Entity != null)
            {
                Entity.Initalize();
                RegisterEntity();
            }
            else
            {
                Logger.Write("TileEntity " + this.Name + " has no entity registered, may be bit buggy");
            }

            
        }


        public string Name
        {
            get
            {
                if (Entity != null)
                {
                    return Entity.Name;
                }
                else
                {
                    return null;
                }

            }
            set
            {
                if (Entity != null)
                {
                    Entity.Name = value;
                }
                else
                {
                    //NOPE
                }
            }
        }


        public void RegisterEntity()
        {
            this.World.Entities.Add(this.Entity);
        }

        public virtual void Kill()
        {
            World.Entities.Remove(this.Entity);
        }
    }

}
