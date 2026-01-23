using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Samples;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Data
{
    /// <summary>
    /// Main base class for all inventory magic data.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(So_InventoryMagicData), menuName = "Scriptable Objects/Inventory/" + nameof(So_InventoryMagicData))]
    public class So_InventoryMagicData : So_InventoryData
    {
        public override InventoryType InventoryType => InventoryType.Magic;
        // inventoryType
        private void OnValidate()
        {
            ItemRegistry.CheckIsDuplicate(this);
            if (uniqueType != UniqueType.Inventory)
            {
                Debug.Log($"Any ScriptableObject that inherits from {nameof(So_InventoryData)} must have its {nameof(UniqueType)} set to {nameof(UniqueType.Inventory)}. fix the issue on {name}.", this);
                uniqueType = UniqueType.Inventory;
            }

            if (inventoryType != InventoryType.Magic)
            {
                Debug.Log($"Any ScriptableObject that inherits from {nameof(So_InventoryItemData)} must have its {nameof(InventoryType)} set to {nameof(InventoryType.Magic)}. fix the issue on {name}.", this);
                inventoryType = InventoryType.Magic;
            }
        }
    }
}