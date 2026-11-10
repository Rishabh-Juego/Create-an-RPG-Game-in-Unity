using System;
using TGL.RPG.Items.InventorySystem.Samples;

namespace RPG_Game.Items.InventorySystem.Samples
{
    [Serializable]
    public class MagicInventoryGridData : InventoryGridData
    {
        public MagicInventoryGridData()
        {
            inventoryType = InventoryType.Magic;
        }
    }
}