using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;

namespace TGL.RPG.CommandPattern.Samples
{
    /// <summary>
    /// example data object
    /// </summary>
    public record AddItemToInventoryDto : InventoryCommandDataBase
    {
        public So_InventoryData inventoryData { get; set; } // the object that is being added
        public int ItemCount { get; set; }
    }
}