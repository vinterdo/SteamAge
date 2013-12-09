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


namespace SteamAge.Generators
{
    class BasicTerrainGenerator : Generator
    {
        public BasicTerrainGenerator(GameWorld World)
            : base(World)
        {
        }

        public override void Generate(Chunk C)
        {
            //Perlin Gen
            float[,] Perlin = new float[20, 20];
            PerlinNoise PNoise = new PerlinNoise(16);
            for (int y = 0; y < 20; y++)
            {
                for (int x = 0; x < 20; x++)
                {
                    Perlin[x, y] = (float)PNoise.Noise((double)(x + C.Position.X * 16 - 2) / 10, (double)(y + C.Position.Y * 16 - 2) / 10, 0);
                }
            }

            if (C.Position.Y <= -2)
            {
                for (int y = 0; y < 16; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        C.Blocks[(int)MathHelper.Clamp(x, 0, 15), (int)MathHelper.Clamp(y, 0, 15)] = Block.GetBlock(0);

                    }
                }
            }
            if (C.Position.Y < 0 && C.Position.Y > -2)
            {
                GenerateSurface(C, Perlin);
                GenerateDirtSurface(C, Perlin);
                GenerateGrassSurface(C, Perlin);
            }

            // Background

            if (C.Position.Y == -1)
            {
                C.BackgroundTex = GeneralManager.Textures["Textures/Backgrounds/BackgroundForest"];
            }
            if (C.Position.Y > -1)
            {
                C.BackgroundTex = GeneralManager.Textures["Textures/Backgrounds/BackgroundDirt"];
            }

        }

        

        private static void GenerateSurface(Chunk C, float[,] Perlin)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {

                    if (((Perlin[x, 0] + Perlin[(int)MathHelper.Clamp(x - 1, 0, 15), 0] + Perlin[(int)MathHelper.Clamp(x + 1, 0, 15), 0]) / 3) * -32 > C.Position.Y * 16 + y +12)
                    {
                        C.Blocks[x, y] = Block.GetBlock(0);
                    }
                    else
                    {
                        C.Blocks[(int)MathHelper.Clamp(x, 0, 15), (int)MathHelper.Clamp(y, 0, 15)] = Block.GetBlock(2);
                    }

                }
            }

        }

        private static void GenerateDirtSurface(Chunk C, float[,] Perlin)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {

                    if ((((Perlin[x, 0] + Perlin[(int)MathHelper.Clamp(x - 1, 0, 15), 0] + Perlin[(int)MathHelper.Clamp(x + 1, 0, 15), 0]) / 3) * -32 > C.Position.Y * 16 + y + 8) && C.Blocks[x, y].IsSolid)
                    {
                        C.Blocks[(int)MathHelper.Clamp(x, 0, 15), (int)MathHelper.Clamp(y, 0, 15)] = Block.GetBlock(1);
                    }

                }
            }

        }

        private static void GenerateGrassSurface(Chunk C, float[,] Perlin)
        {
            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {

                    if ((((Perlin[x, 0] + Perlin[(int)MathHelper.Clamp(x - 1, 0, 15), 0] + Perlin[(int)MathHelper.Clamp(x + 1, 0, 15), 0]) / 3) * -32 > C.Position.Y * 16 + y + 11) && C.Blocks[x, y].IsSolid)
                    {
                        C.Blocks[(int)MathHelper.Clamp(x, 0, 15), (int)MathHelper.Clamp(y, 0, 15)] = Block.GetBlock(3);
                    }

                }
            }

        }

        public override void Initalize()
        {
            
        }
    }
}
