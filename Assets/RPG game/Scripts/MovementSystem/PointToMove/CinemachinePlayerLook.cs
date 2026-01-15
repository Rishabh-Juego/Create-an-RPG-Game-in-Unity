using System;
using System.Collections;
using RPG_Game.Scripts.ChangeCursors;
using RPG_Game.Scripts.CommunicationBus.Sample;
using TGL.RPG.CommunicationBus;
using TGL.RPG.Constants.Sample;
using Unity.Cinemachine;
using UnityEngine;

namespace TGL.RPG.Navigation.PTM
{
    public class CinemachinePlayerLook : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera playerCamera;
        // Horizontal angle limits,
        [Header("Horizontal"), SerializeField] private bool horizontalNoLimit = true;
        [SerializeField, Range(-179.99f, 179.99f)] private float horizontalMinLimit, horizontalMaxLimit;
        
        // Vertical angle limits
        [Header("Vertical"), SerializeField, Range(-10f, 45f)] private float verticalMinLimit = -2f;
        [SerializeField, Range(-10f, 45f)] private float verticalMaxLimit = 35f;
        
        // Zoom distance limits
        [Header("Zoom"), SerializeField, Range(0.1f, 1.5f)] private float zoomSensitivity = 0.35f;
        [SerializeField, Range(2f, 15f)] private float zoomMinLimit = 2;
        [SerializeField, Range(2f, 15f)] private float zoomMaxLimit = 15;
        
        private CinemachineOrbitalFollow orbitalFollow;
        private float currHorizontalAngle, currVerticalAngle, currZoomDistance;
        
        // screen details
        private Coroutine ValidateScreenDimensionsCoroutine;
        private float halfScreenWidth;
        private float halfScreenHeight;

        #region MonoBehaviourMethods

        private void Awake()
        {
            if (playerCamera == null)
            {
                Debug.LogError($"No camera assigned in {nameof(CinemachinePlayerLook)} on {gameObject.name}", this);
                return;
            }
            
            // get the  'CinemachineOrbitalFollow' from the 'Body' stage of the Cinemachine Camera
            orbitalFollow = playerCamera.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineOrbitalFollow;

            if (orbitalFollow == null)
            {
                Debug.LogError($"No 'CinemachineOrbitalFollow' found in {playerCamera.name}, are we still using 'PositionControl: Orbital Follow' in the inspector?", playerCamera);
            }
            else
            {
                currHorizontalAngle = orbitalFollow.HorizontalAxis.Value;
                currVerticalAngle = orbitalFollow.VerticalAxis.Value;
                currZoomDistance = orbitalFollow.Radius;
                
                if(currHorizontalAngle > horizontalMaxLimit || currHorizontalAngle < horizontalMinLimit)
                {
                    Debug.LogWarning($"Initial Horizontal Angle {currHorizontalAngle} is out of bounds [{horizontalMinLimit}, {horizontalMaxLimit}]");
                }
                if(currVerticalAngle > verticalMaxLimit || currVerticalAngle < verticalMinLimit)
                {
                    Debug.LogWarning($"Initial Vertical Angle {currVerticalAngle} is out of bounds [{verticalMinLimit}, {verticalMaxLimit}]");
                }
                if(currZoomDistance > zoomMaxLimit || currZoomDistance < zoomMinLimit)
                {
                    Debug.LogWarning($"Initial Zoom Distance {currZoomDistance} is out of bounds [{zoomMinLimit}, {zoomMaxLimit}]");
                }
            }
        }

        private void OnEnable()
        {
#if UNITY_EDITOR
            if(ValidateScreenDimensionsCoroutine != null) { StopCoroutine(ValidateScreenDimensionsCoroutine); }
            ValidateScreenDimensionsCoroutine = StartCoroutine(CalculateScreenDimentions());
#else
            UpdateScreenDimentions();
#endif
        }

        private void Update()
        {
            // TODO: Replace with input system
            if (Input.mouseScrollDelta.y != 0) // mouse scrolled
            {
                HandleChangeCameraZoom();
            }
            
            // TODO: Replace with input system
            if (Input.GetMouseButtonDown(1)) // Right Click
            {
                // TODO: Find a way to decouple Cursor from this class
                MessageBus.PublishMessage(MessageTypes.ChangeCursor, new ChangeCursorEvent(CursorTypes.MoveCamera));
            }
            if (Input.GetMouseButton(1)) // Right Click
            {
                HandleChangeCameraAngles();
            }
            if (Input.GetMouseButtonUp(1)) // Right Click
            {
                // TODO: Find a way to decouple Cursor from this class
                MessageBus.PublishMessage(MessageTypes.ChangeCursor, new ChangeCursorEvent(CursorTypes.Basic));
            }
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            if (ValidateScreenDimensionsCoroutine != null) { StopCoroutine(ValidateScreenDimensionsCoroutine); }
#endif
        }
        
        #endregion MonoBehaviourMethods

        private void UpdateScreenDimentions()
        {
            halfScreenWidth = Screen.width / 2.0f;
            halfScreenHeight = Screen.height / 2.0f;
        }

        private void HandleChangeCameraAngles()
        {
            // Get new values
            currVerticalAngle -= Input.mousePositionDelta.y; // TODO: Replace with input system
            currHorizontalAngle += Input.mousePositionDelta.x; // TODO: Replace with input system
            
            if (horizontalNoLimit)
            {
                if(currHorizontalAngle < -180f) { currHorizontalAngle += 360f; }
                if(currHorizontalAngle > 180f) { currHorizontalAngle -= 360f; }
            }
            else
            {
                currHorizontalAngle = Mathf.Clamp(currHorizontalAngle, horizontalMinLimit, horizontalMaxLimit);
            }
            
            currVerticalAngle = Mathf.Clamp(currVerticalAngle, verticalMinLimit, verticalMaxLimit);
            orbitalFollow.HorizontalAxis.Value = currHorizontalAngle;
            orbitalFollow.VerticalAxis.Value = currVerticalAngle;
        }

        private void HandleChangeCameraZoom()
        {
            currZoomDistance -= Input.mouseScrollDelta.y * zoomSensitivity; // TODO: Replace with input system
            orbitalFollow.Radius = Mathf.Clamp(currZoomDistance, zoomMinLimit, zoomMaxLimit);
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
    }
}
