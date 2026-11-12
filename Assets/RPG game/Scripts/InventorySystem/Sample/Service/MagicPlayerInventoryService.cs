using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    public class MagicPlayerInventoryService : InventoryPlayerService // <SoInventoryMagicItem, MagicSlotUiInfo> 
    {
        public InventoryType InventorySpecificType => InventoryType.Magic;
    }
}