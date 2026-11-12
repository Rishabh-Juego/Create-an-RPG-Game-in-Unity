using System;
using System.Collections.Generic;
using System.Linq;
using TGL.RPG.Items.InventorySystem.Data;
// using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    [Serializable]
    public abstract class InventoryGridUiInfo : IInventoryGridUiInfo
    {
#region Variables
        protected InventoryType inventorySpecificType;
        protected IInventorySlotInfo[] slotDataArray;
        protected int totSlotsCount;
        protected Dictionary<int, IInventorySlotInfo> slotDataDictionary = new Dictionary<int, IInventorySlotInfo>();
#endregion Variables

#region Properties
        public InventoryType InventorySpecificType => inventorySpecificType;
        public int TotSlotsCount => totSlotsCount;
#endregion Properties
        
#region GenericMethods
        public IInventorySlotInfo[] GetAllSlotData()
        {
            return slotDataArray;
        }

        public virtual void InitializeData(int slotsCount, So_InventoryItem defaultOrNullItem)
        {
	        totSlotsCount = slotsCount;
	        inventorySpecificType = defaultOrNullItem.InventorySpecificType;
            slotDataArray = new IInventorySlotInfo[slotsCount];
            for(int i=0; i < slotsCount; i++)
            {
                slotDataArray[i].Initialize(i, defaultOrNullItem);
                slotDataDictionary[i] = slotDataArray[i];
            }
        }
        
        public void UpdateSlotData(int index, So_InventoryItem itemSo, int count)
        {
	        slotDataArray[index].UpdateSlotData(itemSo, count);
        }

        public void ClearInventory()
        {
	        foreach(var slot in slotDataArray)
	        {
		        slot.ClearSlot();
	        }
        }

        public void DeInitialize()
        {
	        totSlotsCount = 0;
	        inventorySpecificType = InventoryType.Undefined;
	        int slotsCount = slotDataArray.Length;
	        for(int i=0; i < slotsCount; i++)
	        {
		        slotDataArray[i].ClearSlot();
	        }
	        slotDataDictionary = new Dictionary<int, IInventorySlotInfo>();
	        slotDataArray = null;
        }
#endregion GenericMethods

#region functionalMethods
        
#region InventoryInfoQueries
        public int GetTotalItemCount(So_InventoryItem itemSo)
        {
            int totalCount = 0;
            foreach(var slot in slotDataArray)
            {
                if (slot.HasItem(itemSo))
                {
                    totalCount += slot.CurrentStackCount;
                }
            }
            return totalCount;
        }

        public bool HasEmptySlot()
        {
            return slotDataArray.Any(x => x.IsEmpty);
        }

        public bool HasEmptySlot(out int emptySlotCount)
        {
            emptySlotCount = 0;
            foreach(var slot in slotDataArray)
            {
                if (slot.IsEmpty)
                {
                    emptySlotCount++;
                }
            }
            return emptySlotCount > 0;
        }
#endregion InventoryInfoQueries

#region InventoryActionQueries
        public virtual bool CanStackItemInAnyExistingSlot(So_InventoryItem itemSo, int itemCount)
        {
            bool canStack = false;
            foreach (var slot in slotDataArray)
            {
                if (slot.HasItem(itemSo) && slot.IsStackable && !slot.IsFull && slot.CurrentStackCount + itemCount <= slot.MaxSlotStackCount)
                {
                    canStack = true;
                    break;
                }
            }
            return canStack;
        }

        public virtual bool CanStackItemInAllExistingSlots(So_InventoryItem itemSo, int itemCount)
        {
            bool canStack = false;
            int itemCountLeft = itemCount;
            foreach (var slot in slotDataArray)
            {
                if (slot.HasItem(itemSo) && slot.IsStackable && !slot.IsFull)
                {
                    int availableSpace = slot.MaxSlotStackCount - slot.CurrentStackCount;
                    if (availableSpace >= itemCountLeft)
                    {
                        canStack = true;
                        break;
                    }
                    else
                    {
                        itemCountLeft -= availableSpace;
                    }
                }
            }

            return canStack;
        }
        
        public virtual bool CanAddItemsWithinAvailableSlots(So_InventoryItem itemSo, int itemCount)
        {
            bool canAdd = false;
            int itemCountLeft = itemCount;
            int availableSpaceCount = 0;
            bool stackableSpaceFound = false;
            foreach (var slot in slotDataArray)
            {
                if (slot.HasItem(itemSo) && slot.IsStackable && !slot.IsFull)
                {
                    availableSpaceCount = slot.MaxSlotStackCount - slot.CurrentStackCount;
                    stackableSpaceFound = true;
                }
                else if (slot.IsEmpty)
                {
                    availableSpaceCount = slot.MaxSlotStackCount;
                    stackableSpaceFound = true;
                }

                if (stackableSpaceFound)
                {
                    if (availableSpaceCount >= itemCountLeft)
                    {
                        canAdd = true;
                        break;
                    }
                    else
                    {
                        itemCountLeft -= availableSpaceCount;
                    }
                    stackableSpaceFound = false;
                }
            }

            return canAdd;
        }

        public bool CanRemoveItemsFromAnyExistingSlot(So_InventoryItem itemSo, int itemCount)
        {
	        bool canRemove = false;
	        foreach (var slot in slotDataArray)
	        {
		        if (slot.HasItem(itemSo) && slot.CurrentStackCount > itemCount)
		        {
			        canRemove = true;
			        break;
		        }
	        }
	        return canRemove;
        }
        public bool CanRemoveItemsWithinAvailableSlots(So_InventoryItem itemSo, int itemCount)
        {
	        int itemCountLeft = itemCount;
	        bool canRemove = false;
	        
	        foreach (var slot in slotDataArray)
	        {
		        if (slot.HasItem(itemSo))
		        {
			        if (slot.CurrentStackCount >= itemCountLeft)
			        {
				        canRemove = true;
				        break;
			        }
			        else
			        {
				        itemCountLeft -= slot.CurrentStackCount;
			        }
		        }
	        }

	        return canRemove;
        }
#endregion InventoryActionQueries
        
#region InventoryActions
        public virtual bool AddItem(So_InventoryItem itemSo)
        {
	        bool addedInInventory = false;
	        if (!CanAddItemsWithinAvailableSlots(itemSo, 1))
	        {
		        return addedInInventory;
	        }
	        
            // add the item in first slot with same item and available space
            for(int i=0; i<slotDataArray.Length; i++)
            {
                if(slotDataArray[i].HasItem(itemSo) && slotDataArray[i].IsStackable && !slotDataArray[i].IsFull)
                {
                    slotDataArray[i].UpdateSlotData(itemSo, slotDataArray[i].CurrentStackCount + 1);
                    addedInInventory = true;
                    break;
                }
            }
            
            // if not found, add in first empty slot
            for (int i = 0; i < slotDataArray.Length; i++)
            {
                if (slotDataArray[i].IsEmpty)
                {
                    slotDataArray[i].UpdateSlotData(itemSo, 1);
                    addedInInventory = true;
                    break;
                }
            }

            if (!addedInInventory)
            {
	            throw new ArgumentException($"{nameof(AddItem)}() returned false, but {nameof(CanAddItemsWithinAvailableSlots)} is true, something went wrong");
            }
            
            return addedInInventory;
            // throw new ArgumentException($"Could not add item '{itemData.DisplayName}' to inventory of type '{inventoryType}' - no available slot found.");
        }

        public virtual bool AddItems(So_InventoryItem itemSo, int origQuantity)
        {
	        int quantity = origQuantity;
	        bool addedInInventory = false;
	        if (!CanAddItemsWithinAvailableSlots(itemSo, quantity))
	        {
		        return addedInInventory;
	        }
	        
	        // add the item in exiting slots with same item and available space
	        for(int i=0; i<slotDataArray.Length; i++)
	        {
		        if(slotDataArray[i].HasItem(itemSo) && slotDataArray[i].IsStackable && !slotDataArray[i].IsFull)
		        {
			        if (slotDataArray[i].CurrentStackCount + quantity <= slotDataArray[i].MaxSlotStackCount)
			        {
						slotDataArray[i].UpdateSlotData(itemSo, slotDataArray[i].CurrentStackCount + quantity);
						quantity = 0;
						addedInInventory = true;
						break;
			        }
			        else
			        {
				        slotDataArray[i].UpdateSlotData(itemSo, slotDataArray[i].MaxSlotStackCount);
				        quantity -= slotDataArray[i].MaxSlotStackCount - slotDataArray[i].CurrentStackCount; // item count that got filled in already filled slot
			        }
		        }
	        }

	        if (quantity == 0)
	        {
		        return addedInInventory;
	        }
	        
	        // if not found, add in first empty slot
	        for (int i = 0; i < slotDataArray.Length; i++)
	        {
		        if (slotDataArray[i].IsEmpty)
		        {
			        if (itemSo.maxItemStackCount > quantity)
			        {
				        slotDataArray[i].UpdateSlotData(itemSo, quantity);
				        quantity = 0;
				        addedInInventory = true;
						break;
			        }
			        else
			        {
				        slotDataArray[i].UpdateSlotData(itemSo, itemSo.maxItemStackCount);
				        quantity -= itemSo.maxItemStackCount; // item count that got filled in empty slot
			        }
		        }
	        }
	        
	        if (!addedInInventory)
	        {
		        throw new ArgumentException($"{nameof(AddItems)}() returned false, but {nameof(CanAddItemsWithinAvailableSlots)} is true, something went wrong");
				// throw new ArgumentException($"Could not add '{quantity}' items of '{itemData.DisplayName}' to inventory of type '{inventoryType}' - available slots found to be insufficient.");
	        }
	        return addedInInventory;
        }

        public virtual bool RemoveItem(So_InventoryItem itemSo)
        {
	        bool removedFromInventory = false;
	        if (!CanRemoveItemsFromAnyExistingSlot(itemSo, 1))
	        {
		        return removedFromInventory;
	        }

	        for (int i = 0; i < slotDataArray.Length; i++)
	        {
		        if (slotDataArray[i].HasItem(itemSo))
		        {
			        if (slotDataArray[i].CurrentStackCount == 1)
			        {
				        slotDataArray[i].ClearSlot();
			        }
			        else
			        {
				        slotDataArray[i].UpdateSlotData(itemSo, slotDataArray[i].CurrentStackCount - 1);
			        }
			        removedFromInventory = true;
			        break;
		        }
	        }

	        if (!removedFromInventory)
	        {
		        throw new ArgumentException($"{nameof(RemoveItem)}() returned false, but {nameof(CanRemoveItemsFromAnyExistingSlot)} is true, something went wrong");
	        }

	        return removedFromInventory;
        }
        
        public virtual bool RemoveItems(So_InventoryItem itemSo, int origQuantity)
        {
	        int quantityLeft = origQuantity;
	        bool removedFromInventory = false;
	        if (!CanRemoveItemsWithinAvailableSlots(itemSo, quantityLeft))
	        {
		        return removedFromInventory;
	        }

	        for (int i = 0; i < slotDataArray.Length; i++)
	        {
		        if (slotDataArray[i].HasItem(itemSo))
		        {
			        if (slotDataArray[i].CurrentStackCount < quantityLeft)
			        {
				        quantityLeft -= slotDataArray[i].CurrentStackCount;
				        slotDataArray[i].ClearSlot();
			        }
			        else if (slotDataArray[i].CurrentStackCount > quantityLeft)
			        {
				        slotDataArray[i].UpdateSlotData(itemSo, slotDataArray[i].CurrentStackCount - quantityLeft);
				        quantityLeft = 0;
			        }
			        else
			        {
				        slotDataArray[i].ClearSlot();
				        quantityLeft = 0;
			        }
				    removedFromInventory = true;

			        if (quantityLeft == 0)
			        {
				        break;
			        }
		        }
	        }

	        if (!removedFromInventory || quantityLeft > 0)
	        {
		        throw new ArgumentException($"{nameof(RemoveItems)}() returned false, but {nameof(CanRemoveItemsWithinAvailableSlots)} is true, something went wrong");
	        }

	        return removedFromInventory;
        }
#endregion InventoryActions

#endregion functionalMethods
    }
}
