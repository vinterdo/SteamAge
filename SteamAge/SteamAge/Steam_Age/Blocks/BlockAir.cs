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

namespace SteamAge.Blocks
{
    class BlockAir : Block
    {
        public BlockAir()
            : base()
        {
            this.Id = 0;
            this.Textures[0] = new KeyValuePair<BlockState, string>(BlockState.NoEdge, "Textures/Blocks/BlockAir");
            Tex = "Textures/Blocks/BlockAir";
            this.IsSolid = false;
            this.Family = "Air";
            
        }

        public override void Draw(SpriteBatch SpriteBatch, Vector2 Position, Color Color, int State)
        {
            base.Draw(SpriteBatch, Position, Color, 0);
        }

        
    }
}
