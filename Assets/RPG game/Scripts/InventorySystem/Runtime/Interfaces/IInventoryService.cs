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
        IInventoryGridUiInfo InventoryInfo { get; }
        /// <summary>
        /// The item we show in inventory to represent "no item" or "default item".
        /// </summary>
        So_InventoryItem DefaultOrNullItemSo { get; }
        /// <summary>
        /// All SOs for items that are stored in this inventory
        /// </summary>
        So_InventoryItem[] AllInventoryItemsSo { get; }
        /// <summary>
        /// Dictionary of all inventory items by their ID for quick lookup for the related SO
        /// </summary>
        Dictionary<string, So_InventoryItem> AllInventoryItemsDict { get; } // key: id, value: item so for given id
        /// <summary>
        /// Dictionary of all inventory items with their counts, for resource availability tests
        /// </summary>
        Dictionary<So_InventoryItem, int> AllInventoryItemsCountDict { get; } // key: item so, value: the total count of this SO, in one or more stacks
        /// <summary>
        /// Event to inform change in inventory data. As service manages it, data change is informed by the service
        /// </summary>
        public event Action<IInventoryGridUiInfo> OnInventoryDataChanged;
        
	    public InventoryType InventorySpecificType { get; } 
#endregion Properties

#region GenericMethods
        void Initialize(IInventoryGridUiInfo gridObj ,int slotSize, So_InventoryItem defaultOrNullItem);
        void DeInitialize();
        void ClearInventory();
        IInventorySlotInfo[] GetAllItemSlots();
#endregion GenericMethods

#region functionalMethods
        /// <summary>
        /// Can we add item to the inventory in any slot?  <br/>
        /// for actions like picking up from ground, where the inventory is not visible so any slot can be used
        /// </summary>
        /// <param name="item">The item we want to know can be added</param>
        /// <returns>bool stating item can be added</returns>
        bool CanAddItem(So_InventoryItem item);
        bool CanAddItems(So_InventoryItem item, int count);
        bool TryAddItem(So_InventoryItem item);
        bool TryAddItems(So_InventoryItem item, int count);
        bool TryRemoveItem(So_InventoryItem item);
        bool TryRemoveItems(So_InventoryItem item, int count); 
#endregion functionalMethods
    }
}