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
    public class RadioButton : GUIComponent
    {

        public List<CheckBox> Boxes;


        public RadioButton()
        {
            Boxes = new List<CheckBox>();
        }


        public override void Update(GameTime gameTime)
        {
            
        }

        public override bool HandleInput()
        {
            bool Handled = false;
            foreach (CheckBox C in Boxes)
            {
                if (Helper.CheckLMBClick(C.Position))
                {
                    foreach (CheckBox CB in Boxes)
                    {
                        CB.State = false;
                    }

                    C.State = true;
                    Handled = true;
                }
            }
            return Handled;
        }

        public override void Draw(SpriteBatch SpriteBatch)
        {
            foreach (CheckBox C in Boxes)
            {
                C.Draw(SpriteBatch);
            }
        }

        public override bool CheckActive()
        {
            foreach (CheckBox C in Boxes)
            {
                if (C.CheckActive())
                {
                    this.IsActive = true;
                }
            }

            return IsActive;
        }
    }
}
