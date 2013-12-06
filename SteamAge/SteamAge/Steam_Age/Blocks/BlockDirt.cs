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
    class BlockDirt:Block
    {
        


        public BlockDirt()
            : base()
        {
            this.Id = 1;
            this.Name = "Textures/Blocks/BlockDirt";
            this.IsSolid = true;
            this.Family = "Dirt";
            Tex = "Textures/Blocks/Dirt/BlockDirtFull";


            this.Textures[15] = new KeyValuePair<BlockState, string>((BlockState)15, "Textures/Blocks/Dirt/BlockDirtFull");
            this.Textures[0] = new KeyValuePair<BlockState, string>((BlockState)0, "Textures/Blocks/Dirt/BlockDirt");

            this.Textures[1] = new KeyValuePair<BlockState, string>((BlockState)1, "Textures/Blocks/Dirt/BlockDirtTop");
            this.Textures[2] = new KeyValuePair<BlockState, string>((BlockState)2, "Textures/Blocks/Dirt/BlockDirtRight");
            this.Textures[4] = new KeyValuePair<BlockState, string>((BlockState)4, "Textures/Blocks/Dirt/BlockDirtBot");
            this.Textures[8] = new KeyValuePair<BlockState, string>((BlockState)8, "Textures/Blocks/Dirt/BlockDirtLeft");

            this.Textures[3] = new KeyValuePair<BlockState, string>((BlockState)3, "Textures/Blocks/Dirt/BlockDirtTopRight");
            this.Textures[6] = new KeyValuePair<BlockState, string>((BlockState)6, "Textures/Blocks/Dirt/BlockDirtRightBot");
            this.Textures[12] = new KeyValuePair<BlockState, string>((BlockState)12, "Textures/Blocks/Dirt/BlockDirtBotLeft");
            this.Textures[9] = new KeyValuePair<BlockState, string>((BlockState)13, "Textures/Blocks/Dirt/BlockDirtLeftTop");

            this.Textures[14] = new KeyValuePair<BlockState, string>((BlockState)14, "Textures/Blocks/Dirt/BlockDirtRightBotLeft");
            this.Textures[13] = new KeyValuePair<BlockState, string>((BlockState)13, "Textures/Blocks/Dirt/BlockDirtBotLeftTop");
            this.Textures[11] = new KeyValuePair<BlockState, string>((BlockState)11, "Textures/Blocks/Dirt/BlockDirtLeftTopRight");
            this.Textures[7] = new KeyValuePair<BlockState, string>((BlockState)7, "Textures/Blocks/Dirt/BlockDirtTopRightBot");

            this.Textures[10] = new KeyValuePair<BlockState, string>((BlockState)10, "Textures/Blocks/Dirt/BlockDirtLeftRight");
            this.Textures[5] = new KeyValuePair<BlockState, string>((BlockState)5, "Textures/Blocks/Dirt/BlockDirtTopBot");


            ShapeVertices.Add(new Vector2(0, 0));
            ShapeVertices.Add(new Vector2(GameWorld.TileWidth, 0));
            ShapeVertices.Add(new Vector2(GameWorld.TileWidth, GameWorld.TileHeight));
            ShapeVertices.Add(new Vector2(0, GameWorld.TileHeight));

            Drop.AddDrop(1, 0, this);
        }
    }
}
