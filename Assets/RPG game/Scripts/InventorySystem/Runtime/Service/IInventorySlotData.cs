using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem
{
    public interface IInventorySlotData<T> where T : So_InventoryData
    {
        event Action<IInventorySlotData<T>, int> OnSlotChanged;
        
        T ContainedItemData { get; }
        
        int ItemCount { get; }
        int SlotIndex { get; }

        void Initialize(int index, T defaultItem);
        void UpdateSlot(T itemData, int count);
        void ClearSlot();
    }
}