using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    public class ItemPlayerInventoryService : InventoryPlayerService<So_InventoryItemData, ItemSlotData>
    {
        public override UniqueType InventoryType => UniqueType.Item;
    }
}
