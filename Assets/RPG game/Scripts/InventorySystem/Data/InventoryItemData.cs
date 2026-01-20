using System;
using TGL.RPG.IdentityRegistry;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Data
{
    [CreateAssetMenu(fileName = nameof(InventoryItemData), menuName = "Scriptable Objects/Inventory/"+nameof(InventoryItemData))]
    public class InventoryItemData : UniqueScriptable
    {
        public string DisplayName;
        public Sprite itemIcon;
        public GameObject itemPrefab;

        private void OnValidate()
        {
            ItemRegistry.CheckIsDuplicate(this);
        }
    }
}