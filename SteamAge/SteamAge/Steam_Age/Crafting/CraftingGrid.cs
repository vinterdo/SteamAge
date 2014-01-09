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
    public class CraftingGrid
    {
        private ItemSlot[,] RecipeSlots = new ItemSlot[5, 5];
        private CraftingRecipe _MatchingRecipe;
        private int[,] RecipePartIdMatching;
        public Vector2 Position;
        public CraftingRecipe MatchingRecipe
        {
            get
            {
                return _MatchingRecipe;
            }
        }

        public ItemStack Output
        {
            get
            {
                return _MatchingRecipe.Output;
            }
        }

        public ItemSlot OutputSlot;

        public CraftingGrid(GameWorld GameWorld, Vector2 Position)
        {
            this.Position = Position;
            RecipePartIdMatching = new int[5, 5];

            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    RecipeSlots[x, y] = new ItemSlot(GameWorld);
                    RecipeSlots[x, y].Visible = true;
                    Vector2 UpperLeftPos = Position + new Vector2(x * 34, y * 34);
                    RecipeSlots[x, y].Position = new Rectangle((int)UpperLeftPos.X, (int)UpperLeftPos.Y, 32, 32);
                    RecipeSlots[x, y].OnStackModified += UpdateGrid;
                    RecipePartIdMatching[x, y] = -1;
                }
            }

            OutputSlot = new ItemSlot(GameWorld);
            OutputSlot.Visible = true;
            OutputSlot.State = ItemSlot.SlotState.InputLocked;
            OutputSlot.Position = new Rectangle((int)Position.X + 5 * 32 + 10, (int)Position.Y + 2 * 32, 32, 32);

            OutputSlot.OnStackModified += ItemCrafted;
        }

        public void ItemCrafted()
        {
            if (_MatchingRecipe != null)
            {
                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 5; x++)
                    {
#warning FIX!
                        if (MatchingRecipe.RecipeParts[x, y] != null && !MatchingRecipe.RecipeParts[x, y].IsEmpty())
                        {
                            RecipeSlots[x, y].ItemStack.Count -= 1;

                            if (RecipeSlots[x, y].ItemStack.Count == 0)
                            {
                                RecipeSlots[x, y].ItemStack = null;
                            }
                        }
                    }
                }

                UpdateGrid();
            }
        }

        public void UpdateGrid()
        {
            bool Found = false;
            foreach (CraftingRecipe CR in CraftingRecipe.Recipes)
            {
                if (CraftingRecipe.RecipesMatch(this, CR))
                {
                    _MatchingRecipe = CR;
                    Found = true;
                    OutputSlot.ItemStack = new ItemStack(CR.Output.Item, CR.Output.Count);
                    
                }
                
            }

            if (!Found)
            {
                _MatchingRecipe = null;
                OutputSlot.ItemStack = null;
            }
        }

        public void AddToInventory(TileEntities.TileEntityGUI TEGUI)
        {
            foreach (ItemSlot IS in this.RecipeSlots)
            {
                TEGUI.AddSlot(IS);
            }

            TEGUI.AddSlot(OutputSlot);
        }

        public void Draw(SpriteBatch SpriteBatch)
        {
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    RecipeSlots[x, y].Draw(SpriteBatch);
                }
            }
        }

        public ItemStack GetStack(int x, int y)
        {
            return RecipeSlots[x, y].ItemStack;
        }
    }
}
