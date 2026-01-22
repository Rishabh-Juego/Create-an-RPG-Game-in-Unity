using System;
using TGL.RPG.IdentityRegistry;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Data
{
    /// <summary>
    /// Main base class for all inventory items data.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(So_InventoryItemData), menuName = "Scriptable Objects/Inventory/"+nameof(So_InventoryItemData))]
    public class So_InventoryItemData : So_InventoryData { }
}