using System;
using System.Collections.Generic;
using TGL.RPG.CommunicationBus;
using TGL.RPG.CommunicationBus.Sample;
using TGL.ServiceLocator;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace TGL.RPG.Mouse
{
    /// <summary>
    /// Checks if the cursor is over UI elements when mouse buttons are clicked or held.<br/>
    /// publishes <see cref="MessageTypes.CursorOverUI"/>  event messages when the state changes.<br/>
    /// is registered as a service of type <see cref="IDetectUiInteraction"/> in the <see cref="SLocator.GetSlGlobal"/> service locator.
    /// </summary>
    public class DetectCursorOverUi : MonoBehaviour, IDetectUiInteraction
    {
        #region Variables
        [SerializeField] private LayerMask uiLayerMask;
        
        private EventSystem eventSystem;
        private PointerEventData eventData;
        private List<RaycastResult> results = new List<RaycastResult>();
        
        private bool isOverUI;
        private bool isInteractionLocked;
        #endregion Variables

        #region Properties

        // Public property for other Scripts to read
        public bool IsOverUI
        {
            get => isOverUI;
            set
            {
                if (value.Equals(isOverUI)) return;
                isOverUI = value;
                // MessageBus.PublishMessage(MessageTypes.CursorOverUI, new CursorOverUiEvent(value));
            }
        }
        
        #endregion Properties

        #region MonoBehaviourMethods
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            SLocator.GetSlGlobal.Register(typeof(IDetectUiInteraction), this);
        }
        
        void Update()
        {
            UpdateLogic();
        }

        private void OnDestroy()
        {
            SLocator.GetSlGlobal?.UnRegister(typeof(IDetectUiInteraction));
        }
        #endregion MonoBehaviourMethods

        #region ActualLogic
        private void UpdateLogic()
        {
            // 1. Scene-safe EventSystem check
            if (eventSystem == null)
            {
                eventSystem = EventSystem.current;
                if (eventSystem == null) return;
                
                // Re-initialize eventData for the new EventSystem
                eventData = new PointerEventData(eventSystem);
            }

            // TODO: Expand for touch input if needed
            bool anyButtonHeld = UnityEngine.InputSystem.Mouse.current.leftButton.isPressed || UnityEngine.InputSystem.Mouse.current.rightButton.isPressed || UnityEngine.InputSystem.Mouse.current.middleButton.isPressed; 
            bool anyButtonDown = UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame || UnityEngine.InputSystem.Mouse.current.rightButton.wasPressedThisFrame || UnityEngine.InputSystem.Mouse.current.middleButton.wasPressedThisFrame;

            if (!anyButtonHeld)
            {
                // NO BUTTONS PRESSED: Normal Hover Mode
                IsOverUI = CheckForUIUnderCursor();
                isInteractionLocked = false;
            }
            else if (anyButtonDown && !isInteractionLocked)
            {
                // FIRST CLICK FRAME: Lock the state
                IsOverUI = CheckForUIUnderCursor();
                isInteractionLocked = true;
            }
            // During 'anyButtonHeld' but NOT 'anyButtonDown', we do nothing. This "retains" the old state
        }

        private bool CheckForUIUnderCursor()
        {
            if (eventSystem == null) return false;

            results.Clear();
            eventData.position = Input.mousePosition;
            eventSystem.RaycastAll(eventData, results);

            foreach (RaycastResult result in results)
            {
                // If any object in the raycast results matches our UI LayerMask
                if (((1 << result.gameObject.layer) & uiLayerMask) != 0)
                {
                    return true; // We found UI, no need to check the rest
                }
            }
            return false;
        }
        #endregion ActualLogic

        #region InterfaceMethods
        public bool GetIsOverUI() =>  IsOverUI;
        #endregion InterfaceMethods
    }
}

