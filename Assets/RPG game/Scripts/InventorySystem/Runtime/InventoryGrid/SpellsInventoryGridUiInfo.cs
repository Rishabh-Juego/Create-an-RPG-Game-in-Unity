using System;
using TGL.RPG.Items.InventorySystem.Samples;

namespace RPG_Game.Items.InventorySystem.Samples
{
    [Serializable]
    public class SpellsInventoryGridUiInfo : InventoryGridUiInfo
    {
        public SpellsInventoryGridUiInfo()
        {
            inventoryType = InventoryType.Spells;
        }
    }
}