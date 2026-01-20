using UnityEngine;

namespace TGL.RPG.Items.PickingSystem
{
    public interface IPicker
    {
        void AddPickableToPicker(IPickable item);
        void RemovePickableFromPicker(IPickable item);
        bool HasItem(IPickable item);
        
        // Useful for checking if the interactor's hands/inventory are full
        bool CanPickUp { get; }
        
        // Helps the item know where to "fly" toward or attach to
        Transform InteractionTransform { get; }
    }
}
