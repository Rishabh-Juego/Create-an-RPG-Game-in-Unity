using System;
using System.Collections;
using TGL.RPG.Constants.Sample;
using Unity.Cinemachine;
using UnityEngine;

namespace TGL.RPG.Navigation.PTM
{
    public class CinemachinePlayerLook : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera playerCamera;
        [SerializeField] private bool useScreenEdgeLook; 
        private CinemachinePositionComposer camPosComposer;
        
        private Vector3 currOffset; // currPos
        private Vector3 mousePos; // pos
        
        // screen details
        private Coroutine ValidateScreenDimensionsCoroutine;
        private float halfScreenWidth;
        private float halfScreenHeight;

        private void Awake()
        {
            if (playerCamera == null)
            {
                Debug.LogError($"No camera assigned in {nameof(CinemachinePlayerLook)} on {gameObject.name}", this);
                return;
            }
            
            // get the  'CinemachinePositionComposer' from the 'Body' stage of the Cinemachine Camera
            camPosComposer = playerCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachinePositionComposer;

            if (camPosComposer == null)
            {
                Debug.LogError($"No 'CinemachinePositionComposer' found in {playerCamera.name}, are we still using 'PositionControl: PositionComposer' in the inspector?", playerCamera);
            }
            else
            {
                currOffset = camPosComposer.TargetOffset; // offset from the target which we want to track using the camera, we do not track the target directly, but rather the offset from it
            }
        }

        private void OnEnable()
        {
            #if UNITY_EDITOR
            if(ValidateScreenDimensionsCoroutine != null)
                StopCoroutine(ValidateScreenDimensionsCoroutine);
            
            ValidateScreenDimensionsCoroutine = StartCoroutine(CalculateScreenDimentions());
            #else
            UpdateScreenDimentions();
            #endif
        }

        private void UpdateScreenDimentions()
        {
            halfScreenWidth = Screen.width / 2.0f;
            halfScreenHeight = Screen.height / 2.0f;
        }

        private void Update()
        {
            // TODO: Replace with input system
            mousePos = Input.mousePosition;
            camPosComposer.TargetOffset = currOffset; // set to calculated offset
            
            // TODO: Replace with input system
            if (Input.GetMouseButton(1)) // Right Click
            {
                // out of bounds check
                if(mousePos.x < 0 || mousePos.x > Screen.width || 
                   mousePos.y < 0 || mousePos.y > Screen.height)
                {
                    return; 
                }

                if (useScreenEdgeLook)
                {
                    // if we want to limit the offset by a factor of screen dimensions
                    currOffset = mousePos / GameConstants.PlayerCameraConstants.mouseCamOffsetFactor; // MouseCamOffsetFactor - calculate new offset based on mouse position
                }
                else
                {   
                    // bottom left of screen is (0,0), top right is (Screen.width, Screen.height)
                    mousePos.x -= halfScreenWidth; // Centering X
                    mousePos.y -= halfScreenHeight; // Centering Y
                    
                    // if we want to limit the offset by max offset values
                    currOffset = new Vector3((mousePos.x/halfScreenWidth) * GameConstants.PlayerCameraConstants.maxTargetOffsetX, currOffset.y, (mousePos.y / halfScreenHeight) * GameConstants.PlayerCameraConstants.maxTargetOffsetY);
                }

                Debug.Log($"For mouse pos: {mousePos} in screen ({Screen.width}, {Screen.height}), new offset is: {currOffset}");
            }
        }

        private IEnumerator CalculateScreenDimentions()
        {
            while (true)
            {
                UpdateScreenDimentions();
                yield return new WaitForSeconds(10); // in case user decides to change screen resolution while in editor
                //TODO: Add event listener for resolution change instead of polling
            }            
        }

        private void OnDisable()
        {
            
#if UNITY_EDITOR
            if(ValidateScreenDimensionsCoroutine != null)
                StopCoroutine(ValidateScreenDimensionsCoroutine);
#endif
        }
    }
}
