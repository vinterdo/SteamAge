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

namespace SteamAge.Crafting
{
    public class CraftingRecipe
    {
        public CraftingRecipePart[,] RecipeParts = new CraftingRecipePart[5, 5];
        public ItemSlot Output;

        private static List<CraftingRecipe> CraftingRegistry = new List<CraftingRecipe>();

        public static void RegisterRecipe(CraftingRecipe Recipe)
        {

        }

        public static void RemoveRecipe(CraftingRecipe Recipe)
        {
            CraftingRegistry.Remove(Recipe);
        }
    }
}
