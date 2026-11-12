using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.RPG.Items.InventorySystem.Samples;
using TGL.ServiceLocator;
using UnityEngine;

namespace TGL.RPG.CommandPattern.Samples
{
    public class AddItemToInventoryCmd : ICommand<AddItemToInventoryDto>
    {
        public AddItemToInventoryDto Data { get; }
        private InventoryResult _result;
        
        public static InventoryResult CreateAndRun(So_InventoryItem inventoryItemSo, int addCount)
        {
            AddItemToInventoryCmd command = new AddItemToInventoryCmd(inventoryItemSo, addCount);
            return command.Execute() as InventoryResult;
        }
        
        private AddItemToInventoryCmd(So_InventoryItem inventoryItemSo, int addCount)
        {
            Data = new AddItemToInventoryDto
            {
                InventoryType = inventoryItemSo.InventoryType,
                InventoryItem = inventoryItemSo,
                ItemCount = addCount
            };
        }
        
        public CommandResult Execute()
        {
            bool itemPickedUp = false;
            string returnMsg = string.Empty;
            
            switch (Data.InventoryType)
            {
                case InventoryType.Objects:
                    if (SLocator.GetSlGlobal.TryGet(out IInventoryService<SoInventoryObjectItem, ObjectSlotUiInfo> playerItemInventoryService))
                    {
                        if (Data.InventoryItem is SoInventoryObjectItem itemData)
                        {
                            if (playerItemInventoryService.TryAddItems(itemData, Data.ItemCount))
                            {
                                itemPickedUp = true;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"No inventory service found for items when picking up {Data.InventoryItem.DisplayName} of type {Data.InventoryType}.", Data.InventoryItem.objectPrefab);
                    }
                    break;
                case InventoryType.Magic:
                    if (SLocator.GetSlGlobal.TryGet(out IInventoryService<SoInventoryMagicItem, MagicSlotUiInfo> playerMagicInventoryService))
                    {
                        if (Data.InventoryItem is SoInventoryMagicItem magicData)
                        {
                            if (playerMagicInventoryService.TryAddItems(magicData, Data.ItemCount))
                            {
                                itemPickedUp = true;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"No inventory service found for items when picking up {Data.InventoryItem.DisplayName} of type {Data.InventoryType}.", Data.InventoryItem.objectPrefab);
                    }
                    break;
                case InventoryType.Spells:
                    if (SLocator.GetSlGlobal.TryGet(out IInventoryService<SoInventorySpellItem, SpellSlotUiInfo> playerSpellInventoryService))
                    {
                        if (Data.InventoryItem is SoInventorySpellItem spellData)
                        {
                            if (playerSpellInventoryService.TryAddItems(spellData, Data.ItemCount))
                            {
                                itemPickedUp = true;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"No inventory service found for items when picking up {Data.InventoryItem.DisplayName} of type {Data.InventoryType}.", Data.InventoryItem.objectPrefab);
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