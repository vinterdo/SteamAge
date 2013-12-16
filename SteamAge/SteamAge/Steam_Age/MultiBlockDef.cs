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
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using System.Diagnostics;
using VAPI.FluidSim;
using Krypton;
using Krypton.Lights;

namespace SteamAge
{
    public class MultiBlockDef
    {
        Vector2 Size;
        Vector2 Center;
        public Block[,] BlockTable;

        public MultiBlockDef(Vector2 Size, Vector2 Center)
        {
            this.Size = Size;
            this.Center = Center;

            BlockTable = new Block[(int)Size.X, (int)Size.Y];
        }

        public void SetBlock(int x, int y, Block B)
        {
            try
            {
                BlockTable[x, y] = B;
            }
            catch
            {
                Logger.Write("Invalid usage of SetBlock function in MultiBLockDefinition");
            }
        }
    }
}
