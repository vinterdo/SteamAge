using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VAPI;


namespace SteamAge
{
    public abstract class BaseMod
    {
        string ModName;
        string ModVersion;
        public abstract void Register();
        public abstract void Initalize();
        public abstract void Update();
        public abstract void Unload();

    }
}
