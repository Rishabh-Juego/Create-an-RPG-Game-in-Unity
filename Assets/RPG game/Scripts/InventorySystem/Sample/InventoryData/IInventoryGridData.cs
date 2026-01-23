using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    public interface IInventoryGridData<SO_T, U> where U: IInventorySlotData<SO_T> where SO_T : So_InventoryData
    {
        UniqueType UniqueType => UniqueType.Inventory;
        InventoryType InventoryType { get; }
        event Action OnInventoryDataChanged;
        void Initialize(InventoryType inventoryType, int slotSize, SO_T defaultOrNullItem);
        
    }
}