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
    public class TextBox : GUIComponent
    {
        public TextBox(Rectangle Position, string BgTexName, string FontName)
        {
            this.Position = Position;
            this.BgTex = GeneralManager.Textures[BgTexName];
            this.Font = GeneralManager.Fonts[FontName];


            FocusedColor = Color.White;
            UnfocusedColor = Color.White;
        }

        public Rectangle Position;
        public string Text = "";
        Color FocusedColor;
        Color UnfocusedColor;
        Texture2D BgTex;
        FontRenderer Font;
        bool Enabled;

        public void SetColors(Color Focused, Color Unfocused)
        {
            this.FocusedColor = Focused;
            this.UnfocusedColor = Unfocused;
        }

        public void Measure()
        {
            //Position.Width = (int)Font.MeasureString(Text).X;
            //Position.Height = (int)Font.MeasureString(Text).Y;
        }

        public override bool HandleInput()
        {
            if (Enabled)
            {
                foreach (Keys k in Enum.GetValues(typeof(Keys)))
                {
                    if (GeneralManager.CheckKeyEdge(k))
                    {
                        if (k == Keys.Space) Text += " ";
                        else if (k == Keys.Back && Text.Length > 0) Text = Text.Remove(Text.Length - 1);
                        else if (k >= Keys.A && k <= Keys.Z)
                            Text += k.ToString();
                        else if (k >= Keys.D0 && k <= Keys.D9)
                            Text += k.ToString().ToCharArray(1, 1)[0].ToString();

                        switch (k)
                        {
                            case Keys.OemPeriod:
                                Text += ".";
                                break;
                            case Keys.OemComma:
                                Text += ",";
                                break;
                            case Keys.OemMinus:
                                Text += "-";
                                break;
                        }
                        return true;
                    }
                }
            }
            Measure();
            return false;
        }

        public override void Draw(SpriteBatch SpriteBatch)
        {
            if (Enabled)
            {
                SpriteBatch.Draw(BgTex, Position, FocusedColor);
                Font.DrawText(SpriteBatch, Position, Text, FocusedColor);
            }
            else
            {
                SpriteBatch.Draw(BgTex, Position, UnfocusedColor);
                Font.DrawText(SpriteBatch, Position, Text, UnfocusedColor);
                    
            }
        }

        public override bool CheckActive()
        {
            if (Helper.CheckLMBClick(Position))
            {
                IsActive = true;
                Enabled = true;
                return true;
            }
            if ( GeneralManager.IsLMBClicked() && !Helper.CheckIfInside(Position, GeneralManager.MousePos))
            {
                IsActive = false;
                Enabled = false;
                return false;
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
