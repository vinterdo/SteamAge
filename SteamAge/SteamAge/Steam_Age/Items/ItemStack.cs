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
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using VAPI;
namespace SteamAge
{
    public class ItemStack
    {
        public static int MAX_STACK = 64;
        public ItemStack(Item Item, int Count)
        {
            this.Item = Item;
            this.Count = Count;
        }
        public Item Item;
        public int Count;

        public void DrawStack(SpriteBatch SpriteBatch, Rectangle Position)
        {
            Item.DrawIcon(SpriteBatch, Position);
            GeneralManager.Fonts["Fonts/SteamWreck"].DrawText(SpriteBatch, new Rectangle(Position.X + Position.Width / 2, Position.Y + Position.Height / 2,Position.Width/2,Position.Height/2), Count.ToString(), Color.White);
        }
    }
}
