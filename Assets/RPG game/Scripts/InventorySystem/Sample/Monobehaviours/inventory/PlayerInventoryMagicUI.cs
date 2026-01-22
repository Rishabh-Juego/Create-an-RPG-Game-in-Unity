using System;
using System.Collections.Generic;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.ServiceLocator;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    /// <summary>
    /// Small SubSection of main <see cref="PlayerInventoryHandler"/> to show inventory data for the player within <see cref="UniqueType.Magic"/> type.
    /// </summary>
    public class PlayerInventoryMagicUI : PlayerInventoryUI<MagicPlayerInventoryService , So_InventoryMagicData, MagicSlotData, PlayerSlotMagicUI>
    {
        [SerializeField] private List<PlayerSlotMagicUI> allItemSlotsUI;
        protected override List<PlayerSlotMagicUI> AllSlotsUI => allItemSlotsUI;
        
        public override MagicPlayerInventoryService Setup(So_InventoryMagicData defaultOrNullItem)
        {
            _inventoryService = new MagicPlayerInventoryService();
            Initialize(UniqueType.Magic, defaultOrNullItem);
            return _inventoryService;
        }
    }
}