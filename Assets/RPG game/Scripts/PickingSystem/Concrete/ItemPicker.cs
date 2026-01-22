using System;
using System.Collections.Generic;
using TGL.RPG.CommunicationBus;
using TGL.RPG.CommunicationBus.Sample;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.ServiceLocator;
using UnityEngine;
using UnityEngine.InputSystem;

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
            if (pickableItems.Count > 0)
            {
                if (Keyboard.current.pKey.wasPressedThisFrame) // P key to pick up closest item
                {
                    // find and pick up the closest item
                    IPickable closestItem = FindClosestItem();
                    PickItem(closestItem);
                }
                else if (Keyboard.current.oKey.wasPressedThisFrame) // O key to pick up all items in range
                {
                    IPickable[] allRangedItems = pickableItems.ToArray();
                    Array.ForEach(allRangedItems, PickItem); // 'PickItem' updates 'pickableItems', so we cannot use `pickableItems.ForEach(PickItem)`
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
            if(item.GetObjectData() is not So_InventoryData inventoryData)
            {
                Debug.LogError($"Cannot pick item as it is not of {nameof(So_InventoryData)} type.", item.GetObject());
                return;
            }
            // MessageBus.PublishMessage(MessageTypes.AddItemToInventory, new AddItemToInventoryEvent(inventoryItemData));

            bool itemPickedUp = false;
            switch (inventoryData.UniqueType)
            {
                case UniqueType.Item:
                    if (SLocator.GetSlGlobal.TryGet(out IInventoryService<So_InventoryItemData, ItemSlotData> playerItemInventoryService))
                    {
                        if (inventoryData is So_InventoryItemData itemData)
                        {
                            if (playerItemInventoryService.TryAddItem(itemData))
                            {
                                item.PickUp(this);
                                itemPickedUp = true;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"No inventory service found for items when picking up {item.GetItemName()} of type {inventoryData.UniqueType}.", item.GetObject());
                    }
                    break;
                case UniqueType.Magic:
                    if (SLocator.GetSlGlobal.TryGet(out IInventoryService<So_InventoryMagicData, MagicSlotData> playerMagicInventoryService))
                    {
                        if (inventoryData is So_InventoryMagicData magicData)
                        {
                            if (playerMagicInventoryService.TryAddItem(magicData))
                            {
                                item.PickUp(this);
                                itemPickedUp = true;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"No inventory service found for items when picking up {item.GetItemName()} of type {inventoryData.UniqueType}.", item.GetObject());
                    }
                    break;
                case UniqueType.Spell:
                    if (SLocator.GetSlGlobal.TryGet(out IInventoryService<So_InventorySpellData, SpellSlotData> playerSpellInventoryService))
                    {
                        if (inventoryData is So_InventorySpellData spellData)
                        {
                            if (playerSpellInventoryService.TryAddItem(spellData))
                            {
                                item.PickUp(this);
                                itemPickedUp = true;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"No inventory service found for items when picking up {item.GetItemName()} of type {inventoryData.UniqueType}.", item.GetObject());
                    }
                    break;
            }

            if (!itemPickedUp)
            {
                Debug.LogWarning($"Failed to pick item {item.GetItemName()} to inventory.", item.GetObject()); 
            }
            else
            {
                pickableItems.Remove(item);
            }
        }
        
        #endregion Private_Methods
    }
}
