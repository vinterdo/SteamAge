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
    public abstract class GUIComponent
    {

        public bool IsActive = false;

        public abstract bool HandleInput();

        public abstract void Draw(SpriteBatch SpriteBatch, GameTime GameTime);

        public abstract void Update(GameTime gameTime);

        public abstract bool CheckActive();
    }
}
