using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Samples;
using TGL.RPG.Utilities.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace TGL.RPG.Items.InventorySystem.Data
{
    /// <summary>
    /// Main base ScriptableObject class for all inventory data.
    /// </summary>
    public abstract class So_InventoryItem : So_UniqueScriptable
    {
        [Space(10), Header("Inventory Data"), Space(5)] public string DisplayName;
        [FormerlySerializedAs("itemIcon")] public Sprite objectIcon;
        [FormerlySerializedAs("itemPrefab")] public GameObject objectPrefab;
        [FormerlySerializedAs("maxStackCount")] public int maxItemStackCount = 1;
        [SerializeField] protected InventoryType inventorySpecificType = InventoryType.Undefined; 
        
        public override GameObjectType ObjectType => GameObjectType.Inventory;
        public virtual InventoryType InventorySpecificType => inventorySpecificType;

        private void OnValidate()
        {
            ItemRegistry.CheckIsDuplicate(this);
            if (gameObjectType != GameObjectType.Inventory)
            {
                Debug.Log($"Any ScriptableObject that inherits from {nameof(So_InventoryItem)} must have its {nameof(ObjectType)} set to {nameof(GameObjectType.Inventory)}. fix the issue on {name}.", this);
                gameObjectType = GameObjectType.Inventory;
            }
        }
    }
}