using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.RPG.Items.InventorySystem.Samples;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem
{
    [Serializable]
    public abstract class InventorySlotData : IInventorySlotData // So_InventoryData
    {
        // variables
        protected string itemId;
        protected InventoryType inventoryType;
        protected string displayName;
        protected int maxStackSize;
        protected int itemCount;
        protected int slotIndex;
        protected So_InventoryData _defaultScriptableItem;

        // properties
        public string ItemId => itemId;
        public InventoryType InventoryType => inventoryType;
        public So_InventoryData EmptyItemSo => _defaultScriptableItem;
        public string DisplayName => displayName;
        public int MaxStackCount => maxStackSize;
        public bool HasData => _defaultScriptableItem != null && !string.Equals(_defaultScriptableItem.UniqueID, itemId);
        public bool IsStackable => maxStackSize > 1;
        public bool IsEmpty => string.Equals(_defaultScriptableItem.UniqueID, itemId);
        public bool IsFull => itemCount == maxStackSize;
        public int CurrentStackCount => itemCount;
        public int SlotIndex => slotIndex;
        
        // events
        public event Action<IInventorySlotData, int> OnSlotDataChanged;

        // methods
        public void Initialize(int index, So_InventoryData defaultScriptableItem)
        {
            _defaultScriptableItem = defaultScriptableItem;
            slotIndex = index;
            itemCount = 0;
            ClearSlot();
        }

        public bool SetSlotData(So_InventoryData itemData, int count)
        {
            if (inventoryType != itemData.InventoryType)
            {
                Debug.Log($"We have initialization issue: Tried to set '{itemId}'({inventoryType}) slot with mismatched inventory type '{itemData.InventoryType}' for slot index: {slotIndex}");
                return false;
            }
            itemId = itemData.UniqueID;
            inventoryType = itemData.InventoryType;
            displayName = itemData.DisplayName;
            maxStackSize = itemData.maxStackCount;
            itemCount = count;
            return true;
        }
        
        public bool UpdateSlotData(So_InventoryData itemData, int countOfItems)
        {
            if (HasData)
            {
                // if we have valid data, validate the new scriptable item received as paramenter
                if (!IsValidItemData(itemData.UniqueID))
                {
                    Debug.Log($"Tried to update '{itemId}' slot with invalid data '{itemData}' for slot index: {slotIndex}");
                    return false;
                }
            }
            else
            {
                // if we don't have valid data, just set the new scriptable item received as parameter
                if (!SetSlotData(itemData, countOfItems))
                {
                    return false;
                }
            }

            itemCount = countOfItems; // move this line to an abstract method if we need a virual method for different update logics
            OnSlotDataChanged?.Invoke(this, itemCount);
            return true;
        }

        public virtual void ClearSlot()
        {
            itemId = _defaultScriptableItem.UniqueID;
            inventoryType = _defaultScriptableItem.InventoryType;
            displayName = _defaultScriptableItem.DisplayName;
            maxStackSize = _defaultScriptableItem.maxStackCount;
            itemCount = 0;
            OnSlotDataChanged?.Invoke(this, itemCount);
        }

        protected virtual bool IsValidItemData(string itemData)
        {
            return string.Equals(itemData, itemId);
        }

        public virtual bool HasItem(So_InventoryData itemData)
        {
            return string.Equals(itemData.UniqueID, itemId);
        }
    }
}