using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    public class MagicPlayerInventoryService : InventoryPlayerService<So_InventoryMagicData, MagicSlotData> 
    {
        public override UniqueType InventoryType => UniqueType.Magic;
    }
}