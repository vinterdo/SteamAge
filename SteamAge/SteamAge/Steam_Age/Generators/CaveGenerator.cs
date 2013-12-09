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
using SteamAge;

namespace SteamAge.Generators
{
    public class CaveGenerator : Generator
    {
        public CaveGenerator(GameWorld World)
            : base(World)
        {
        }


        public override void Initalize()
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


            if (C.Position.Y >= -1)
            {
                GenerateCaves(C, Perlin);
            }
        }


        private void GenerateCaves(Chunk C, float[,] Perlin)
        {
            for (int y = -2; y < 18; y++)
            {
                for (int x = -2; x < 18; x++)
                {
                    if (Perlin[x + 2, y + 2] > 0.32f)
                    {
                        int Radius;
                        if (Perlin[x + 2, y + 2] < 0.37)
                        {
                            Radius = 3;
                        }
                        else
                        {
                            Radius = 4;
                        }

                        for (int y1 = 0; y1 < 16; y1++)
                        {
                            for (int x1 = 0; x1 < 16; x1++)
                            {
                                if (Math.Pow(x1 - x, 2) + Math.Pow(y1 - y, 2) <= Math.Pow(Radius, 2))
                                {
                                    C.Blocks[(int)MathHelper.Clamp(x1, 0, 15), (int)MathHelper.Clamp(y1, 0, 15)] = Block.GetBlock(0);
                                }
                            }
                        }
                    }
                }
            }

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    if (C.Blocks[(int)MathHelper.Clamp(x - 1, 0, 15), (int)MathHelper.Clamp(y, 0, 15)].Id == 0 &&
                       C.Blocks[(int)MathHelper.Clamp(x + 1, 0, 15), (int)MathHelper.Clamp(y, 0, 15)].Id == 0)
                    {
                        C.Blocks[(int)MathHelper.Clamp(x, 0, 15), (int)MathHelper.Clamp(y, 0, 15)] = Block.GetBlock(0);
                    }


                }
            }

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    if (C.Blocks[(int)MathHelper.Clamp(x - 1, 0, 15), (int)MathHelper.Clamp(y, 0, 15)].Id == 1 &&
                       C.Blocks[(int)MathHelper.Clamp(x + 1, 0, 15), (int)MathHelper.Clamp(y, 0, 15)].Id == 1 &&
                        C.Blocks[(int)MathHelper.Clamp(x - 1, 0, 15), (int)MathHelper.Clamp(y - 1, 0, 15)].Id == 1 &&
                       C.Blocks[(int)MathHelper.Clamp(x + 1, 0, 15), (int)MathHelper.Clamp(y + 1, 0, 15)].Id == 1)
                    {
                        C.Blocks[(int)MathHelper.Clamp(x, 0, 15), (int)MathHelper.Clamp(y, 0, 15)] = Block.GetBlock(2);
                    }


                }
            }
        }
    }
}
