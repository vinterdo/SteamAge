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
    public class MenuComponent:GUIComponent
    {
        //public string[] Values;
        public List<Button> Buttons;
        public List<Label> Labels;
        public int CurrentId = 0;
        public Rectangle Position;

        public MenuComponent(string[] Values, Rectangle Position, string ButtonBgName, Color UnActiveColor, Color ActiveColor, string FontName, Vector2 LabelOffset)
        {
            Buttons = new List<Button>();
            Labels = new List<Label>();
            //this.Values = Values;
            this.Position = Position;

            for (int i = 0; i < Values.Length; i++)
            {
                Rectangle ButtonPos = new Rectangle((int)(Position.X), (int)(Position.Y + i * Position.Height / Values.Length), (int)(Position.Width), (int)(Position.Height / Values.Length));
                Button Button = new Button(ButtonPos, "", GeneralManager.Textures[ButtonBgName], UnActiveColor, ActiveColor, FontName);
                Buttons.Add(Button);

                Label L = new Label();
                L.Font = GeneralManager.Fonts[FontName];
                L.Name = Values[i];
                L.Position = new Rectangle(ButtonPos.X + (int)LabelOffset.X, ButtonPos.Y + (int)LabelOffset.Y, ButtonPos.Width - 2 * (int)LabelOffset.X, ButtonPos.Height - 2 * (int)LabelOffset.Y);
                Labels.Add(L);
            }
        }

        public override void Draw(SpriteBatch SpriteBatch)
        {
            foreach (Button B in Buttons)
            {
                B.Draw(SpriteBatch);
            }

            foreach (Label L in Labels)
            {
                L.Draw(SpriteBatch);
            }
        }

        public override bool CheckActive()
        {
            IsActive = false;
            foreach (Button B in Buttons)
            {
                if (B.CheckActive())
                {
                    IsActive = true;
                }
            }
            return IsActive;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Button B in Buttons)
            {
                B.IsActive = IsActive;
                B.Update(gameTime);
            }
        }

        public override bool HandleInput()
        {
            foreach (Button B in Buttons)
            {
                if (B.HandleInput())
                {
                    return true;
                }
            }
            return false;
        }
        

    }
}
