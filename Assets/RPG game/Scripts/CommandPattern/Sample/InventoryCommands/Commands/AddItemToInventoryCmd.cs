using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.ServiceLocator;
using UnityEngine;

namespace TGL.RPG.CommandPattern.Samples
{
    public class AddItemToInventoryCmd : ICommand<AddItemToInventoryDto>
    {
        public AddItemToInventoryDto Data { get; }
        private InventoryResult _result;
        
        public static InventoryResult CreateAndRun(So_InventoryData inventoryDataSO, int addCount)
        {
            AddItemToInventoryCmd command = new AddItemToInventoryCmd(inventoryDataSO, addCount);
            return command.Execute() as InventoryResult;
        }
        
        private AddItemToInventoryCmd(So_InventoryData inventoryDataSO, int addCount)
        {
            Data = new AddItemToInventoryDto
            {
                InventoryType = inventoryDataSO.UniqueType,
                inventoryData = inventoryDataSO,
                ItemCount = addCount
            };
        }
        
        public CommandResult Execute()
        {
            bool itemPickedUp = false;
            string returnMsg = string.Empty;
            
            switch (Data.InventoryType)
            {
                case UniqueType.Item:
                    if (SLocator.GetSlGlobal.TryGet(out IInventoryService<So_InventoryItemData, ItemSlotData> playerItemInventoryService))
                    {
                        if (Data.inventoryData is So_InventoryItemData itemData)
                        {
                            if (playerItemInventoryService.TryAddItems(itemData, Data.ItemCount))
                            {
                                itemPickedUp = true;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"No inventory service found for items when picking up {Data.inventoryData.DisplayName} of type {Data.InventoryType}.", Data.inventoryData.objectPrefab);
                    }
                    break;
                case UniqueType.Magic:
                    if (SLocator.GetSlGlobal.TryGet(out IInventoryService<So_InventoryMagicData, MagicSlotData> playerMagicInventoryService))
                    {
                        if (Data.inventoryData is So_InventoryMagicData magicData)
                        {
                            if (playerMagicInventoryService.TryAddItems(magicData, Data.ItemCount))
                            {
                                itemPickedUp = true;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"No inventory service found for items when picking up {Data.inventoryData.DisplayName} of type {Data.InventoryType}.", Data.inventoryData.objectPrefab);
                    }
                    break;
                case UniqueType.Spell:
                    if (SLocator.GetSlGlobal.TryGet(out IInventoryService<So_InventorySpellData, SpellSlotData> playerSpellInventoryService))
                    {
                        if (Data.inventoryData is So_InventorySpellData spellData)
                        {
                            if (playerSpellInventoryService.TryAddItems(spellData, Data.ItemCount))
                            {
                                itemPickedUp = true;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"No inventory service found for items when picking up {Data.inventoryData.DisplayName} of type {Data.InventoryType}.", Data.inventoryData.objectPrefab);
                    }
                    break;
            }
            _result = new InventoryResult(itemPickedUp, returnMsg);
            return _result;
        }

        public bool Undo()
        {
            throw new System.NotImplementedException();
        }
    }
}