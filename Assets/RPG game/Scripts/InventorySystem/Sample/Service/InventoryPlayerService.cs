using System;
using System.Collections.Generic;
using System.Linq;
using TGL.RPG.Items.InventorySystem.Data;
// using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    public abstract class InventoryPlayerService : IInventoryService
    {
#region Variables
        protected IInventoryGridUiInfo gridUiInfo;
        protected So_InventoryItem _defaultOrNullItemSo;
        protected List<So_InventoryItem> _allInventoryItemsSo;
        protected Dictionary<string, So_InventoryItem> inventoryItemsDict = new Dictionary<string, So_InventoryItem>(); // TODO: Need to update the list when we add slots or update slots
        protected bool _initialized;
#endregion  Variables

#region Properties
        public IInventoryGridUiInfo InventoryInfo => gridUiInfo;
        public So_InventoryItem DefaultOrNullItemSo => _defaultOrNullItemSo;
        public So_InventoryItem[] AllInventoryItemsSo => _allInventoryItemsSo.ToArray();
        public Dictionary<string, So_InventoryItem> AllInventoryItemsDict => inventoryItemsDict; // TODO: Need to update the list when we add slots or update slots
        public Dictionary<So_InventoryItem, int> AllInventoryItemsCountDict => AllInventoryItemsSo.Distinct().ToDictionary(x => x, x => gridUiInfo.GetTotalItemCount(x));
        public event Action<IInventoryGridUiInfo> OnInventoryDataChanged;
        public InventoryType InventorySpecificType => InventoryType.Undefined;
#endregion Properties

#region GenericMethods
        public void Initialize(IInventoryGridUiInfo gridObj, int slotSize, So_InventoryItem defaultOrNullItem)
        {
            gridUiInfo = gridObj; // gridData = Activator.CreateInstance<IInventoryGridData>();
            if (defaultOrNullItem.InventorySpecificType != InventorySpecificType)
            {
	            UnityEngine.Debug.LogError($"Cannot initialize inventory of type {InventorySpecificType} as the default item is of {defaultOrNullItem.InventorySpecificType} Type.");
	            return;
            }
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
	            UnityEngine.Debug.LogError($"Cannot check CanAddItem for uninitialized inventory of type {InventorySpecificType}.");
                return false;
            }

            if (_defaultOrNullItemSo == item)
            {
	            UnityEngine.Debug.LogError($"Cannot add default item to inventory of type {InventorySpecificType}.");
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
	            UnityEngine.Debug.LogError($"Cannot check CanAddItem for uninitialized inventory of type {InventorySpecificType}.");
                return false;
            }

            if (_defaultOrNullItemSo == item)
            {
	            UnityEngine.Debug.LogError($"Cannot add default item to inventory of type {InventorySpecificType}.");
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
		        UnityEngine.Debug.LogError($"Cannot try to remove item for uninitialized inventory of type {InventorySpecificType}.");
		        return false;
	        }

	        if (_defaultOrNullItemSo == item)
	        {
		        UnityEngine.Debug.LogError($"Cannot remove default item from inventory of type {InventorySpecificType}.");
		        return false;
	        }

	        // if the grid does not have a total count higher or equal to quantity, we cannot remove any items
	        if (gridUiInfo.GetTotalItemCount(item) < 1)
	        {
		        return false;
	        }
	        
	        bool removed = gridUiInfo.RemoveItem(item);
	        if (removed && gridUiInfo.GetTotalItemCount(item) == 0 && _allInventoryItemsSo.Contains(item))
	        {
		        _allInventoryItemsSo.Remove(item);
	        }
	        return removed;
        }

        public bool TryRemoveItems(So_InventoryItem item, int count)
        {
	        if (!_initialized)
	        {
		        UnityEngine.Debug.LogError($"Cannot try to remove item for uninitialized inventory of type {InventorySpecificType}.");
		        return false;
	        }

	        if (_defaultOrNullItemSo == item)
	        {
		        UnityEngine.Debug.LogError($"Cannot remove default item from inventory of type {InventorySpecificType}.");
		        return false;
	        }

	        // if the grid does not have a total count higher or equal to quantity, we cannot remove any items
	        if (gridUiInfo.GetTotalItemCount(item) < count)
	        {
		        return false;
	        }
	        
	        bool removed = gridUiInfo.RemoveItems(item, count);
	        if (removed && gridUiInfo.GetTotalItemCount(item) == 0 && _allInventoryItemsSo.Contains(item))
	        {
		        _allInventoryItemsSo.Remove(item);
	        }
	        return removed;
        }

        protected bool AggregateInventorySlotsForItem(So_InventoryItem item)
        {
	        if (!_initialized)
	        {
		        UnityEngine.Debug.LogError($"Cannot try to Aggregate item for uninitialized inventory of type {InventorySpecificType}.");
		        return false;
	        }

	        if (_defaultOrNullItemSo == item)
	        {
		        UnityEngine.Debug.LogError($"Cannot aggregate default item from inventory of type {InventorySpecificType}.");
		        return false;
	        }
	        // Merge all slots with same Items so that we do not use too many slots when few slots are enough
	        List<IInventorySlotInfo> allItemSlotsInUse = gridUiInfo.GetAllSlotData().Where(x=> x.CurrItemSo.UniqueID == item.UniqueID).ToList(); // TODO: Might be wrong
	        throw;
	        throw new NotImplementedException();
        }
        
#endregion functionalMethods
        
/*
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
*/
    }
}