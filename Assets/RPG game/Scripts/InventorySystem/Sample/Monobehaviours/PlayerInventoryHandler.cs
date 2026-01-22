using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.ServiceLocator;
using UnityEngine;
using UnityEngine.Serialization;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    public class PlayerInventoryHandler : MonoBehaviour
    {
        [FormerlySerializedAs("playerItemInventoryUI")] [SerializeField] private PlayerInventoryItemUI playerInventoryItemUI;
        [SerializeField] private So_InventoryItemData emptyItemSlotPrefab;
        private IInventoryService<So_InventoryItemData, ItemSlotData> playerItemInventoryService;
        
        [FormerlySerializedAs("playerMagicInventoryUI")] [SerializeField] private PlayerInventoryMagicUI playerInventoryMagicUI;
        [SerializeField] private So_InventoryMagicData emptyMagicSlotPrefab;
        private IInventoryService<So_InventoryMagicData, MagicSlotData> playerMagicInventoryService;
        
        [FormerlySerializedAs("playerSpellInventoryUI")] [SerializeField] private PlayerInventorySpellUI playerInventorySpellUI;
        [SerializeField] private So_InventorySpellData emptySpellSlotPrefab;
        private IInventoryService<So_InventorySpellData, SpellSlotData> playerSpellInventoryService;
        
        private void Awake()
        {
            // TODO: load from saved data when we have resume functionality, for now just create new empty inventories
            
            // create item inventory
            playerItemInventoryService = playerInventoryItemUI.Setup(emptyItemSlotPrefab);
            playerMagicInventoryService = playerInventoryMagicUI.Setup(emptyMagicSlotPrefab);
            playerSpellInventoryService = playerInventorySpellUI.Setup(emptySpellSlotPrefab);
            
            // register to listen for inventory changes
            playerItemInventoryService.OnInventoryChanged += playerInventoryItemUI.UpdateUI;
            playerMagicInventoryService.OnInventoryChanged += playerInventoryMagicUI.UpdateUI;
            playerSpellInventoryService.OnInventoryChanged += playerInventorySpellUI.UpdateUI; 
            
            // register in service locator for global access
            SLocator.GetSlGlobal.Register(playerItemInventoryService); // SLocator.GetSlGlobal.Get(out IInventoryService<So_InventoryItemData, ItemSlotData> inventoryService);
            SLocator.GetSlGlobal.Register(playerMagicInventoryService);
            SLocator.GetSlGlobal.Register(playerSpellInventoryService);
        }

        private void OnDestroy()
        {
            // unregister from listening to inventory changes
            playerItemInventoryService.OnInventoryChanged -= playerInventoryItemUI.UpdateUI;
            playerMagicInventoryService.OnInventoryChanged -= playerInventoryMagicUI.UpdateUI;
            playerSpellInventoryService.OnInventoryChanged -= playerInventorySpellUI.UpdateUI;
            
            // unregister from service locator
            SLocator.GetSlGlobal.UnRegister(playerItemInventoryService);
            SLocator.GetSlGlobal.UnRegister(playerMagicInventoryService);
            SLocator.GetSlGlobal.UnRegister(playerSpellInventoryService);
        }
    }
}
