using UnityEngine;

namespace TGL.RPG.Items.PickingSystem
{
    public interface IPickable
    {
        void PickUp(IPicker interactor);
        string GetItemName();
    }
}
