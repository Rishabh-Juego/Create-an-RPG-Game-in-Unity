using System;
using System.Collections.Generic;
using System.Linq;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    public abstract class InventoryPlayerService<SO_U, T> : IInventoryService<SO_U, T> where SO_U : So_InventoryData where T : InventorySlotData<SO_U>
    {
        /// <summary>
        ///  Number of slots in the inventory
        /// </summary>
        protected int slotCount;
        
        /// <summary>
        /// Data of items in the inventory slots
        /// </summary>
        protected T[] slotItems;

        protected SO_U _defaultOrNullItem;
        protected bool _initialized;

        public event Action OnInventoryChanged;
        public abstract UniqueType InventoryType { get; }

        public T[] GetAllItems() => slotItems;
        public SO_U DefaultOrNullItem => _defaultOrNullItem;
        
        public void Initialize(int slotSize, SO_U defaultOrNullItem)
        {
            slotCount = slotSize;
            _defaultOrNullItem = defaultOrNullItem;
            slotItems = new T[slotCount];
            for(int i = 0; i < slotCount; i++)
            {
                T slotItem = Activator.CreateInstance<T>(); // because U is constrained to InventorySlotData<T> which is abstract
                slotItem.Initialize(i, defaultOrNullItem);
                slotItems[i] = slotItem;
            }

            _initialized = true;
        }

        /// <summary>
        /// Can we add item to the inventory in any slot?  <br/>
        /// for actions like picking up from ground, where the inventory is not visible so any slot can be used
        /// </summary>
        /// <param name="item">The item we want to know can be added</param>
        /// <returns>bool stating item can be added</returns>
        public bool CanAddItem(SO_U item)
        {
            if (!_initialized)
            {
                Debug.LogError($"Cannot check CanAddItem for uninitialized inventory of type {InventoryType}.");
                return false;
            }

            if (_defaultOrNullItem == item)
            {
                Debug.LogError($"Cannot add default item to inventory of type {InventoryType}.");
                return false;
            }
            
            // if we have at least one empty slot, we can add the item
            if (slotItems.Any(x => x.ContainedItemData == _defaultOrNullItem))
            {
                return true;
            }
            
            // if no empty slots, we check if the same item is available and can be stacked
            IEnumerable<T> sameItemSlots = slotItems.Where(x => x.ContainedItemData == item);
            foreach (T sameItemSlot in sameItemSlots)
            {
                // we found a slot with the same item, we can add it here, keeping empty slots for other items
                if (sameItemSlot.ItemCount < sameItemSlot.ContainedItemData.maxStackCount)
                {
                    return true;
                }
                // else continue to look for other slots with the same item
            }
            
            // if we cannot stack or no empty slots, we cannot add the item
            return false;
        }

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
                if (sameItemSlot.ItemCount < sameItemSlot.ContainedItemData.maxStackCount)
                {
                    sameItemSlot.UpdateSlot(sameItemSlot.ContainedItemData, sameItemSlot.ItemCount + 1);
                    OnInventoryChanged?.Invoke();
                    return true;
                }
                // else continue to look for other slots with the same item
            }

            // Since we could not stack, we look for an empty slot
            T emptySlot = slotItems.FirstOrDefault(x => x.ContainedItemData == _defaultOrNullItem);
            if (emptySlot != null)
            {
                emptySlot.UpdateSlot(item, 1);
                OnInventoryChanged?.Invoke();
                return true;
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
            int availableItemCountInInventory = slotItems.Where(x => x.ContainedItemData == item).Sum(x => x.ItemCount);
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
            foreach (T sameItemSlot in slotItems.Where(x => x.ContainedItemData == item && x.ItemCount < x.ContainedItemData.maxStackCount))
            {
                int spaceInThisSlot = sameItemSlot.ContainedItemData.maxStackCount - sameItemSlot.ItemCount;
                if (itemsLeftToAdd <= spaceInThisSlot)
                {
                    // we can add all remaining items in this slot
                    sameItemSlot.UpdateSlot(sameItemSlot.ContainedItemData, sameItemSlot.ItemCount + itemsLeftToAdd);
                    OnInventoryChanged?.Invoke();
                    AggregateInventorySlotsForItem(item);
                    return true;
                }
                else
                {
                    // fill this slot and continue to next
                    sameItemSlot.UpdateSlot(sameItemSlot.ContainedItemData, sameItemSlot.ContainedItemData.maxStackCount);
                    itemsLeftToAdd -= spaceInThisSlot;
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
                        emptySlot.UpdateSlot(item, itemsLeftToAdd);
                        OnInventoryChanged?.Invoke();
                        AggregateInventorySlotsForItem(item);
                        return true;
                        // loop stops at return, so break is not needed
                    }
                    else
                    {
                        emptySlot.UpdateSlot(emptySlot.ContainedItemData, item.maxStackCount);
                        itemsLeftToAdd -= item.maxStackCount;
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
                sameItemSlotsCount = sameItemSlot.ItemCount;
                // we found a slot with the same item, we can add it
                if (sameItemSlotsCount >= 1)
                {
                    if (sameItemSlotsCount == 1)
                    {
                        sameItemSlot.ClearSlot();
                    }
                    else
                    {
                        sameItemSlot.UpdateSlot(sameItemSlot.ContainedItemData, sameItemSlotsCount - 1);
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
            int totalItemCount = sameItemSlots.Sum(x => x.ItemCount);
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
                sameItemSlots[i].UpdateSlot(item, itemsToAddInThisSlot);
                itemsAdded += itemsToAddInThisSlot;
            }
        }

        /// <summary>
        /// clears all items from inventory
        /// </summary>
        public void ClearInventory()
        {
            for(int i = 0; i < slotCount; i++)
            {
                slotItems[i].ClearSlot();
            }
            OnInventoryChanged?.Invoke();
        }
    }
}