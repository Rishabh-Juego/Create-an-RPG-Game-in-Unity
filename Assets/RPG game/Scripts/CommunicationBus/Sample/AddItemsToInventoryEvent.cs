using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.CommunicationBus.Sample
{
    public class AddItemsToInventoryEvent : IMessageBody
    {
        public readonly InventoryItemData item;
        public readonly int quantity;
        public AddItemsToInventoryEvent(InventoryItemData item, int quantity)
        {
            this.item = item;
            this.quantity = quantity;
        }
    }
}