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

namespace SteamAge.Items
{
    public class PatternSlot : GUIComponent
    {
        public Item PatternItem;
        public Rectangle Position; // can be null, beware!
        public bool Visible = false; // if (Visible) -> Positions is not null
        public GameWorld World;

        public PatternSlot(GameWorld World)
            : base()
        {
            this.World = World;
        }

        public override bool CheckActive()
        {
            if (Visible)
            {
                if (Position.Contains(Helper.VectorToPoint(GeneralManager.MousePos)))
                    return true;
            }
            return false;
        }

        public override void Draw(SpriteBatch SpriteBatch)
        {
            if (Visible)
            {
                if (CheckActive())
                {
                    SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/ItemSlot"], Position, Color.White);

                }
                else
                {
                    SpriteBatch.Draw(GeneralManager.Textures["Textures/GUI/ItemSlot"], Position, Color.Gray);
                }

                if (PatternItem != null)
                {
                    PatternItem.DrawIcon(SpriteBatch, Position);
                }
            }
        }

        public override bool HandleInput()
        {
            return false;
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}
