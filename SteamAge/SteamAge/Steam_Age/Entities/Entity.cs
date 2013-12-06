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
//using Box2D.XNA;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace SteamAge
{
    public class Entity
    {
        protected GameWorld World;
        public string Name;

        public Entity(GameWorld World)
        {
            this.World = World;
        }

        public virtual void Update(GameTime GameTime)
        {
        }

        public virtual void Draw(SpriteBatch SpriteBatch, Vector2 CameraPos)
        {
        }

        public virtual void Initalize()
        {
        }
    }
}
