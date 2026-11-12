using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.RPG.Items.InventorySystem.Samples;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem
{
    public interface IInventorySlotInfo
    {
        // From ScriptableObject
        string ItemId { get; }
        public GameObjectType GameObjectType => GameObjectType.Inventory;
        public InventoryType InventoryType { get; }
        public So_InventoryItem EmptyItemSo { get; }
        public So_InventoryItem CurrItemSo { get; }
        string DisplayName { get; }
        int MaxSlotStackCount { get; }
        bool HasData { get; }
        bool IsStackable { get; }
        bool IsEmpty { get; }
        bool IsFull { get; }
        
        // From Inventory Slot Data
        int CurrentStackCount { get; }
        int SlotIndex { get; }
        
        // Events
        event Action<IInventorySlotInfo, int> OnSlotDataChanged;
        
        // generic methods
        void Initialize(int index, So_InventoryItem defaultScriptableItem);
        bool SetSlotData(So_InventoryItem itemSo, int count);
        bool UpdateSlotData(So_InventoryItem itemSo, int count);
        void ClearSlot();
        
        // functional methods
        bool HasItem(So_InventoryItem itemSo);
    }
}