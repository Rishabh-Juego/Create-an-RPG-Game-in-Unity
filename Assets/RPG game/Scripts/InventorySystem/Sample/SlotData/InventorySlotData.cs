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
        
        public event Action<IInventorySlotData<T>, int> OnSlotDataChanged;
        
        public T ContainedItemData => itemData;
        public int ItemCount => itemCount;
        public int SlotIndex => slotIndex;

        public void Initialize(int index, T defaultScriptableItem)
        {
            defaultItemData = defaultScriptableItem;
            slotIndex = index;
            ClearSlot();
        }
        
        public bool UpdateSlot(T scriptableItem, int countOfItems)
        {
            if (!IsValidItemData(scriptableItem))
            {
                return false;
            }
            itemData = scriptableItem;
            itemCount = countOfItems;
            OnSlotDataChanged?.Invoke(this, countOfItems);
            return true;
        }

        public void ClearSlot()
        {
            itemData = defaultItemData;
            itemCount = 0;
        }

        protected virtual bool IsValidItemData(T itemData)
        {
            return defaultItemData.InventoryType == itemData.InventoryType;
        }
    }
}