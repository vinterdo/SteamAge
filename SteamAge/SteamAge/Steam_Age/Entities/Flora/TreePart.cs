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
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using System.Diagnostics;
using VAPI.FluidSim;
using Krypton;
using Krypton.Lights;
using SteamAge.Generators;

namespace SteamAge.Entities
{
    public class TreePart
    {
        public Fixture Fixture;
        public Texture2D Tex;
        private Vector2 _Size;
        public Vector2 Size
        {
            get
            {
                return _Size;
            }
        }

        public Vector2 Position
        {
            get
            {
                return Fixture.Body.Position;
            }
            set
            {
                Fixture.Body.Position = value;
            }
        }

        public TreePart(Vector2 Size, GameWorld GameWorld, Texture2D Tex) 
        {
            Body Body = BodyFactory.CreateBody(GameWorld.PhysicalWorld);
            Body.BodyType = BodyType.Dynamic;
            PolygonShape Shape = new PolygonShape(1);
            Shape.SetAsBox(Size.X / 2, Size.Y / 2);
            Fixture = Body.CreateFixture(Shape);
            this.Tex = Tex;
            _Size = Size;
        }

        public void Draw(SpriteBatch SpriteBatch, Vector2 Camera)
        {
            SpriteBatch.Draw(Tex, new Rectangle((int)(Fixture.Body.Position - Camera).X - 12, (int)(Fixture.Body.Position - Camera).Y - 12, 24, 24), null, Color.White, 0f, Vector2.One, SpriteEffects.None, 0.5f);
        }
    }
}
