using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem
{
    public interface IInventoryService<T, U> where T : So_InventoryData where U : IInventorySlotData<T>
    {
        UniqueType InventoryType { get; }
        event Action OnInventoryChanged;
        void Initialize(int slotSize, T defaultOrNullItem);
        U[] GetAllItems();
        
        T DefaultOrNullItem { get; }
        
        bool CanAddItem(T item);
        bool TryAddItem(T item);
        bool RemoveItemFromInventory(T item);
        
        void ClearInventory();
    }
}