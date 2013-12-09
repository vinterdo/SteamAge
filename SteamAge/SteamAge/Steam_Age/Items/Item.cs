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
    public class Item
    {
        public int Id;
        public string Tex;
        public static Dictionary<int, Item> ItemRegistry = new Dictionary<int,Item>();


        public static void RegisterItem(Item Item)
        {
            if (!(Item is Block))
            {
                try
                {
                    ItemRegistry.Add(Item.Id, Item);
                }
                catch (ArgumentException e)
                {
                    Logger.Write("Item ID already registered - ERROR");
                    Logger.Write(e.StackTrace);
                }
            }
            else
            {
                Logger.Write("BLock cant be registered by RegisterItem method, use RegisterBlock instead. Registration Skipped");
            }
        }

        public ItemStack GetStack(int Count)
        {
            return new ItemStack(this, Count);
        }

        public virtual void DrawIcon(SpriteBatch SpriteBatch, Rectangle Position)
        {
        }
    }
}
