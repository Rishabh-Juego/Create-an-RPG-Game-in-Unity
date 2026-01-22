using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    public class SpellPlayerInventoryService : InventoryPlayerService<So_InventorySpellData, SpellSlotData> 
    {
        public override UniqueType InventoryType => UniqueType.Spell;
    }
}