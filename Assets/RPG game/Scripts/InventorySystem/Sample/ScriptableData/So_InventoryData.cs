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
    public abstract class So_InventoryData : So_UniqueScriptable
    {
        [Space(10), Header("Inventory Data"), Space(5)] public string DisplayName;
        [FormerlySerializedAs("itemIcon")] public Sprite objectIcon;
        [FormerlySerializedAs("itemPrefab")] public GameObject objectPrefab;
        public int maxStackCount = 1;
        [SerializeField] protected InventoryType inventoryType = InventoryType.Undefined; 
        
        public override UniqueType UniqueType => UniqueType.Inventory;
        public virtual InventoryType InventoryType => InventoryType.Undefined;

        private void OnValidate()
        {
            ItemRegistry.CheckIsDuplicate(this);
            if (uniqueType != UniqueType.Inventory)
            {
                Debug.Log($"Any ScriptableObject that inherits from {nameof(So_InventoryData)} must have its {nameof(UniqueType)} set to {nameof(UniqueType.Inventory)}. fix the issue on {name}.", this);
                uniqueType = UniqueType.Inventory;
            }
        }
    }
}