using TGL.RPG.IdentityRegistry;
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

        private void OnValidate()
        {
            ItemRegistry.CheckIsDuplicate(this);
        }
    }
}