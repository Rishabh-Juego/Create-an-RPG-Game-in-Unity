using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.RPG.Items.InventorySystem.Samples;

namespace TGL.RPG.Items.InventorySystem
{
    public interface IInventoryGridUiInfo 
    {
#region Properties
        GameObjectType ObjectType => GameObjectType.Inventory;
        InventoryType InventoryType { get; }
        int TotSlotsCount { get; }
#endregion Properties
        
#region GenericMethods
        IInventorySlotInfo[] GetAllSlotData();
        void InitializeData(int slotsCount, So_InventoryItem defaultOrNullItem);
        void UpdateSlotData(int index, So_InventoryItem itemSo, int count); // should handle both 'SetSlotData' and 'UpdateSlotData' logic in IInventorySlotData
        void ClearInventory();
        void DeInitialize();
#endregion GenericMethods
        
#region functionalMethods
        
#region InventoryInfoQueries
        /// <summary>
        /// How many items of given type are in the inventory grid.
        /// </summary>
        /// <param name="itemSo">The scriptable object whose data we are trying to count</param>
        /// <returns>The total count of the passed scriptable object in this inventory</returns>
        int GetTotalItemCount(So_InventoryItem itemSo);
        
        /// <summary>
        /// Checks if there is at least one empty slot in the inventory grid.
        /// </summary>
        /// <returns>bool stating if we have any empty slot</returns>
        bool HasEmptySlot();
        
        /// <summary>
        /// Checks if there is empty slot(s) and gets the count of empty slots in the inventory grid.
        /// </summary>
        /// <param name="emptySlotCount">The out variable with count of empty slots</param>
        /// <returns>bool stating if we have any empty slot</returns>
        bool HasEmptySlot(out int emptySlotCount);
#endregion InventoryInfoQueries

#region InventoryActionQueries
        /// <summary>
        /// Can we stack the given item in any one of the 'used' slots in this inventory grid
        /// </summary>
        /// <param name="itemSo"></param>
        /// <param name="itemCount"></param>
        /// <returns>is this stackable</returns>
        bool CanStackItemInAnyExistingSlot(So_InventoryItem itemSo, int itemCount);
        
        /// <summary>
        /// Can we stack the given item in one or many of the 'used' slots in this inventory grid, without using 'empty' slots
        /// </summary>
        /// <param name="itemSo">The item we want to stack</param>
        /// <param name="itemCount">The count of items to be added</param>
        /// <returns>is this stackable</returns>
        bool CanStackItemInAllExistingSlots(So_InventoryItem itemSo, int itemCount);
        
        /// <summary>
        /// Can we add the given item in 'used as well as empty slots' in this inventory grid
        /// </summary>
        /// <param name="itemSo">The item we want to stack</param>
        /// <param name="itemCount">The count of items to be added</param>
        /// <returns>is this Add-able</returns>
        bool CanAddItemsWithinAvailableSlots(So_InventoryItem itemSo, int itemCount);
        
        /// <summary>
        /// Can we remove <paramref name="itemCount"/> <paramref name="itemSo"/> from one slot of the grid
        /// </summary>
        /// <param name="itemSo">the item we want to remove</param>
        /// <param name="itemCount">the quantity of item to remove</param>
        /// <returns><c>true</c> if the items can be removed, false otherwise</returns>
        bool CanRemoveItemsFromAnyExistingSlot(So_InventoryItem itemSo, int itemCount);
        
        /// <summary>
        /// Can we remove <paramref name="itemCount"/> <paramref name="itemSo"/> from all slots together of the grid
        /// </summary>
        /// <param name="itemSo">the item we want to remove</param>
        /// <param name="itemCount">the quantity of item to remove</param>
        /// <returns><c>true</c> if the items can be removed, false otherwise</returns>
        bool CanRemoveItemsWithinAvailableSlots(So_InventoryItem itemSo, int itemCount);
#endregion InventoryActionQueries
        
#region InventoryActions
        /// <summary>
        /// Adds the given item to the inventory grid data structure.
        /// </summary>
        /// <param name="itemSo">The item to add</param>
        /// <exception cref="ArgumentException">Thrown when no available slot is found in the inventory of the specified type with extra space as well as no empty slot is found</exception>
        bool AddItem(So_InventoryItem itemSo);
        
        /// <summary>
        /// Adds the given quantity of items to the inventory grid data structure.
        /// </summary>
        /// <param name="itemSo">The item to add</param>
        /// <param name="quantity">The quantity / count of the items to be added</param>
        /// <exception cref="ArgumentException">Thrown when the item in the given quantity cannot be added to the grid</exception>
        bool AddItems(So_InventoryItem itemSo, int quantity);

        /// <summary>
        /// Remove one <paramref name="itemSo"/> from the grid
        /// </summary>
        /// <param name="itemSo">the item to remove</param>
        /// <returns><c>true</c> if the item was successfully removed, false otherwise</returns>
        bool RemoveItem(So_InventoryItem itemSo);
        
        /// <summary>
        /// Remove <paramref name="quantity"/> <paramref name="itemSo"/> from the grid
        /// </summary>
        /// <param name="itemSo">the item to remove</param>
        /// <param name="quantity">the quantity of the item to remove</param>
        /// <returns><c>true</c> if the items were successfully removed, false otherwise</returns>
        bool RemoveItems(So_InventoryItem itemSo, int quantity);
#endregion InventoryActions
        
#endregion functionalMethods
    }
}