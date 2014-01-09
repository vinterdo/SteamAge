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
    public class Button:GUIComponent
    {
        public Rectangle Position;
        public string Text;
        public Texture2D Background;
        public Color ButtonColor;
        public Color ActiveColor;
        public FontRenderer Font;

        public delegate void ButtonAction();

        public ButtonAction Action;

        public Button(Rectangle Position, string Text, Texture2D Background, Color ButtonColor, Color ActiveColor, string Font)
        {
            this.Position = Position;
            this.Text = Text;
            this.Background = Background;
            this.ButtonColor = ButtonColor;
            this.ActiveColor = ActiveColor;
            if (Font != null)
            {
                this.Font = GeneralManager.Fonts[Font];
            }
        }


        public override void Draw(SpriteBatch SpriteBatch)
        {
            // TODO Calc Scale
            //=======
            if (!IsActive)
            {
                SpriteBatch.Draw(Background, Position, ButtonColor);
                if (Font != null)
                {
                    Font.DrawText(SpriteBatch, Position, Text, Color.Gray);
                }
                //SpriteBatch.DrawString(Font, Text, Helper.GetTopLeftFromRect(Position), ButtonColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0.5f);
            }
            else
            {
                SpriteBatch.Draw(Background, Position, ActiveColor);
                if (Font != null)
                {
                    Font.DrawText(SpriteBatch, Position, Text, Color.Gray);
                }
                //SpriteBatch.DrawString(Font, Text, Helper.GetTopLeftFromRect(Position), ActiveColor, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0.5f);
            
            }
        }
        public override void Update(GameTime gameTime)
        {
            
        }

        public override bool CheckActive()
        {
            
            IsActive = Helper.CheckIfInside(Position, GeneralManager.MousePos);
            return IsActive;
        }

        public override bool HandleInput()
        {
            if (GeneralManager.CheckKeyEdge(Keys.Enter) && IsActive || Helper.CheckLMBClick(Position))
            {
                if (Action != null)
                {
                    Action();
                }
                return true;
            }
            return false;
        }
    }
}
