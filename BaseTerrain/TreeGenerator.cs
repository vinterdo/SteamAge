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
using SteamAge;
using SteamAge.Generators;
using SteamAge.TileEntities;

namespace BaseTerrain
{
    public class TreeGenerator : Generator
    {
        public TreeGenerator(GameWorld World)
            : base(World)
        {
        }

        public override void Initalize()
        {
        }

        public override void Generate(Chunk C)
        {
            for (int y = 1; y < 15; y++)
            {
                for (int x = 1; x < 15; x++)
                {
                    if (C.Blocks[x - 1,y] == Block.GetBlock(0) && C.Blocks[x - 1,y] == Block.GetBlock(0) && C.Blocks[x - 1,y] == Block.GetBlock(0))
                    {
                        //World.SetBlock(new Vector2(x - 11 , y - 2) + C.Position * 32, new TreeTE(World, new Vector2(x - 11, y - 2) + C.Position * 32));
                        World.SetBlock(new Vector2(x , y) + C.Position * 32, new CraftingTableTE(new Vector2(x , y ) + C.Position * 32, World));
                    }
                }
            }
        }
    }
}
