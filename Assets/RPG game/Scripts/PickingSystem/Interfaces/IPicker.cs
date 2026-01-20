using UnityEngine;

namespace TGL.RPG.Items.PickingSystem
{
    public interface IPicker
    {
        // You can pass the item's data or the whole object
        void OnItemPickedUp(IPickable item);
        
        // Useful for checking if the interactor's hands/inventory are full
        bool CanPickUp { get; }
        
        // Helps the item know where to "fly" toward or attach to
        Transform InteractionTransform { get; }
    }
}
