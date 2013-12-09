using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VAPI;
using SteamAge.Crafting;
using SteamAge.Items;

namespace SteamAge.TileEntities
{
    public class TileEntityGUI : Window
    {
        private List<ItemSlot> _ItemSlots;
        public TileEntityGUI(Rectangle Position)
            : base(Position, new Color(0.5f, 0.5f, 0.5f, 0.5f))
        {
        }

        public virtual void Open()
        {
        }

        public virtual void Close()
        {
        }

        public virtual void Destroy()
        {
        }

        public virtual void AddSlot(ItemSlot Slot)
        {
            _ItemSlots.Add(Slot);
            this.GUIComponents.Push(Slot);
        }

        public List<ItemSlot> ItemSlots
        {
            get
            {
                return _ItemSlots;
            }
        }

        public void DestroySlot(ItemSlot ItemSlot)
        {
            _ItemSlots.Remove(ItemSlot);
        }
    }
}
