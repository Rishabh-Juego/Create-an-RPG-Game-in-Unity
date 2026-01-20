using UnityEngine;

namespace TGL.RPG.Items.PickingSystem
{
    public class ItemPicker : MonoBehaviour, IPicker
    {
        private bool canPick = true;
        public bool CanPickUp { get { return canPick; } }
        public Transform InteractionTransform { get { return transform; } }

        public void OnItemPickedUp(IPickable item)
        {
            throw new System.NotImplementedException();
        }

    }
}
