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

namespace BaseTerrain
{
    public class ModMain : BaseMod
    {
        public override void Initalize(GameWorld World)
        {
            World.Generator.RegisterGenerator(new CaveGenerator(World));
            World.Generator.RegisterGenerator(new TreeGenerator(World));
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
