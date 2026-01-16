using TGL.RPG.CameraManagement;
using UnityEngine;
using UnityEngine.AI;

namespace TGL.RPG.Navigation.PTM
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMovementPtm : MonoBehaviour
    {
        private Camera cam; // reference to the Game camera, using registry to get the active camera
        private NavMeshAgent _agent; // agent which will move using the current script
        private Ray ray;
        private RaycastHit hit;
        
        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            UpdateSceneReferences();
        }

        public void UpdateSceneReferences()
        {
            cam = CameraRegistry.Get()?.ActiveCamera;
            if (cam == null)
            {
                Debug.LogError("No active camera found in the scene. Please ensure there is a camera with a GameCamera component.");
            }
        }
        
        void Update()
        {
            // TODO: Replace with input system
            if (Input.GetMouseButtonDown(0)) // on left mouse button click
            {
                ray = cam.ScreenPointToRay(Input.mousePosition); // create a ray from the camera to the mouse position
                if (Physics.Raycast(ray, out hit)) // if the ray hits something
                {
                    _agent.SetDestination(hit.point); // set the agent's destination to the hit point
                }
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