using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VAPI;

namespace SteamAge
{
    public class DropDefinition
    {
        List<KeyValuePair<KeyValuePair<float, float>, Item>> Drop; // Probability ( min, max - min) / Item

        public DropDefinition()
        {
            Drop = new List<KeyValuePair<KeyValuePair<float, float>, Item>>();
        }

        public List<ItemStack> GetDrop()
        {
            List<ItemStack> DropItems = new List<ItemStack>();


            foreach (KeyValuePair<KeyValuePair<float, float>, Item> D in Drop)
            {
                DropItems.Add(new ItemStack(D.Value, (int)D.Key.Key + (int)Helper.GetRandomTo(D.Key.Value)));
            }

            return DropItems;
        }

        public void AddDrop(float Min, float Addition, Item Item)
        {
            Drop.Add(new KeyValuePair<KeyValuePair<float,float>,Item>(new KeyValuePair<float, float>(Min, Addition), Item));
        }

        public void PollDrop(Player Player)
        {
            foreach (KeyValuePair<KeyValuePair<float, float>, Item> D in Drop)
            {
                if (D.Key.Value > 0)
                {
                    Player.AddToInv(new ItemStack(D.Value, (int)D.Key.Key + (int)Helper.GetRandom() % (int)D.Key.Value));
                }
                else
                {
                    Player.AddToInv(new ItemStack(D.Value, (int)D.Key.Key));
                }
            }
        }
    }
}
