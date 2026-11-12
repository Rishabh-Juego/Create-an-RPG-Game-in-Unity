using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    public class SpellPlayerInventoryService : InventoryPlayerService // <SoInventorySpellItem, SpellSlotUiInfo> 
    {
        public InventoryType InventorySpecificType => InventoryType.Spells;
    }
}