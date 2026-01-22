using Unity.Cinemachine;
using UnityEngine;
using TGL.ServiceLocator;

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
                SLocator.GetSlGlobal.Register(typeof(IActiveCameraProvider), this);
            }
            else
            {
                Debug.LogError($"Could not register GameCamera or Cinemachine on {gameObject.name} due to missing references.", this);
            }
        }

        private void OnDestroy() 
        {
            
            SLocator.GetSlGlobal?.UnRegister(typeof(IActiveCameraProvider));
        }
    }
}