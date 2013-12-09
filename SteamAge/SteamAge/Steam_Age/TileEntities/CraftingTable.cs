using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using VAPI;
using SteamAge.Crafting;

namespace SteamAge.TileEntities
{
    public class CraftingTableTE : TileEntity, IInventoryTE
    {
        public TileEntityGUI Inventory;
        public CraftingGrid CraftingGrid;

        public CraftingTableTE(Vector2 Position, GameWorld GameWorld)
            : base(GameWorld, "CraftingTable")
        {
            this.Entity = new CraftingTableEntity(GameWorld);
            this.TileBlock = Block.GetBlock(1001);
            this.Name = "Textures/TileEntities/CraftingTable";

            Inventory = new TileEntityGUI(new Rectangle(100, 100, 500, 500));
            GameWorld.ParentScreen.AddGUI(Inventory);
            CraftingGrid = new CraftingGrid(GameWorld, new Vector2(100, 100));
            CraftingGrid.AddToInventory(Inventory);
        }

        public void Close()
        {
            Inventory.Close();
        }

        public void OpenGUI()
        {
            Inventory.Open();
        }

        public TileEntityGUI GetGUI()
        {
            return Inventory;
        }

        public void DestroyGUI()
        {
            throw new NotImplementedException();
        }

        public override void Kill()
        {
            //CraftingGrid.Destroy(); // TODO: Add grid destroying
            // REMOVING INVENTORY WINDOW!!!!!!!!!!!!
            this = null;
            // ======== up: reminder ==============
            base.Kill();
        }
    }

    public class CraftingTableBlock : Block, IEntityBlock
    {
        public CraftingTableBlock()
            : base()
        {
            IsSolid = false;
            Family = "CraftingTable";
            Tex = "Textures/TileEntities/CraftingTable";
            Id = 1001;

            Textures[0] = new KeyValuePair<Block.BlockState, string>((Block.BlockState)0, "Textures/TileEntities/CraftingTable");


            Drop.AddDrop(1, 0, this);
        }
        public TileEntity GetNewTE(GameWorld GameWorld, Vector2 Position)
        {
            return new CraftingTableTE(Position, GameWorld);
        }
    }

    public class CraftingTableEntity : Entity
    {

        public CraftingTableEntity(GameWorld GameWorld)
            : base(GameWorld)
        {

        }
    }
}
