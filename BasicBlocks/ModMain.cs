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

namespace BasicBlocks
{
    public class ModMain : BaseMod
    {
        public override void Initalize()
        {
            StoneBlock StoneBlock = new StoneBlock();
            GrassBlock GrassBlock = new GrassBlock();
            Block.RegisterBlock(StoneBlock);
            Block.RegisterBlock(GrassBlock);
        }

        public override void Register()
        {
            
        }

        public override void Update()
        {
            
        }

        public override void Unload()
        {
            
        }
    }
}
