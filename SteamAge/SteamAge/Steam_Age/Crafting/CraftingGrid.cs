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
        public Vector2 Position;
        public CraftingRecipe MatchingRecipe
        {
            get
            {
                return _MatchingRecipe;
            }
        }

        public ItemSlot Output
        {
            get
            {
                return _MatchingRecipe.Output;
            }
        }

        public CraftingGrid(Vector2 Position)
        {
            this.Position = Position;

            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    Vector2 UpperLeftPos = Position + new Vector2(x * 32, y * 32);
                    RecipeSlots[x, y].Position = new Rectangle((int)UpperLeftPos.X, (int)UpperLeftPos.Y, 32, 32);
                }
            }
        }

        public void UpdateGrid()
        {

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
    }
}
