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
    /// Small SubSection of main <see cref="PlayerInventoryHandler"/> to show inventory data for the player within <see cref="InventoryType.Objects"/> type.
    /// </summary>
    public class PlayerInventoryObjectUI : PlayerInventoryUI<ObjectPlayerInventoryService, SoInventoryObjectItem,  ObjectSlotUiInfo, PlayerSlotObjectUI>
    {
        [SerializeField] private List<PlayerSlotObjectUI> allItemSlotsUI;
        protected override List<PlayerSlotObjectUI> AllSlotsUI => allItemSlotsUI;

        public override ObjectPlayerInventoryService Setup(SoInventoryObjectItem defaultOrNullItem)
        {
            _inventoryService = new ObjectPlayerInventoryService();
            Initialize(InventoryType.Objects, defaultOrNullItem);
            return _inventoryService;
        }
    }
}
