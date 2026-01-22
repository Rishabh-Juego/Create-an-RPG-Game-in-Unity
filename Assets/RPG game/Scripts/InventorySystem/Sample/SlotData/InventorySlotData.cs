using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem
{
    public abstract class InventorySlotData<T> : IInventorySlotData<T> where T : So_InventoryData
    {
        protected T itemData; 
        protected int itemCount;
        protected int slotIndex;
        protected T defaultItemData; 
        
        public event Action<IInventorySlotData<T>, int> OnSlotChanged;
        
        public T ContainedItemData => itemData;
        public int ItemCount => itemCount;
        public int SlotIndex => slotIndex;

        public void Initialize(int index, T defaultScriptableItem)
        {
            defaultItemData = defaultScriptableItem;
            slotIndex = index;
            ClearSlot();
        }
        
        public void UpdateSlot(T scriptableItem, int countOfItems)
        {
            itemData = scriptableItem;
            itemCount = countOfItems;
            OnSlotChanged?.Invoke(this, countOfItems);
        }

        public void ClearSlot()
        {
            itemData = defaultItemData;
            itemCount = 0;
        }
    }
}