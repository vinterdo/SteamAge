using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Bitmap FrontTop;
        Bitmap FrontBot;
        Bitmap FrontLeft;
        Bitmap FrontRight;
        Bitmap Back;
        string BlockName = "Wood/BlockWood";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Back = new Bitmap("BackImage.png");  
            FrontTop = new Bitmap("FrontImage.png");
            FrontBot = (Bitmap)FrontTop.Clone();
            FrontBot.RotateFlip(RotateFlipType.Rotate180FlipNone);
            FrontLeft = (Bitmap)FrontTop.Clone();
            FrontLeft.RotateFlip(RotateFlipType.Rotate270FlipNone);
            FrontRight = (Bitmap)FrontTop.Clone();
            FrontRight.RotateFlip(RotateFlipType.Rotate90FlipNone);

            for (int i = 0; i < 16; i++)
            {
                Bitmap DrawingBmp = (Bitmap)Back.Clone();
                Graphics G = Graphics.FromImage(DrawingBmp);
                G.Clear(Color.CornflowerBlue);
                G.DrawImage(Back, new Point(0, 0));

                switch (i)
                {
                    case 0:
                        //G.DrawImage(FrontTop, new Point(0, 0));
                        //G.DrawImage(FrontRight, new Point(0, 0));
                        //G.DrawImage(FrontBot, new Point(0, 0));
                        //G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 1:
                        //G.DrawImage(FrontTop, new Point(0, 0));
                        //G.DrawImage(FrontRight, new Point(0, 0));
                        G.DrawImage(FrontBot, new Point(0, 0));
                        //G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + "Bot.png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 2:
                        //G.DrawImage(FrontTop, new Point(0, 0));
                        //G.DrawImage(FrontRight, new Point(0, 0));
                        G.DrawImage(FrontBot, new Point(0, 0));
                        G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + "BotLeft.png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 3:
                        G.DrawImage(FrontTop, new Point(0, 0));
                        //G.DrawImage(FrontRight, new Point(0, 0));
                        G.DrawImage(FrontBot, new Point(0, 0));
                        G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + "BotLeftTop.png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 4:
                        G.DrawImage(FrontTop, new Point(0, 0));
                        //G.DrawImage(FrontRight, new Point(0, 0));
                        //G.DrawImage(FrontBot, new Point(0, 0));
                        //G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + "Top.png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 5:
                        G.DrawImage(FrontTop, new Point(0, 0));
                        G.DrawImage(FrontRight, new Point(0, 0));
                        G.DrawImage(FrontBot, new Point(0, 0));
                        G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + "Full.png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 6:
                        //G.DrawImage(FrontTop, new Point(0, 0));
                        //G.DrawImage(FrontRight, new Point(0, 0));
                        //G.DrawImage(FrontBot, new Point(0, 0));
                        G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + "Left.png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 7:
                        //G.DrawImage(FrontTop, new Point(0, 0));
                        G.DrawImage(FrontRight, new Point(0, 0));
                        //G.DrawImage(FrontBot, new Point(0, 0));
                        G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + "LeftRight.png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 8:
                        G.DrawImage(FrontTop, new Point(0, 0));
                        //G.DrawImage(FrontRight, new Point(0, 0));
                        //G.DrawImage(FrontBot, new Point(0, 0));
                        G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + "LeftTop.png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 9:
                        G.DrawImage(FrontTop, new Point(0, 0));
                        G.DrawImage(FrontRight, new Point(0, 0));
                        //G.DrawImage(FrontBot, new Point(0, 0));
                        G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + "LeftTopRight.png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 10:
                        //G.DrawImage(FrontTop, new Point(0, 0));
                        G.DrawImage(FrontRight, new Point(0, 0));
                        //G.DrawImage(FrontBot, new Point(0, 0));
                        //G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + "Right.png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 11:
                        //G.DrawImage(FrontTop, new Point(0, 0));
                        G.DrawImage(FrontRight, new Point(0, 0));
                        G.DrawImage(FrontBot, new Point(0, 0));
                        //G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + "RightBot.png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 12:
                        //G.DrawImage(FrontTop, new Point(0, 0));
                        G.DrawImage(FrontRight, new Point(0, 0));
                        G.DrawImage(FrontBot, new Point(0, 0));
                        G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + "RightBotLeft.png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 13:
                        G.DrawImage(FrontTop, new Point(0, 0));
                        G.DrawImage(FrontRight, new Point(0, 0));
                        G.DrawImage(FrontBot, new Point(0, 0));
                        //G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + "TopRightBot.png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 14:
                        G.DrawImage(FrontTop, new Point(0, 0));
                        //G.DrawImage(FrontRight, new Point(0, 0));
                        G.DrawImage(FrontBot, new Point(0, 0));
                        //G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + "TopBot.png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 15:
                        G.DrawImage(FrontTop, new Point(0, 0));
                        G.DrawImage(FrontRight, new Point(0, 0));
                        //G.DrawImage(FrontBot, new Point(0, 0));
                        //G.DrawImage(FrontLeft, new Point(0, 0));
                        DrawingBmp.Save(BlockName + "TopRight.png", System.Drawing.Imaging.ImageFormat.Png);
                        break;
                }


            }


        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
