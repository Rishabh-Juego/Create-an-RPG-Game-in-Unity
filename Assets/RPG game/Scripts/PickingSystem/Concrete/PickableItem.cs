using System;
using System.Collections;
using TGL.RPG.IdentityRegistry;
using TGL.RPG.Items.InventorySystem.Data;
using UnityEngine;
using TGL.RPG.Items.PickingSystem;

namespace TGL.RPG.Items.PickingSystem.Samples
{
    public class PickableItem : MonoBehaviour, IPickableItem
    {
        #region Variables
        [Header("Picking Animation Variables")]
        [SerializeField, Tooltip("The amount of distance it jumps in a single frame"), Range(0.1f, 0.9f)] private float moveStep = 0.5f;
        [SerializeField, Tooltip("The minimum distance this object gets to the target before disappearing"), Range(0.1f, 3f)] private float minCloseness = 0.2f;
        [SerializeField, Tooltip("The scriptable Object that has all info about this model")] private So_InventoryData pickableData;
        private Coroutine _moveTowardsPicker;
        #endregion Variables

        
        #region Interface_Methods
        
        public string GetItemName() => pickableData.DisplayName;
        
        public Vector3 GetObjectPosition() => transform.position;
        
        public GameObject GetObject() => gameObject;
        
        public So_UniqueScriptable GetObjectData() => pickableData;
        
        public void PickUp(IPicker interactor)
        {
            Transform target = interactor.InteractionTransform;
            _moveTowardsPicker = StartCoroutine(MoveTowardsPicker(target));
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
