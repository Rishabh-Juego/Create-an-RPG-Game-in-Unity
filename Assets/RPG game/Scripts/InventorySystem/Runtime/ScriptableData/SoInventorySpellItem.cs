using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Samples;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Data
{
    /// <summary>
    /// Main base class for all inventory magic data.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(SoInventorySpellItem), menuName = "Scriptable Objects/Inventory/" + nameof(SoInventorySpellItem))]
    public class SoInventorySpellItem : So_InventoryItem
    {
        public override InventoryType InventorySpecificType => InventoryType.Spells;
        // inventoryType
        private void OnValidate()
        {
            ItemRegistry.CheckIsDuplicate(this);
            if (gameObjectType != GameObjectType.Inventory)
            {
                Debug.Log($"Any ScriptableObject that inherits from {nameof(So_InventoryItem)} must have its {nameof(ObjectType)} set to {nameof(GameObjectType.Inventory)}. fix the issue on {name}.", this);
                gameObjectType = GameObjectType.Inventory;
            }

            if (inventorySpecificType != InventoryType.Spells)
            {
                Debug.Log($"Any ScriptableObject that inherits from {nameof(SoInventorySpellItem)} must have its {nameof(InventoryType)} set to {nameof(InventoryType.Spells)}. fix the issue on {name}.", this);
                inventorySpecificType = InventoryType.Spells;
            }
        }
    }
}