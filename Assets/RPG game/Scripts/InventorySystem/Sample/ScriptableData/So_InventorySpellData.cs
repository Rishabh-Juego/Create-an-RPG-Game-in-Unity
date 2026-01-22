using System;
using TGL.RPG.IdentityRegistry;
using UnityEngine;

namespace TGL.RPG.Items.InventorySystem.Data
{
    /// <summary>
    /// Main base class for all inventory magic data.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(So_InventorySpellData), menuName = "Scriptable Objects/Inventory/"+nameof(So_InventorySpellData))]
    public class So_InventorySpellData : So_InventoryData { }
}