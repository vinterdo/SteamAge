using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SteamAge.TileEntities
{
    interface IInventoryTE
    {
        TileEntityGUI GetGUI(); // return TileEntityGUI object that is used as TE GUI
        void DestroyGUI(); // Destroying GUI, used to drop items on ground
        void OpenGUI(); // Call TileEntityGUI.Open();
        void Close();
    }
}
