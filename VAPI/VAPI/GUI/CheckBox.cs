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
    public class CheckBox : GUIComponent
    {
        public Texture2D TexOff;
        public Texture2D TexOn;
        public Rectangle Position;

        public bool State = false;


        public CheckBox(Rectangle Position, string TexOn, string TexOff, bool DefaultState)
        {
            this.Position = Position;
            this.TexOff = GeneralManager.Textures[TexOff];
            this.TexOn = GeneralManager.Textures[TexOn];
            State = DefaultState;
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
            if (Helper.CheckLMBClick(Position))
            {
                State = !State;
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch SpriteBatch)
        {

            if (!State)
            {
                SpriteBatch.Draw(TexOff, Position, Color.White);
            }
            else
            {
                SpriteBatch.Draw(TexOn, Position, Color.White);
            }
        }
    }
}
