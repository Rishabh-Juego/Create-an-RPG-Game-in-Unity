using System;
using System.Collections.Generic;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.RPG.Items.InventorySystem.Samples;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem
{
    public interface IInventoryService
    {
        #region Properties
        /// <summary>
        /// The type of inventory items this service handles.
        /// </summary>
        IInventoryGridData InventoryData { get; }
        /// <summary>
        /// The item we show in inventory to represent "no item" or "default item".
        /// </summary>
        So_InventoryData DefaultOrNullItemSo { get; }
        /// <summary>
        /// All SOs for items that are stored in this inventory
        /// </summary>
        So_InventoryData[] AllInventoryItemsSo { get; }
        /// <summary>
        /// Dictionary of all inventory items by their ID for quick lookup for the related SO
        /// </summary>
        Dictionary<string, So_InventoryData> AllInventoryItemsDict { get; } // key: id, value: item so for given id
        /// <summary>
        /// Event to inform change in inventory data
        /// </summary>
        public event Action<IInventoryGridData> OnInventoryDataChanged;
        #endregion Properties

        #region GenericMethods
        void Initialize(IInventoryGridData gridObj ,int slotSize, So_InventoryData defaultOrNullItem);
        void ClearInventory();
        IInventorySlotData[] GetAllItemData();
        #endregion GenericMethods

        #region functionalMethods
        /// <summary>
        /// Can we add item to the inventory in any slot?  <br/>
        /// for actions like picking up from ground, where the inventory is not visible so any slot can be used
        /// </summary>
        /// <param name="item">The item we want to know can be added</param>
        /// <returns>bool stating item can be added</returns>
        bool CanAddItem(So_InventoryData item);
        bool CanAddItems(So_InventoryData item, int count);
        bool TryAddItem(So_InventoryData item);
        bool TryAddItems(So_InventoryData item, int count);
        bool TryRemoveItem(So_InventoryData item);
        bool TryRemoveItems(So_InventoryData item, int count); 
        #endregion functionalMethods
    }
}