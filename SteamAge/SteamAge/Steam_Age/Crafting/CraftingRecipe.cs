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
        public ItemStack Output;

        private static List<CraftingRecipe> CraftingRegistry = new List<CraftingRecipe>();

        public CraftingRecipe(GameWorld World)
        {
            Output = new ItemStack(null, 0);
        }

        public static List<CraftingRecipe> Recipes
        {
            get
            {
                return CraftingRegistry;
            }
        }

        public static void RegisterRecipe(CraftingRecipe Recipe)
        {
            CraftingRegistry.Add(Recipe);
            Logger.Write("Recipe for item id " + Recipe.Output.Item.Id + " added to CraftingRegistry");
        }

        public static void RemoveRecipe(CraftingRecipe Recipe)
        {
            CraftingRegistry.Remove(Recipe);
        }

        public static bool RecipesMatch(CraftingGrid CG, CraftingRecipe CR)
        {
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if (CR.RecipeParts[x, y] == null)
                    {
                        if (CG.GetStack(x, y) == null)
                        {
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if(!CR.RecipeParts[x, y].Match(CG.GetStack(x, y)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
