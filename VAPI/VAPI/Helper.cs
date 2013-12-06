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

namespace VAPI
{
    public class Helper
    {
        private static Random Random = new Random();

        public static Vector2 GetTopLeftFromRect(Rectangle Rect)
        {
            return new Vector2(Rect.X, Rect.Y);
        }

        public static Vector2 GetBottomRightFromRect(Rectangle Rect)
        {
            return new Vector2(Rect.X + Rect.Width, Rect.Y + Rect.Height);
        }


        public static Vector2 GetBottomLeftFromRect(Rectangle Rect)
        {
            return new Vector2(Rect.X , Rect.Y + Rect.Height);
        }


        public static Vector2 GetTopRightFromRect(Rectangle Rect)
        {
            return new Vector2(Rect.X + Rect.Width, Rect.Y );
        }

        public static bool CheckLMBClick(Rectangle Rect)
        {
            return CheckIfInside(Rect, GeneralManager.MousePos) && GeneralManager.IsLMBClickedEdge();
        }

        public static bool CheckIfInside(Rectangle Rect, Vector2 Vec)
        {
            return Rect.X < Vec.X && Rect.Y < Vec.Y && Rect.X + Rect.Width > Vec.X && Rect.Y + Rect.Height > Vec.Y;
        }


        public static Vector2 GetVectorFromPoint(Point Point)
        {
            return new Vector2(Point.X, Point.Y);
        }

        public static int GetRandom()
        {
            return Random.Next();
        }

        public static float GetRandomTo(float Target)
        {
            if (Target != 0)
            {
                try
                {
                    return ((float)((int)Helper.GetRandom() % (int)(Target * 10000f)) / 10000f);
                }
                catch (DivideByZeroException)
                {
                    return 0;
                }
            }
            else return 0;
        }

        public static Vector2 GetRandomTo(Vector2 Target)
        {
            return new Vector2((Helper.GetRandom() % (Target.X * 10000f)) / 10000f, (Helper.GetRandom() % (Target.Y * 10000f)) / 10000f);
        }



        public static Vector2 GetVectorFromAngle(float Angle)
        {
            return new Vector2((float)Math.Sin(Angle), (float)Math.Cos(Angle));
        }

        public static float GetAngleFromVector(Vector2 Vec)
        {
            return (float)Math.Atan2(Vec.X, Vec.Y);
        }

        public static Vector2 RotateVector(float Angle, Vector2 Vec, Vector2 Center)
        {
            return new Vector2((float)(Math.Cos(Angle) * (Vec.X - Center.X) - Math.Sin(Angle) * (Vec.Y - Center.Y)), (float)(Math.Sin(Angle) * (Vec.X - Center.X) + Math.Cos(Angle) * (Vec.Y - Center.Y))) + Center;
        }


        public static bool CheckCollision(Vector2 Vec, Rectangle Rect)
        {
            return Vec.X > Rect.X && Vec.Y > Rect.Y && Vec.X < Rect.X + Rect.Width && Vec.Y < Rect.Y + Rect.Height;
        }

        public static bool CheckCollision(Vector2 Vec, Rectangle Rect, float Angle, Vector2 Center)
        {
            return CheckCollision(RotateVector(Angle * -1, Vec, Center), Rect);
        }

        public static Vector2 RotateVector(Vector2 Base, float Angle, Vector2 Center)
        {
            return Vector2.Transform(Base - Center, Matrix.CreateRotationZ(Angle)) + Center;
        }

        public static Vector2 GetCenter(Texture2D Tex)
        {
            return new Vector2(Tex.Width, Tex.Height);
        }

        public static float[,] BlurTable(float[,] Tab)
        {
            float[,] Out = new float[Tab.GetLength(0), Tab.GetLength(1)];
            for (int y = 0; y < Tab.GetLength(1); y++)
            {
                for (int x = 0; x < Tab.GetLength(0); x++)
                {
                    Out[x, y] += Tab[x, y];
                    Out[x, y] += Tab[(int)MathHelper.Clamp(x -1, 0, Tab.GetLength(0) -1), y];
                    Out[x, y] += Tab[(int)MathHelper.Clamp(x + 1, 0, Tab.GetLength(0) -1), y];
                    Out[x, y] += Tab[x, (int)MathHelper.Clamp(y - 1, 0, Tab.GetLength(1) -1)];
                    Out[x, y] += Tab[x, (int)MathHelper.Clamp(y + 1, 0, Tab.GetLength(1) -1)];

                    Out[x, y] /= 5;
                }
            }


            return Out;
        }

        public static Point VectorToPoint(Vector2 Vec)
        {
            return new Point((int)Vec.X, (int)Vec.Y);
        }

        public static Color AddColors(Color Color1, Color Color2)
        {
            return new Color(Color1.R + Color2.R, Color1.G + Color2.G, Color1.B + Color2.B);
        }
    }
}
