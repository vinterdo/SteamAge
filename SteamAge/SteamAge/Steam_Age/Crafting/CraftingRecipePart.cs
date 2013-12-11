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
    public class CraftingRecipePart
    {
        private List<ItemStack> Parts;

        public CraftingRecipePart()
        {
            Parts = new List<ItemStack>();
        }

        public bool Match(ItemStack IS)
        {
            foreach (ItemStack Stack in Parts)
            {
                if (Stack == null && IS == null)
                {
                    return true;
                }
                else if (IS != null)
                {
                    if (Stack.Item == IS.Item && Stack.Count <= IS.Count)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void AddPart(ItemStack IS)
        {
            if (!Parts.Contains(IS))
            {
                Parts.Add(IS);
            }
        }
    }
}
