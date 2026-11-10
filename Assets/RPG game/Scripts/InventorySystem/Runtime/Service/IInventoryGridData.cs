using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.RPG.Items.InventorySystem.Samples;

namespace TGL.RPG.Items.InventorySystem
{
    public interface IInventoryGridData 
    {
        #region Properties
        UniqueType UniqueType => UniqueType.Inventory;
        InventoryType InventoryType { get; }
        int SlotCount { get; }
        #endregion Properties
        
        #region GenericMethods
        IInventorySlotData[] GetAllSlotData();
        void InitializeData(int slotSize, So_InventoryData defaultOrNullItem);
        void UpdateSlotData(int index, So_InventoryData itemData, int count); // should handle both 'SetSlotData' and 'UpdateSlotData' logic in IInventorySlotData
        void ClearInventory();
        #endregion GenericMethods
        
#region functionalMethods
        
        #region InventoryInfoQueries
        /// <summary>
        /// How many items of given type are in the inventory grid.
        /// </summary>
        /// <param name="itemData">The scriptable object whose data we are trying to count</param>
        /// <returns>The total count of the passed scriptable object in this inventory</returns>
        int GetTotalItemCount(So_InventoryData itemData);
        
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
        /// <param name="itemData"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        bool CanStackItemInAnySlot(So_InventoryData itemData, int itemCount);
        
        /// <summary>
        /// Can we stack the given item in one or many of the 'used' slots in this inventory grid
        /// </summary>
        /// <param name="itemData">The item we want to stack</param>
        /// <param name="itemCount">The count of items to be added</param>
        /// <returns>is this stackable</returns>
        bool CanStackItemInExistingSlots(So_InventoryData itemData, int itemCount);
        
        /// <summary>
        /// Can we add the given item in 'used as well as empty slots' in this inventory grid
        /// </summary>
        /// <param name="itemData">The item we want to stack</param>
        /// <param name="itemCount">The count of items to be added</param>
        /// <returns></returns>
        bool CanAddItemsWithinAvailableSlots(So_InventoryData itemData, int itemCount);
        #endregion InventoryActionQueries
        
        #region InventoryActions
        /// <summary>
        /// Adds the given item to the inventory grid data structure.
        /// </summary>
        /// <param name="itemData">The item to add</param>
        /// <exception cref="ArgumentException">Thrown when no available slot is found in the inventory of the specified type with extra space as well as no empty slot is found</exception>
        void AddItem(So_InventoryData itemData);
        
        /// <summary>
        /// Adds the given quantity of items to the inventory grid data structure.
        /// </summary>
        /// <param name="itemData">The item to add</param>
        /// <param name="quantity">The quantity / count of the items to be added</param>
        /// <exception cref="ArgumentException">Thrown when the item in the given quantity cannot be added to the grid</exception>
        void AddItems(So_InventoryData itemData, int quantity);
        
        
        #endregion InventoryActions
        
#endregion functionalMethods
    }
}