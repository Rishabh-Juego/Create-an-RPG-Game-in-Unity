using Unity.Cinemachine;
using UnityEngine;

namespace TGL.RPG.CameraManagement
{
    public class GameCamera : MonoBehaviour, IActiveCameraProvider
    {
        [SerializeField] private Camera gameCamera;
        [SerializeField] private CinemachineCamera camSettings;
        
        public Camera ActiveCamera => gameCamera;
        public CinemachineCamera CameraSettings => camSettings;

        private void Awake() 
        {
            if (gameCamera != null && camSettings != null)
            {
                CameraRegistry.Register(this);
            }
            else
            {
                Debug.LogError($"Could not register GameCamera or Cinemachine on {gameObject.name} due to missing references.", this);
            }
        }

        private void OnDestroy() 
        {
            CameraRegistry.Unregister(this);
        }
    }
}