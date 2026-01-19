using System;
using UnityEngine;
using UnityEngine.UI;

namespace TGL.RPG.UI.Inventory
{
    public class OpenCloseInventory : MonoBehaviour
    {
        /// <summary>
        /// has all inventory UI elements
        /// </summary>
        public GameObject inventoryUiGob;
        /// <summary>
        /// The button to close the inventory UI, if we can see it, means inventory is open
        /// </summary>
        public Button inventoryCloseButton;
        /// <summary>
        /// The button to open the inventory UI, if we can see it, means inventory is closed
        /// </summary>
        public Button inventoryOpenButton;

        private void Awake()
        {
            ValidateSerializedFields();
        }

        private void OnEnable()
        {
            inventoryOpenButton.onClick.AddListener(OpenInventory);
            inventoryCloseButton.onClick.AddListener(CloseInventory);
        }

        private void Start()
        {
            CloseInventory();
        }

        private void OnDisable()
        {
            inventoryOpenButton.onClick.RemoveListener(OpenInventory);
            inventoryCloseButton.onClick.RemoveListener(CloseInventory);
        }

        private void ValidateSerializedFields()
        {
            if (inventoryUiGob == null)
            {
                Debug.LogError($"The inventory UI GameObject is not assigned in the inspector.", gameObject);
            }

            if (inventoryCloseButton == null)
            {
                Debug.LogError($"The inventory close button is not assigned in the inspector.", gameObject);
            }

            if (inventoryOpenButton == null)
            {
                Debug.LogError($"The inventory open button is not assigned in the inspector.", gameObject);
            }
        }

        /// <summary>
        /// if needed to open the inventory UI, we can call this method from inspector button
        /// </summary>
        public void OpenInventory()
        {
            inventoryUiGob.SetActive(true);
            ShowInventoryAsOpen(true);
            Time.timeScale = 0f;
        }
        
        /// <summary>
        /// if needed to close the inventory UI, we can call this method from inspector button
        /// </summary>
        public void CloseInventory()
        {
            inventoryUiGob.SetActive(false);
            ShowInventoryAsOpen(false);
            Time.timeScale = 1f;
        }

        private void ShowInventoryAsOpen(bool isOpen)
        {
            inventoryCloseButton.gameObject.SetActive(isOpen);
            inventoryOpenButton.gameObject.SetActive(!isOpen);
        }
        
    }
}
