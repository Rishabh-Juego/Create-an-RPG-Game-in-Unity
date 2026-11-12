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
    public abstract class PlayerSlotUI<T> : MonoBehaviour where T : IInventorySlotInfo
    {
        [SerializeField] protected Image slotIcon;
        [SerializeField] protected TMP_Text slotName;
        [SerializeField] protected GameObject countGroup;
        [SerializeField] protected TMP_Text countText;
        
        protected IInventorySlotInfo _uiSlotInfo;
        private int itemCount;

        public void Initialize(T slotData)
        {
            _uiSlotInfo = slotData;
            itemCount = 0;
            countText.text = itemCount.ToString("00");
            _uiSlotInfo.OnSlotDataChanged += UpdateReferences;
            UpdateUI(slotData);
        }
        
        public void DeInitialize()
        {
            if (_uiSlotInfo != null)
            {
                _uiSlotInfo.OnSlotDataChanged -= UpdateReferences;
            }
        }

        private void UpdateReferences(IInventorySlotInfo uiSlotRef, int newCount)
        {
            if (uiSlotRef is T concreteData)
            {
                _uiSlotInfo = concreteData;
                itemCount = uiSlotRef.CurrentStackCount;
                itemCount = newCount;
                UpdateUI(concreteData);
            }
            else
            {
                Debug.LogError($"Slot Changed update came with type {uiSlotRef.GetType().FullName}, but we were expecting type {typeof(T).FullName}", gameObject);
            }
        }

        public void UpdateUI(T concreteData)
        {
            slotIcon.sprite = concreteData.CurrItemSo.objectIcon;
            slotName.text = concreteData.DisplayName;
            
            countGroup.SetActive(itemCount > 1);
            countText.text = itemCount.ToString("00");
        }
    }
}

