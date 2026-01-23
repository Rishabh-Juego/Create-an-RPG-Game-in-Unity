using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Samples;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Data
{
    /// <summary>
    /// Main base class for all inventory items data.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(So_InventoryItemData), menuName = "Scriptable Objects/Inventory/" + nameof(So_InventoryItemData))]
    public class So_InventoryItemData : So_InventoryData
    {
        public override InventoryType InventoryType => InventoryType.Items;
        // inventoryType
        private void OnValidate()
        {
            ItemRegistry.CheckIsDuplicate(this);
            if (uniqueType != UniqueType.Inventory)
            {
                Debug.Log($"Any ScriptableObject that inherits from {nameof(So_InventoryData)} must have its {nameof(UniqueType)} set to {nameof(UniqueType.Inventory)}. fix the issue on {name}.", this);
                uniqueType = UniqueType.Inventory;
            }

            if (inventoryType != InventoryType.Items)
            {
                Debug.Log($"Any ScriptableObject that inherits from {nameof(So_InventoryItemData)} must have its {nameof(InventoryType)} set to {nameof(InventoryType.Items)}. fix the issue on {name}.", this);
                inventoryType = InventoryType.Items;
            }
        }
    }
}