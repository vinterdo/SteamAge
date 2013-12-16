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
    public class WorldHelper
    {
        /*
         * Vector2 ChunkV = new Vector2((int)Math.Floor((double)Vect.X / ChunkSize), (int)Math.Floor((double)Vect.Y / ChunkSize));

            if (this.Chunks.ContainsKey(ChunkV))
            {
                int PosX = (int)Math.Floor(Vect.X) - (int)ChunkV.X * ChunkSize;
                int PosY = (int)Math.Floor(Vect.Y) - (int)ChunkV.Y * ChunkSize;
                if (PosX < 0) PosX = 15 - PosX;
                if (PosY < 0) PosY = 15 - PosY;
         */

        public static Vector2 GetChunkPos(Vector2 PixelPos)
        {
            return new Vector2((int)Math.Floor((double)PixelPos.X / GameWorld.ChunkSize), (int)Math.Floor((double)PixelPos.Y / GameWorld.ChunkSize));
        }

        public static Vector2 GetBlockInChunkPos(Vector2 PixelPos)
        {
            Vector2 ChunkV = GetChunkPos(PixelPos);
            int PosX = (int)Math.Floor(PixelPos.X) - (int)ChunkV.X * GameWorld.ChunkSize;
            int PosY = (int)Math.Floor(PixelPos.Y) - (int)ChunkV.Y * GameWorld.ChunkSize;
            if (PosX < 0) PosX = 15 - PosX;
            if (PosY < 0) PosY = 15 - PosY;

            return new Vector2(PosX, PosY);
        }

        public static Vector2 GetPixelPos(Vector2 ChunkPos, Vector2 BlockPos)
        {
            throw new NotImplementedException();
        }

        public static Vector2 GetBlockPos(Vector2 PixelPos)
        {
            int PosX = (int)Math.Floor(PixelPos.X / 32);
            int PosY = (int)Math.Floor(PixelPos.Y /32);

            return new Vector2(PosX, PosY);
        }

         
    }
}
