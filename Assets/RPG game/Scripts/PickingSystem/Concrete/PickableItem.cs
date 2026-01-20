using System;
using System.Collections;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;
using TGL.RPG.Items.PickingSystem;

namespace TGL.RPG.Items.PickingSystem.Samples
{
    public class PickableItem : MonoBehaviour, IPickableItem
    {
        #region Variables
        [SerializeField, Range(0.1f, 0.9f)] private float moveStep = 0.5f;
        
        [SerializeField, Range(0.1f, 3f)] private float minCloseness = 0.2f;
        
        [SerializeField] private InventoryItemData pickableData;
        
        #endregion Variables

        
        #region Interface_Methods
        
        public string GetItemName() => pickableData.DisplayName;
        
        public Vector3 GetObjectPosition() => transform.position;
        
        public GameObject GetObject() => gameObject;
        
        public UniqueScriptable GetObjectData() => pickableData;
        
        public void PickUp(IPicker interactor)
        {
            Transform target = interactor.InteractionTransform;
            StartCoroutine(MoveTowardsPicker(target));
        }
        
        #endregion Interface_Methods

        
        #region MonoBehaviour_Methods
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPicker picker))
            {
                if (picker.CanPickUp)
                {
                    picker.AddPickableToPicker(this);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IPicker picker))
            {
                if (picker.HasItem(this))
                {
                    picker.RemovePickableFromPicker(this);
                }
            }
        }
        
        #endregion MonoBehaviour_Methods

        
        #region Private_Methods

        IEnumerator MoveTowardsPicker(Transform target)
        {
            while (Vector3.Distance(transform.position, target.position) > minCloseness)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, moveStep);
                yield return null;
            }
            
            Destroy(gameObject);
        }

        #endregion Private_Methods
    }
}
