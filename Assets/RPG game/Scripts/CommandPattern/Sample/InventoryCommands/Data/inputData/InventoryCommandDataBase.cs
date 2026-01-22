using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;

namespace TGL.RPG.CommandPattern.Samples
{
    /// <summary>
    /// base class for inventory command data, any data related to inventory commands should inherit from this
    /// </summary>
    public abstract record InventoryCommandDataBase
    {
        public UniqueType InventoryType { get; set; } // Mutable property
    }
}