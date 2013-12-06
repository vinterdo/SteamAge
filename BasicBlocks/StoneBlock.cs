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
using VAPI;

namespace BasicBlocks
{
    public class StoneBlock : Block
    {
        
        public StoneBlock()
            : base()
        {
            
            this.Id = 2;
            this.Name = "Granite";
            this.IsSolid = true;
            this.Family = "Stone";
            Tex = "Textures/Blocks/" + Name + "/Block" + Name + "Full";

            this.Textures[15] = new KeyValuePair<BlockState, string>((BlockState)15, "Textures/Blocks/" + Name + "/Block" + Name + "Full");
            this.Textures[0] = new KeyValuePair<BlockState, string>((BlockState)0, "Textures/Blocks/" + Name + "/Block" + Name);

            this.Textures[1] = new KeyValuePair<BlockState, string>((BlockState)1, "Textures/Blocks/" + Name + "/Block" + Name + "Top");
            this.Textures[2] = new KeyValuePair<BlockState, string>((BlockState)2, "Textures/Blocks/" + Name + "/Block" + Name + "Right");
            this.Textures[4] = new KeyValuePair<BlockState, string>((BlockState)4, "Textures/Blocks/" + Name + "/Block" + Name + "Bot");
            this.Textures[8] = new KeyValuePair<BlockState, string>((BlockState)8, "Textures/Blocks/" + Name + "/Block" + Name + "Left");

            this.Textures[3] = new KeyValuePair<BlockState, string>((BlockState)3, "Textures/Blocks/" + Name + "/Block" + Name + "TopRight");
            this.Textures[6] = new KeyValuePair<BlockState, string>((BlockState)6, "Textures/Blocks/" + Name + "/Block" + Name + "RightBot");
            this.Textures[12] = new KeyValuePair<BlockState, string>((BlockState)12, "Textures/Blocks/" + Name + "/Block" + Name + "BotLeft");
            this.Textures[9] = new KeyValuePair<BlockState, string>((BlockState)13, "Textures/Blocks/" + Name + "/Block" + Name + "LeftTop");

            this.Textures[14] = new KeyValuePair<BlockState, string>((BlockState)14, "Textures/Blocks/" + Name + "/Block" + Name + "RightBotLeft");
            this.Textures[13] = new KeyValuePair<BlockState, string>((BlockState)13, "Textures/Blocks/" + Name + "/Block" + Name + "BotLeftTop");
            this.Textures[11] = new KeyValuePair<BlockState, string>((BlockState)11, "Textures/Blocks/" + Name + "/Block" + Name + "LeftTopRight");
            this.Textures[7] = new KeyValuePair<BlockState, string>((BlockState)7, "Textures/Blocks/" + Name + "/Block" + Name + "TopRightBot");

            this.Textures[10] = new KeyValuePair<BlockState, string>((BlockState)10, "Textures/Blocks/" + Name + "/Block" + Name + "LeftRight");
            this.Textures[5] = new KeyValuePair<BlockState, string>((BlockState)5, "Textures/Blocks/" + Name + "/Block" + Name + "TopBot");


            ShapeVertices.Add(new Vector2(0, 0));
            ShapeVertices.Add(new Vector2(GameWorld.TileWidth, 0));
            ShapeVertices.Add(new Vector2(GameWorld.TileWidth, GameWorld.TileHeight));
            ShapeVertices.Add(new Vector2(0, GameWorld.TileHeight));

            this.Drop.AddDrop(1, 0, this);

        }

    }
}
