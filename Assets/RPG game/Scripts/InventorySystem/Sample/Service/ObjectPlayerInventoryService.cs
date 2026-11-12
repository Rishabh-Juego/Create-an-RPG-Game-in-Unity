using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Samples
{
    public class ObjectPlayerInventoryService : InventoryPlayerService // <SoInventoryObjectItem, ObjectSlotUiInfo>
    {
        public InventoryType InventorySpecificType => InventoryType.Objects;
    }
}
