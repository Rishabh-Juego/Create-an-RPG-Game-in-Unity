using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using TGL.RPG.Items.InventorySystem.Samples;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem
{
    [Serializable]
    public class MagicSlotUiInfo : InventorySlotUiInfo
    {
        public MagicSlotUiInfo()
        {
            inventorySpecificType = InventoryType.Magic;
        }
    }
}