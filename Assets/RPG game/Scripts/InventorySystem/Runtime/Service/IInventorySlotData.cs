using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.RPG.Items.InventorySystem.Samples;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem
{
    public interface IInventorySlotData
    {
        // From ScriptableObject
        string ItemId { get; }
        public UniqueType UniqueType => UniqueType.Inventory;
        public InventoryType InventoryType { get; }
        public So_InventoryData EmptyItemSo { get; }
        string DisplayName { get; }
        int MaxStackCount { get; }
        bool HasData { get; }
        bool IsStackable { get; }
        bool IsEmpty { get; }
        bool IsFull { get; }
        
        // From Inventory Slot Data
        int CurrentStackCount { get; }
        int SlotIndex { get; }
        
        // Events
        event Action<IInventorySlotData, int> OnSlotDataChanged;
        
        // generic methods
        void Initialize(int index, So_InventoryData defaultScriptableItem);
        bool SetSlotData(So_InventoryData itemData, int count);
        bool UpdateSlotData(So_InventoryData itemData, int count);
        void ClearSlot();
        
        // functional methods
        bool HasItem(So_InventoryData itemData);
    }
}