using System;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Samples;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Data
{
    /// <summary>
    /// Main base class for all inventory items data.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(SoInventoryObjectItem), menuName = "Scriptable Objects/Inventory/" + nameof(SoInventoryObjectItem))]
    public class SoInventoryObjectItem : So_InventoryItem
    {
        public override InventoryType InventoryType => InventoryType.Objects;
        // inventoryType
        private void OnValidate()
        {
            ItemRegistry.CheckIsDuplicate(this);
            if (gameObjectType != GameObjectType.Inventory)
            {
                Debug.Log($"Any ScriptableObject that inherits from {nameof(So_InventoryItem)} must have its {nameof(ObjectType)} set to {nameof(GameObjectType.Inventory)}. fix the issue on {name}.", this);
                gameObjectType = GameObjectType.Inventory;
            }

            if (inventoryType != InventoryType.Objects)
            {
                Debug.Log($"Any ScriptableObject that inherits from {nameof(SoInventoryObjectItem)} must have its {nameof(InventoryType)} set to {nameof(InventoryType.Objects)}. fix the issue on {name}.", this);
                inventoryType = InventoryType.Objects;
            }
        }
    }
}