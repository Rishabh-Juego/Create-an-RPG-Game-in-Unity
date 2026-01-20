using System;
using System.Collections.Generic;
using TGL.RPG.CommunicationBus;
using TGL.RPG.CommunicationBus.Sample;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.Items.PickingSystem
{
    public class ItemPicker : MonoBehaviour, IPicker
    {
        #region Variables
        
        private bool canPick = true;
        List<IPickable> pickableItems = new List<IPickable>();
        
        #endregion Variables

        
        #region Properties
        
        public bool CanPickUp => canPick;
        public Transform InteractionTransform => transform;
        
        #endregion Properties
        

        #region Interface_Methods
        
        public void AddPickableToPicker(IPickable item)
        {
            if (!pickableItems.Contains(item))
            {
                pickableItems.Add(item);
            }
        }

        public void RemovePickableFromPicker(IPickable item)
        {
            if (pickableItems.Contains(item))
            {
                pickableItems.Remove(item);
            }
        }

        public bool HasItem(IPickable item) => pickableItems.Contains(item);

        #endregion Interface_Methods
        

        #region MonoBehaviour_Methods

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P)) // TODO: Replace with input system
            {
                if (pickableItems.Count > 0)
                {
                    // TODO: can replace this with picking all items in range if needed
                    // find and pick up the closest item
                    IPickable closestItem = FindClosestItem();
                    PickItem(closestItem);
                }
            }
        }

        #endregion MonoBehaviour_Methods
        

        #region Private_Methods

        private IPickable FindClosestItem()
        {
            float closestDistance = float.MaxValue;
            float itemDistance;
            IPickable closestItem = null;
            foreach (IPickable pickableItem in pickableItems)
            {
                itemDistance = Vector3.Distance(transform.position, pickableItem.GetObjectPosition());
                if (itemDistance < closestDistance)
                {
                    closestDistance = itemDistance;
                    closestItem = pickableItem;
                }
            }

            return closestItem;
        }

        private void PickItem(IPickable item)
        {
            pickableItems.Remove(item);
            if(item.GetObjectData() is not InventoryItemData inventoryItemData)
            {
                Debug.LogError($"Cannot pick item as it is not of {nameof(inventoryItemData)} type.", item.GetObject());
                return;
            }
            MessageBus.PublishMessage(MessageTypes.AddItemToInventory, new AddItemToInventoryEvent(inventoryItemData));
            item.PickUp(this);
        }

        #endregion Private_Methods
    }
}
