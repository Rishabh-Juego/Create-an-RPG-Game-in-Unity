using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem
{
    public interface IInventorySlotData<SO_T> where SO_T : So_InventoryData
    {
        event Action<IInventorySlotData<SO_T>, int> OnSlotChanged;
        
        SO_T ContainedItemData { get; }
        
        int ItemCount { get; }
        int SlotIndex { get; }

        void Initialize(int index, SO_T defaultItem);
        void UpdateSlot(SO_T itemData, int count);
        void ClearSlot();
    }
}