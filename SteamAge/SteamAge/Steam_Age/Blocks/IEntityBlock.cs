using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VAPI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SteamAge
{
    public interface IEntityBlock // Every block related to Tile Entuty have to implements this inteface
    {
        TileEntity GetNewTE(GameWorld World, Vector2 Position); // return new TileEntity related to block
    }
}
