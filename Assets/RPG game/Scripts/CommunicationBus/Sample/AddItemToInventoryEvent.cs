using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;

namespace TGL.RPG.CommunicationBus.Sample
{
    public class AddItemToInventoryEvent : IMessageBody
    {
        public readonly InventoryItemData item;
        public AddItemToInventoryEvent(InventoryItemData item)
        {
            this.item = item;
        }
    }
}
