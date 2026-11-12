using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.RPG.Items.InventorySystem.Samples;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem
{
    [Serializable]
    public abstract class InventorySlotUiInfo : IInventorySlotInfo // So_InventoryData
    {
        // variables
        protected string itemId;
        protected InventoryType inventoryType;
        protected string displayName;
        protected int maxSlotStackSize;
        protected int itemCount;
        protected int slotIndex;
        protected So_InventoryItem _defaultScriptableItem;
        protected So_InventoryItem _currentScriptableItem;

        // properties
        public string ItemId => itemId;
        public InventoryType InventoryType => inventoryType;
        public string DisplayName => displayName;
        public bool IsStackable => maxSlotStackSize > 1;
        public int MaxSlotStackCount => maxSlotStackSize;
        public bool IsFull => itemCount == maxSlotStackSize;
        public int CurrentStackCount => itemCount;
        public int SlotIndex => slotIndex;
        public So_InventoryItem EmptyItemSo => _defaultScriptableItem;
        public So_InventoryItem CurrItemSo => _currentScriptableItem;
        public bool HasData => _defaultScriptableItem != null && !string.Equals(_defaultScriptableItem.UniqueID, itemId);
        public bool IsEmpty => string.Equals(_defaultScriptableItem.UniqueID, itemId);
        
        // events
        public event Action<IInventorySlotInfo, int> OnSlotDataChanged;

        // methods
        public void Initialize(int index, So_InventoryItem defaultScriptableItem)
        {
            _defaultScriptableItem = defaultScriptableItem;
            slotIndex = index;
            itemCount = 0;
            ClearSlot();
        }

        public bool SetSlotData(So_InventoryItem itemSo, int count)
        {
            if (inventoryType != itemSo.InventoryType)
            {
                Debug.LogError($"We have initialization issue: Tried to set '{itemId}'({inventoryType}) slot with mismatched inventory type '{itemSo.InventoryType}' for slot index: {slotIndex}");
                return false;
            }
            itemId = itemSo.UniqueID;
            inventoryType = itemSo.InventoryType;
            displayName = itemSo.DisplayName;
            maxSlotStackSize = itemSo.maxItemStackCount;
            itemCount = count;
            _currentScriptableItem = itemSo;
            if (itemCount > maxSlotStackSize)
            {
	            Debug.LogError($"Item count '{itemCount}' exceeds max slot stack size '{maxSlotStackSize}' for item '{itemId}' in slot index: {slotIndex}");
	            return false;
            }
            return true;
        }
        
        public bool UpdateSlotData(So_InventoryItem itemSo, int countOfItems)
        {
            if (HasData)
            {
                // if we have valid data, validate the new scriptable item received as paramenter
                if (!IsValidItemData(itemSo.UniqueID))
                {
                    Debug.LogError($"Tried to update '{itemId}' slot with invalid data '{itemSo}' for slot index: {slotIndex}");
                    return false;
                }
            }
            else
            {
                // if we don't have valid data, just set the new scriptable item received as parameter
                if (!SetSlotData(itemSo, countOfItems))
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
            maxSlotStackSize = _defaultScriptableItem.maxItemStackCount;
            _currentScriptableItem = _defaultScriptableItem;
            itemCount = 0;
            OnSlotDataChanged?.Invoke(this, itemCount);
        }

        protected virtual bool IsValidItemData(string itemData)
        {
            return string.Equals(itemData, itemId);
        }

        public virtual bool HasItem(So_InventoryItem itemSo)
        {
            return string.Equals(itemSo.UniqueID, itemId);
        }
    }
}