using System;
using System.Collections.Generic;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.ServiceLocator;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    /// <summary>
    /// Small SubSection of main <see cref="PlayerInventoryHandler"/> to show inventory data for the player within <see cref="UniqueType.Spell"/> type.
    /// </summary>
    public class PlayerInventorySpellUI : PlayerInventoryUI<SpellPlayerInventoryService, So_InventorySpellData, SpellSlotData, PlayerSlotSpellUI>  
    {
        [SerializeField] private List<PlayerSlotSpellUI> allItemSlotsUI;
        protected override List<PlayerSlotSpellUI> AllSlotsUI => allItemSlotsUI;
        
        public override SpellPlayerInventoryService Setup(So_InventorySpellData defaultOrNullItem)
        {
            _inventoryService = new SpellPlayerInventoryService();
            Initialize(UniqueType.Spell, defaultOrNullItem);
            return _inventoryService;
        }
    }
}