using System;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    [Serializable]
    public class ObjectInventoryGridUiInfo : InventoryGridUiInfo
    {
        public ObjectInventoryGridUiInfo()
        {
	        inventorySpecificType = InventoryType.Objects;
        }
    }
}