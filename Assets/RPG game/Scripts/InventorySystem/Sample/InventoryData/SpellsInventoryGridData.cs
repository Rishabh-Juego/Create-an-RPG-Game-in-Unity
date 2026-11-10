using System;
using TGL.RPG.Items.InventorySystem.Samples;

namespace RPG_Game.Items.InventorySystem.Samples
{
    [Serializable]
    public class SpellsInventoryGridData : InventoryGridData
    {
        public SpellsInventoryGridData()
        {
            inventoryType = InventoryType.Spells;
        }
    }
}