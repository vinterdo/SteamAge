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

namespace VAPI
{
    public class Label : GUIComponent
    {
        public FontRenderer Font;
        public Rectangle Position;

        public string Name;

        public Label()
        {
            // TODO: Construct any child components here
        }


        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch SpriteBatch)
        {
            float ScaleX = 1f;
            float ScaleY = 1f;
            
            //ScaleX = Position.Width / Font.MeasureString(Name).X;
            //ScaleY = Position.Height / Font.MeasureString(Name).Y;

            if (ScaleX < ScaleY)
            {
                //float Offset = (Position.Height / 2f) - (Font.MeasureString(Name).Y * ScaleX) / 2;
                Font.DrawText(SpriteBatch, Position, Name, Color.White);
                //SpriteBatch.DrawString(Font, Name, new Vector2(Position.X, Position.Y + Offset), Color.White, 0.0f, Vector2.Zero, ScaleX, SpriteEffects.None, 0.0f);
            }
            else
            {
                //float Offset = (Position.Width / 2f) - (Font.MeasureString(Name).X * ScaleY) / 2;
                Font.DrawText(SpriteBatch, Position, Name, Color.White);
                //SpriteBatch.DrawString(Font, Name, new Vector2(Position.X + Offset, Position.Y), Color.White, 0.0f, Vector2.Zero, ScaleY, SpriteEffects.None, 0.0f);
            }
            
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool HandleInput()
        {
            return false;
        }
    }
}
