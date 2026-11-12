using System;
using System.Collections.Generic;
using System.Linq;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    public abstract class InventoryPlayerService : IInventoryService
    {
#region Variables
        protected IInventoryGridUiInfo gridUiInfo;
        protected So_InventoryItem _defaultOrNullItemSo;
        protected List<So_InventoryItem> _allInventoryItemsSo; // TODO: Need to update the list when we add slots or update slots
        protected Dictionary<string, So_InventoryItem> inventoryItemsDict = new Dictionary<string, So_InventoryItem>(); // TODO: Need to update the list when we add slots or update slots
        protected bool _initialized;
#endregion  Variables

#region Properties
        public IInventoryGridUiInfo InventoryInfo => gridUiInfo;
        public So_InventoryItem DefaultOrNullItemSo => _defaultOrNullItemSo;
        public So_InventoryItem[] AllInventoryItemsSo => _allInventoryItemsSo.ToArray();
        public Dictionary<string, So_InventoryItem> AllInventoryItemsDict => inventoryItemsDict;
        public Dictionary<So_InventoryItem, int> AllInventoryItemsCountDict => AllInventoryItemsSo.Distinct().ToDictionary(x => x, x => gridUiInfo.GetTotalItemCount(x));
        public event Action<IInventoryGridUiInfo> OnInventoryDataChanged;
#endregion Properties

#region GenericMethods
        public void Initialize(IInventoryGridUiInfo gridObj, int slotSize, So_InventoryItem defaultOrNullItem)
        {
            gridUiInfo = gridObj; // gridData = Activator.CreateInstance<IInventoryGridData>();
            gridUiInfo.InitializeData(slotSize, defaultOrNullItem);
            _defaultOrNullItemSo = defaultOrNullItem;
            _allInventoryItemsSo = new List<So_InventoryItem>();
            inventoryItemsDict = new Dictionary<string, So_InventoryItem>();
            
            OnInventoryDataChanged?.Invoke(InventoryInfo);
            _initialized = true;
        }

        public void DeInitialize()
        {
	        _defaultOrNullItemSo = null;
	        _allInventoryItemsSo = new List<So_InventoryItem>();
	        inventoryItemsDict = new Dictionary<string, So_InventoryItem>();
	        gridUiInfo.DeInitialize();
	        gridUiInfo = null;
	        _initialized = false;
	        OnInventoryDataChanged?.Invoke(InventoryInfo);
        }

        public void ClearInventory()
        {
            if (!_initialized)
            {
                return;
            }
            
            gridUiInfo.ClearInventory();
            _allInventoryItemsSo.Clear();
            inventoryItemsDict.Clear();
            OnInventoryDataChanged?.Invoke(InventoryInfo);
        }

        public IInventorySlotInfo[] GetAllItemSlots()
        {
	        return gridUiInfo.GetAllSlotData();
        }
#endregion GenericMethods

#region functionalMethods
        public bool CanAddItem(So_InventoryItem item)
        {
            if (!_initialized)
            {
                Debug.LogError($"Cannot check CanAddItem for uninitialized inventory of type {item.InventoryType}.");
                return false;
            }

            if (_defaultOrNullItemSo == item)
            {
                Debug.LogError($"Cannot add default item to inventory of type {item.InventoryType}.");
                return false;
            }
            
            // if we have at least one empty slot, we can add the item as the count is 1
            if (gridUiInfo.HasEmptySlot())
            {
	            return true;
            }
            
            // if no empty slots, we check if the same item is available and can be stacked
            if (gridUiInfo.CanStackItemInAnyExistingSlot(item, 1))
            {
	            return true;
            }
            
            return false;
        }

        public bool CanAddItems(So_InventoryItem item, int count)
        {
            if (!_initialized)
            {
                Debug.LogError($"Cannot check CanAddItem for uninitialized inventory of type {item.InventoryType}.");
                return false;
            }

            if (_defaultOrNullItemSo == item)
            {
                Debug.LogError($"Cannot add default item to inventory of type {item.InventoryType}.");
                return false;
            }
            
            // we check if the same item is available and can be stacked in all available slots
            if (gridUiInfo.CanStackItemInAllExistingSlots(item, count))
            {
                return true;
            }
            
            // if we cannot stack in existing slots, we check if we can add items in existing as well as empty slots together
            if (gridUiInfo.CanAddItemsWithinAvailableSlots(item, count))
            {
                return true;
            }
            
            // if we cannot add the items, return false
            return false;
        }

        public bool TryAddItem(So_InventoryItem item)
        {
            if (!CanAddItem(item))
            {
                return false;
            }
            
            bool added = gridUiInfo.AddItem(item);
            if (added && !_allInventoryItemsSo.Contains(item))
            {
	            _allInventoryItemsSo.Add(item);
            }
            return added;
        }

        public bool TryAddItems(So_InventoryItem item, int count)
        {
	        if (!CanAddItems(item, count))
	        {
		        return false;
	        }
	        bool added = gridUiInfo.AddItems(item, count);
	        if (added && !_allInventoryItemsSo.Contains(item))
	        {
		        _allInventoryItemsSo.Add(item);
	        }
	        return added;
        }

        public bool TryRemoveItem(So_InventoryItem item)
        {
	        if (!_initialized)
	        {
		        Debug.LogError($"Cannot try to remove item for uninitialized inventory of type {item.InventoryType}.");
		        return false;
	        }

	        if (_defaultOrNullItemSo == item)
	        {
		        Debug.LogError($"Cannot remove default item from inventory of type {item.InventoryType}.");
		        return false;
	        }

	        if (gridUiInfo.GetTotalItemCount(item) > 1)
	        {
		        return false;
	        }

	        throw;
        }

        public bool TryRemoveItems(So_InventoryItem item, int count)
        {
	        throw;
        }

        protected void AggregateInventorySlotsForItem(So_InventoryItem item)
        {
	        // Merge all slots with same Items so that we do not use too many slots when few slots are enough
	        throw;
        }
        
#endregion functionalMethods
        
        /*-------*-------*-------*-------*-------*-------*-------*-------*-------*/
        /*-------*-------*-------*-------*       *-------*-------*-------*-------*/
        /*-------*-------*-------*-------*-------*-------*-------*-------*-------*/
        
        
        

        /// <summary>
        /// Try to add item anywhere, for actions like picking up from ground, <br/>
        /// where the inventory is not visible so any slot can be used
        /// </summary>
        /// <param name="item">The item we want to add</param>
        /// <returns>bool stating if item was added</returns>
        public bool TryAddItem(SO_U item)
        {
            if (!_initialized)
            {
                Debug.LogError($"Cannot try to add item for uninitialized inventory of type {InventoryType}.");
                return false;
            }

            if (_defaultOrNullItem == item)
            {
                Debug.LogError($"Cannot add default item to inventory of type {InventoryType}.");
                return false;
            }
            
            // we check if the same item is available and can be stacked
            IEnumerable<T> sameItemSlots = slotItems.Where(x => x.ContainedItemData == item);
            foreach (T sameItemSlot in sameItemSlots)
            {
                // we found a slot with the same item, we can add it here, keeping empty slots for other items
                if (sameItemSlot.CurrentStackCount < sameItemSlot.ContainedItemData.maxStackCount)
                {
                    if (sameItemSlot.UpdateSlotData(sameItemSlot.ContainedItemData, sameItemSlot.CurrentStackCount + 1))
                    {
                        OnInventoryChanged?.Invoke();
                        return true;
                    }
                    else
                    {
                        Debug.LogError($"We were expecting to be able to add item {item.name} to inventory of type {InventoryType} but something went wrong.", item);
                        return false;
                    }
                }
                // else continue to look for other slots with the same item
            }

            // Since we could not stack, we look for an empty slot
            T emptySlot = slotItems.FirstOrDefault(x => x.ContainedItemData == _defaultOrNullItem);
            if (emptySlot != null)
            {
                if (emptySlot.UpdateSlotData(item, 1))
                {
                    OnInventoryChanged?.Invoke();
                    return true;
                }
                else
                {
                    Debug.LogError($"We were expecting to be able to add item {item.name} to inventory of type {InventoryType} but something went wrong.", item);
                    return false;
                }
            }

            return false;
        }
        
        public bool TryAddItems(SO_U item, int itemCount)
        {
            if (!_initialized)
            {
                Debug.LogError($"Cannot try to remove item for uninitialized inventory of type {InventoryType}.");
                return false;
            }

            if (_defaultOrNullItem == item)
            {
                Debug.LogError($"Cannot remove default item from inventory of type {InventoryType}.");
                return false;
            }
            
            int emptySlotCount = slotItems.Count(x => x.ContainedItemData == _defaultOrNullItem);
            int sameItemSlotsCount = slotItems.Count(x => x.ContainedItemData == item);
            int availableItemCountInInventory = slotItems.Where(x => x.ContainedItemData == item).Sum(x => x.CurrentStackCount);
            int availableSpaceInSameItemSlots = sameItemSlotsCount * item.maxStackCount - availableItemCountInInventory;
            int maxCountIfUsingEmptySlots = emptySlotCount * item.maxStackCount;
            int maxInventoryAddCapacity = availableSpaceInSameItemSlots + maxCountIfUsingEmptySlots;
            if (itemCount > maxInventoryAddCapacity)
            {
                Debug.LogWarning($"we can add {maxInventoryAddCapacity} items of type {item.name} but tried to add {itemCount} items.");
                return false;
            }

            int itemsLeftToAdd = itemCount;
            // add in same item slots first
            foreach (T sameItemSlot in slotItems.Where(x => x.ContainedItemData == item && x.CurrentStackCount < x.ContainedItemData.maxStackCount))
            {
                int spaceInThisSlot = sameItemSlot.ContainedItemData.maxStackCount - sameItemSlot.CurrentStackCount;
                if (itemsLeftToAdd <= spaceInThisSlot)
                {
                    // we can add all remaining items in this slot
                    if (sameItemSlot.UpdateSlotData(sameItemSlot.ContainedItemData, sameItemSlot.CurrentStackCount + itemsLeftToAdd))
                    {
                        OnInventoryChanged?.Invoke();
                        AggregateInventorySlotsForItem(item);
                        return true;
                    }
                    else
                    {
                        Debug.LogError($"We were expecting to be able to add item {item.name} to inventory of type {InventoryType} but something went wrong.", item);
                        return false;
                    }
                }
                else
                {
                    // fill this slot and continue to next
                    if (sameItemSlot.UpdateSlotData(sameItemSlot.ContainedItemData, sameItemSlot.ContainedItemData.maxStackCount))
                    {
                        itemsLeftToAdd -= spaceInThisSlot;
                    }
                    else
                    {
                        Debug.LogError($"We were expecting to be able to add item {item.name} to inventory of type {InventoryType} but something went wrong.", item);
                        return false;
                    }
                }
            }

            if (itemsLeftToAdd > 0)
            {
                // add items in empty slots
                foreach (T emptySlot in slotItems.Where(x => x.ContainedItemData == _defaultOrNullItem))
                {
                    if (itemsLeftToAdd <= item.maxStackCount)
                    {
                        // we can add all remaining items in this slot
                        if (emptySlot.UpdateSlotData(item, itemsLeftToAdd))
                        {
                            OnInventoryChanged?.Invoke();
                            AggregateInventorySlotsForItem(item);
                            return true;
                            // loop stops at return, so break is not needed
                        }
                        else
                        {
                            Debug.LogError($"We were expecting to be able to add item {item.name} to inventory of type {InventoryType} but something went wrong.", item);
                            return false;
                        }
                    }
                    else
                    {
                        if (emptySlot.UpdateSlotData(emptySlot.ContainedItemData, item.maxStackCount))
                        {
                            itemsLeftToAdd -= item.maxStackCount;
                        }
                        else
                        {
                            Debug.LogError($"We were expecting to be able to add item {item.name} to inventory of type {InventoryType} but something went wrong.", item);
                            return false;
                        }
                    }
                }
            }

            Debug.LogError($"We had the capacity to add {itemCount} items of type {item.name} but something went wrong.");
            return false;
        }

        /// <summary>
        /// Try to remove item from anywhere, for actions like unlocking a door using key from inventory, <br/>
        /// where the inventory is not visible so any slot can be used
        /// </summary>
        /// <param name="item">The item we want to get from the inventory</param>
        /// <returns>was the item removed from the inventory?</returns>
        public bool TryRemoveItem(SO_U item)
        {
            if (!_initialized)
            {
                Debug.LogError($"Cannot try to remove item for uninitialized inventory of type {InventoryType}.");
                return false;
            }

            if (_defaultOrNullItem == item)
            {
                Debug.LogError($"Cannot remove default item from inventory of type {InventoryType}.");
                return false;
            }

            int sameItemSlotsCount;
            T sameItemSlot = slotItems.FirstOrDefault(x => x.ContainedItemData == item);
            if (sameItemSlot != null)
            {
                sameItemSlotsCount = sameItemSlot.CurrentStackCount;
                // we found a slot with the same item, we can add it
                if (sameItemSlotsCount >= 1)
                {
                    if (sameItemSlotsCount == 1)
                    {
                        sameItemSlot.ClearSlot();
                    }
                    else
                    {
                        if (!sameItemSlot.UpdateSlotData(sameItemSlot.ContainedItemData, sameItemSlotsCount - 1))
                        {
                            Debug.LogError($"We were expecting to be able to add item {item.name} to inventory of type {InventoryType} but something went wrong.", item);
                            return false;
                        }
                    }

                    AggregateInventorySlotsForItem(item);
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }
            return false;
        }

        public bool TryRemoveItems(SO_U item, int itemCount)
        {
            Debug.LogError($"We have not implemented {nameof(TryRemoveItems)} in inventory of type {InventoryType} yet.");
            return false;
        }
        
        /// <summary>
        /// after change of an item, we need to aggregate the slots to avoid fragmentation of same items across multiple slots
        /// </summary>
        /// <param name="item"></param>
        protected void AggregateInventorySlotsForItem(SO_U item)
        {
            List<T> sameItemSlots = slotItems.Where(x => x.ContainedItemData == item).ToList();
            int totalItemCount = sameItemSlots.Sum(x => x.CurrentStackCount);
            int numberOfSlotsBeingUsed = sameItemSlots.Count();
            
            // calculate how many slots are needed to store all actual items
            int slotsNeeded = (int)Math.Ceiling((double)totalItemCount / item.maxStackCount);
            
            // calculate extra slots being used, if any
            int extraSlots = numberOfSlotsBeingUsed - slotsNeeded;

            if (extraSlots == 0)
            {
                return;
            }

            // create variables to track items added
            int itemsAdded = 0;
            int itemsToAddInThisSlot;
            // clear extra slots from the end
            for (int i = numberOfSlotsBeingUsed - 1; i > (numberOfSlotsBeingUsed - extraSlots) - 1; i--)
            {
                sameItemSlots[i].ClearSlot();
            }
            // re-distribute items in the needed slots
            for (int i = 0; i < slotsNeeded; i++)
            {
                itemsToAddInThisSlot = Math.Min(item.maxStackCount, totalItemCount - itemsAdded);
                if (sameItemSlots[i].UpdateSlotData(item, itemsToAddInThisSlot))
                {
                    itemsAdded += itemsToAddInThisSlot;
                }
                else
                {
                    Debug.LogError($"We were expecting to be able to add item {item.name} to inventory of type {InventoryType} but something went wrong.", item);
                    return;
                }
            }
        }
    }
}