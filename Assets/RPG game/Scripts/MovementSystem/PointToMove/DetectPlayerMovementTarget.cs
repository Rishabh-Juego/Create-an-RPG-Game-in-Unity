using System;
using TGL.RPG.CameraManagement;
using TGL.RPG.CommunicationBus;
using TGL.RPG.CommunicationBus.Sample;
using TGL.RPG.Mouse;
using TGL.ServiceLocator;
using UnityEngine;

namespace TGL.RPG.Navigation.PTM
{
    public class DetectPlayerMovementTarget : MonoBehaviour
    {
        private Ray ray;
        private RaycastHit hit;
        private Camera cam; // reference to the Game camera, using registry to get the active camera
        // services
        private IActiveCameraProvider activeCameraProvider;
        private IDetectUiInteraction uiInteraction;

        private void Awake()
        {
            GetAllServices();
        }

        private void Start()
        {
            UpdateSceneReferences();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) // TODO: Replace with input system
            {
                if (uiInteraction == null)
                {
                    GetAllServices();
                }
                if (uiInteraction.GetIsOverUI())
                {
                    return;
                }
                ray = cam.ScreenPointToRay(Input.mousePosition); // create a ray from the camera to the mouse position
                if (Physics.Raycast(ray, out hit)) // if the ray hits something
                {
                    MessageBus.PublishMessage(MessageTypes.PlayerMove, new PlayerMoveEvent(hit.point));
                }
            }
        }

        private void GetAllServices()
        {
            if (!SLocator.GetSlGlobal.TryGet(out activeCameraProvider))
            {
                Debug.LogWarning($"Could not find IActiveCameraProvider in the scene for {gameObject.name}");
                return;
            }

            if (!SLocator.GetSlGlobal.TryGet(out uiInteraction))
            {
                Debug.LogWarning($"Could not find IDetectUiInteraction in the scene for {gameObject.name}");
                return;
            }
        }

        public void UpdateSceneReferences()
        {
            if (activeCameraProvider == null)
            {
                GetAllServices();
            }
            cam = activeCameraProvider.ActiveCamera;
            if (cam == null)
            {
                Debug.LogError("No active camera found in the scene. Please ensure there is a camera with a GameCamera component.");
            }
        }
        

        private void OnDrawGizmos()
        {
            if (cam == null) return;
            
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(ray.origin, hit.point);
                Gizmos.DrawSphere(hit.point, 0.1f);
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(ray.origin, ray.direction * 100);
            }
        }
    }
}
