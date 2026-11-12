using System;
using TGL.RPG.Items.InventorySystem.Samples;

namespace RPG_Game.Items.InventorySystem.Samples
{
    [Serializable]
    public class MagicInventoryGridUiInfo : InventoryGridUiInfo
    {
        public MagicInventoryGridUiInfo()
        {
            inventoryType = InventoryType.Magic;
        }
    }
}