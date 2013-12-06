using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VAPI;
using SteamAge;

namespace SteamAge
{
    public abstract class Generator
    {
        protected GameWorld World;

        public Generator(GameWorld World)
        {
            this.World = World;
        }
        public abstract void Generate(Chunk C);
        
        public abstract void Initalize();
    }
}
