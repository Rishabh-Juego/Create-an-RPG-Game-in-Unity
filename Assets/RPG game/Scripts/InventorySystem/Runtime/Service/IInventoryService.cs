using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem
{
    public interface IInventoryService<SO_T, U> where SO_T : So_InventoryData where U : IInventorySlotData<SO_T>
    {
        UniqueType InventoryType { get; }
        event Action OnInventoryChanged;
        void Initialize(int slotSize, SO_T defaultOrNullItem);
        U[] GetAllItems();
        
        SO_T DefaultOrNullItem { get; }
        
        bool CanAddItem(SO_T item);
        bool TryAddItem(SO_T item);
        bool TryAddItems(SO_T item, int count);
        bool TryRemoveItem(SO_T item);
        bool TryRemoveItems(SO_T item, int count);
        
        void ClearInventory();
    }
}