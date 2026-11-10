using System;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    [Serializable]
    public class ItemInventoryGridData : InventoryGridData
    {
        public ItemInventoryGridData()
        {
            inventoryType = InventoryType.Items;
        }
    }
}