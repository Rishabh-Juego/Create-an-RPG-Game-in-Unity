using System;
using System.Collections.Generic;
using System.Linq;
using TGL.RPG.Items.InventorySystem.Data;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    [Serializable]
    public abstract class InventoryGridData : IInventoryGridData
    {
        protected InventoryType inventoryType;
        protected IInventorySlotData[] slotDataArray;
        protected int slotCount;

        public InventoryType InventoryType => inventoryType;
        public int SlotCount => slotCount;
        protected Dictionary<int, IInventorySlotData> slotDataDictionary = new Dictionary<int, IInventorySlotData>();
        
        
        public IInventorySlotData[] GetAllSlotData()
        {
            return slotDataArray;
        }

        public virtual void InitializeData(int slotSize, So_InventoryData defaultOrNullItem)
        {
            slotCount = slotSize;
            inventoryType = defaultOrNullItem.InventoryType;
            slotDataArray = new IInventorySlotData[slotSize];
            for(int i=0; i < slotSize; i++)
            {
                slotDataArray[i].Initialize(slotSize, defaultOrNullItem);
                slotDataDictionary[i] = slotDataArray[i];
            }
        }

        public void UpdateSlotData(int index, So_InventoryData itemData, int count)
        {
            slotDataArray[index].UpdateSlotData(itemData, count);
        }

        public void ClearInventory()
        {
            foreach(var slot in slotDataArray)
            {
                slot.ClearSlot();
            }
        }

        public int GetTotalItemCount(So_InventoryData itemData)
        {
            int totalCount = 0;
            foreach(var slot in slotDataArray)
            {
                if (slot.HasItem(itemData))
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

        public virtual bool CanStackItemInAnySlot(So_InventoryData itemData, int itemCount)
        {
            bool canStack = false;
            foreach (var slot in slotDataArray)
            {
                if (slot.HasItem(itemData) && slot.IsStackable && !slot.IsFull && slot.CurrentStackCount + itemCount <= slot.MaxStackCount)
                {
                    canStack = true;
                    break;
                }
            }
            return canStack;
        }

        public virtual bool CanStackItemInExistingSlots(So_InventoryData itemData, int itemCount)
        {
            bool canStack = false;
            int itemCountLeft = itemCount;
            foreach (var slot in slotDataArray)
            {
                if (slot.HasItem(itemData) && slot.IsStackable && !slot.IsFull)
                {
                    int availableSpace = slot.MaxStackCount - slot.CurrentStackCount;
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

        public virtual bool CanAddItemsWithinAvailableSlots(So_InventoryData itemData, int itemCount)
        {
            
            bool canStack = false;
            int itemCountLeft = itemCount;
            int availableSpace = 0;
            bool subtractAvailableSpace = false;
            foreach (var slot in slotDataArray)
            {
                subtractAvailableSpace = false;
                if (slot.HasItem(itemData) && slot.IsStackable && !slot.IsFull)
                {
                    availableSpace = slot.MaxStackCount - slot.CurrentStackCount;
                    subtractAvailableSpace = true;
                }
                else if (slot.IsEmpty)
                {
                    availableSpace = slot.MaxStackCount;
                    subtractAvailableSpace = true;
                }

                if (subtractAvailableSpace)
                {
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

        public virtual void AddItem(So_InventoryData itemData)
        {
            // add the item in first slot with same item and available space
            for(int i=0; i<slotDataArray.Length; i++)
            {
                if(slotDataArray[i].HasItem(itemData) && slotDataArray[i].IsStackable && !slotDataArray[i].IsFull)
                {
                    slotDataArray[i].UpdateSlotData(itemData, slotDataArray[i].CurrentStackCount + 1);
                    break;
                }
            }
            
            // if not found, add in first empty slot
            for (int i = 0; i < slotDataArray.Length; i++)
            {
                if (slotDataArray[i].IsEmpty)
                {
                    slotDataArray[i].UpdateSlotData(itemData, 1);
                    break;
                }
            }
            
            throw new ArgumentException($"Could not add item '{itemData.DisplayName}' to inventory of type '{inventoryType}' - no available slot found.");
        }
        
        void AddItems(So_InventoryData itemData, int quantity);
    }
}