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
    class DynamicBody : Entity
    {
        Fixture BodyFixture;
        Texture2D Texture;
        
        public DynamicBody(GameWorld World, Shape Shape, Vector2 Position, string TexName):base(World)
        {
            Body BodyDec;
            BodyDec = new Body(World.PhysicalWorld);
            BodyDec.BodyType = BodyType.Dynamic;
            BodyFixture = BodyDec.CreateFixture(Shape);
            BodyFixture.Body.Position = Position;
            Texture = GeneralManager.Textures[TexName];
        }

        public override void Draw(SpriteBatch SpriteBatch, Vector2 CameraPos)
        {
            AABB test;
            BodyFixture.GetAABB(out test, 0);
            Rectangle BoundingRect = new Rectangle((int)BodyFixture.Body.Position.X - (int)Math.Abs(test.LowerBound.X - test.UpperBound.X) / 2 - (int)CameraPos.X, (int)BodyFixture.Body.Position.Y - (int)Math.Abs(test.LowerBound.Y - test.UpperBound.Y) / 2 - (int)CameraPos.Y, (int)Math.Abs(test.LowerBound.X - test.UpperBound.X), (int)Math.Abs(test.LowerBound.Y - test.UpperBound.Y));
            SpriteBatch.Draw(Texture, BoundingRect/*BodyFixture.Body.Position*/, null, Color.White, BodyFixture.Body.Rotation, BodyFixture.Body.LocalCenter, SpriteEffects.None, 0.5f);
        }
    }
}
