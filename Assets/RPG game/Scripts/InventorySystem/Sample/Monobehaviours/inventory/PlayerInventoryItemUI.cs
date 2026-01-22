using System;
using System.Collections.Generic;
using System.Linq;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.ServiceLocator;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    /// <summary>
    /// Small SubSection of main <see cref="PlayerInventoryHandler"/> to show inventory data for the player within <see cref="UniqueType.Item"/> type.
    /// </summary>
    public class PlayerInventoryItemUI : PlayerInventoryUI<ItemPlayerInventoryService, So_InventoryItemData,  ItemSlotData, PlayerSlotItemUI>
    {
        [SerializeField] private List<PlayerSlotItemUI> allItemSlotsUI;
        protected override List<PlayerSlotItemUI> AllSlotsUI => allItemSlotsUI;

        public override ItemPlayerInventoryService Setup(So_InventoryItemData defaultOrNullItem)
        {
            _inventoryService = new ItemPlayerInventoryService();
            Initialize(UniqueType.Item, defaultOrNullItem);
            return _inventoryService;
        }
    }
}
