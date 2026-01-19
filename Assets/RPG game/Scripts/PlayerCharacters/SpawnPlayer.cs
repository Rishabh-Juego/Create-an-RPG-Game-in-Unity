using System;
using TGL.RPG.CameraManagement;
using TGL.ServiceLocator;
using Unity.Cinemachine;
using UnityEngine;

namespace TGL.RPG.Character
{
    public class SpawnPlayer : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        private ISelectedCharacter playerCharacterInfo;
        private GameObject player;

        private void Awake()
        {
            ValidateSerializedFields();
        }

        private void ValidateSerializedFields()
        {
            if (spawnPoint == null)
            {
                Debug.LogError($"The spawn point is not assigned in the inspector.", gameObject);
            }
        }


        private void Start()
        {
            // get the selected player character info from the GameManager
            if (SLocator.GetSlGlobal.TryGet(out playerCharacterInfo))
            {
                player = Instantiate(playerCharacterInfo.SelectedCharacter.playerPrefab, spawnPoint.position, spawnPoint.rotation);
                player.name = playerCharacterInfo.SelectedCharacterName;
                if (!SLocator.GetSlGlobal.TryGet(out IActiveCameraProvider cameraProvider))
                {
                    Debug.LogError($"The player has scripts that require an active camera provider, but none was found in the scene.", gameObject);
                    return;
                }

                CinemachineCamera gameCinemachineCamera = cameraProvider.CameraSettings;
                if (gameCinemachineCamera == null)
                {
                    Debug.LogError($"No camera assigned in the active camera provider for the player.", gameObject);
                    return;
                }
                gameCinemachineCamera.Target = new CameraTarget()
                {
                    TrackingTarget = player.transform, // set the player as the tracking target
                    LookAtTarget = player.transform, // set the player as the look at target
                    CustomLookAtTarget = false // do not use a custom look at target, we track and look at the player
                };
            }
            else
            {
                Debug.LogError($"We did not find a selected player character in the Service Locator.", gameObject);
                return;
            }   
            Debug.Log($"Player is instantiated at spawn point: {player.transform.position}", gameObject);
        }
    }
}
