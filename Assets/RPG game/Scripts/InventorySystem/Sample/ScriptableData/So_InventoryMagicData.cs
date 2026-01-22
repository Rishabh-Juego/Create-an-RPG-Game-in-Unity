using System;
using TGL.RPG.IdentityRegistry;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Data
{
    /// <summary>
    /// Main base class for all inventory magic data.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(So_InventoryMagicData), menuName = "Scriptable Objects/Inventory/"+nameof(So_InventoryMagicData))]
    public class So_InventoryMagicData : So_InventoryData { }
}