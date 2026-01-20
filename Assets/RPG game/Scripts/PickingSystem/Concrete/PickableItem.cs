using System;
using UnityEngine;
using TGL.RPG.Items.PickingSystem;

namespace TGL.RPG.Items.PickingSystem.Samples
{
    public class PickableItem : MonoBehaviour, IPickableItem
    {
        public void PickUp(IPicker interactor)
        {
            throw new System.NotImplementedException();
        }

        public string GetItemName()
        {
            throw new System.NotImplementedException();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPicker picker))
            {
                Destroy(gameObject);
            }
        }
    }
}
