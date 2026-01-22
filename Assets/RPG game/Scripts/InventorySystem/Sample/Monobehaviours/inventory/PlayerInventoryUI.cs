using System;
using System.Collections.Generic;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.ServiceLocator;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    public abstract class PlayerInventoryUI<T, SO_T, U, V> : MonoBehaviour where T : IInventoryService<SO_T, U> where SO_T : So_InventoryData where U : InventorySlotData<SO_T> where V : PlayerSlotUI<U, SO_T>
    {
        protected T _inventoryService;
        protected abstract List<V> AllSlotsUI { get; }
        protected SO_T _defaultOrNullItem;
        protected UniqueType _inventoryType;
        
        public abstract T Setup(SO_T defaultOrNullItem);
        
        protected void Initialize(UniqueType inventoryType, SO_T defaultOrNullItem)
        {
            _inventoryType = inventoryType;
            _defaultOrNullItem = defaultOrNullItem;
            
            _inventoryService.Initialize(AllSlotsUI.Count, defaultOrNullItem);
            U[] allItemsData = _inventoryService.GetAllItems();
            AssignSlotDataToUi(allItemsData);
        }
        
        public void UpdateUI()
        {
            foreach (V slotUI in AllSlotsUI)
            {
                slotUI.UpdateUI();
            }
        }

        protected virtual void AssignSlotDataToUi(U[] allItemsData)
        {
            if (allItemsData.Length != AllSlotsUI.Count)
            {
                Debug.LogError($"We have {allItemsData.Length} data items but {AllSlotsUI.Count} UI slots. Something went wrong!", gameObject);
                return;
            }
            
            // initialize each slot UI with corresponding data
            for (int i = 0; i < allItemsData.Length; i++)
            {
                U itemData = allItemsData[i];
                V slotUI = AllSlotsUI[i];
                slotUI.Initialize(itemData);
            }
        }
        
        protected virtual void UnassignSlotDataFromUi()
        {            
            // initialize each slot UI with corresponding data
            for (int i = 0; i < AllSlotsUI.Count; i++)
            {
                
                V slotUI = AllSlotsUI[i];
                slotUI.DeInitialize();
            }
        }

        private void OnDestroy()
        {
            UnassignSlotDataFromUi();
        }
    }
}
