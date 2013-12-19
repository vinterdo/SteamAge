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

namespace SteamAge.TileEntities
{
    interface IMultiBlockTE
    {
        Vector2 GetCenterCoord();
        MultiBlockDef GetMultiBlockDef();
        bool CanBePlaced(Vector2 BlockPos);
    }
}
