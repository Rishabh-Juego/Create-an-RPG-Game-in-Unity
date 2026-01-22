using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.ServiceLocator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    // PlayerSlotMagicUI : PlayerSlotUI<MagicSlotData, So_InventoryMagicData>
    public abstract class PlayerSlotUI<T, U> : MonoBehaviour where T : InventorySlotData<U> where U : So_InventoryData
    {
        [SerializeField] protected Image slotIcon;
        [SerializeField] protected TMP_Text slotName;
        [SerializeField] protected GameObject countGroup;
        [SerializeField] protected TMP_Text countText;
        
        protected IInventorySlotData<U> _slotData;
        private int itemCount;

        public void Initialize(T slotData)
        {
            _slotData = slotData;
            itemCount = 0;
            countText.text = itemCount.ToString("00");
            slotData.OnSlotChanged += UpdateReferences;
            UpdateUI();
        }
        
        public void DeInitialize()
        {
            if (_slotData != null)
            {
                _slotData.OnSlotChanged -= UpdateReferences;
            }
        }

        private void UpdateReferences(IInventorySlotData<U> dataRef, int newCount)
        {
            _slotData = dataRef;
            itemCount = dataRef.ItemCount;
            itemCount = newCount;
            UpdateUI();
        }

        public void UpdateUI()
        {
            slotIcon.sprite = _slotData.ContainedItemData.objectIcon;
            slotName.text = _slotData.ContainedItemData.DisplayName;
            
            countGroup.SetActive(itemCount > 1);
            countText.text = itemCount.ToString("00");
        }
    }
}

