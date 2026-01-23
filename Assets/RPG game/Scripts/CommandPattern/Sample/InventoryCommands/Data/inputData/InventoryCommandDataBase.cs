using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.RPG.Items.InventorySystem.Samples;

namespace TGL.RPG.CommandPattern.Samples
{
    /// <summary>
    /// base class for inventory command data, any data related to inventory commands should inherit from this
    /// </summary>
    public abstract record InventoryCommandDataBase
    {
        public UniqueType UniqueType = UniqueType.Inventory;
        public InventoryType InventoryType { get; set; } // Mutable property
    }
}