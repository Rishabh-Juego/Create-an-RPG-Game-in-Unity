using System;
using System.Collections.Generic;
using TGL.RPG.CommunicationBus;
using TGL.RPG.CommunicationBus.Sample;
using UnityEngine;
using UnityEngine.UI;

namespace TGL.RPG.GameCursor
{
    [RequireComponent(typeof(Image))]
    public class CursorHandler : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private List<CursorMapping> cursorMappings;

        private Image cursorImage;
        private GameObject cursorObj;

        private void Awake()
        {
            if (ValidateSerializedFields())
            {
                RegisterMessageBusEvents();
                DisableUserCursor();
            }
            else
            {
                Debug.LogError($"Serialized fields not valid on {gameObject.name}", this);
            }
        }

        private void Update()
        {
            // TODO: Replace with input system
            cursorObj.transform.position = Input.mousePosition;
        }

        private void OnDestroy()
        {
            UnregisterMessageBusEvents();
        }

        private void RegisterMessageBusEvents()
        {
            MessageBus.RegisterMessageListener(MessageTypes.ShowCursor, ShowCursor);
            MessageBus.RegisterMessageListener(MessageTypes.ChangeCursor, ChangeCursorType);
        }

        private void UnregisterMessageBusEvents()
        {
            MessageBus.UnregisterMessageListener(MessageTypes.ShowCursor, ShowCursor);
            MessageBus.UnregisterMessageListener(MessageTypes.ChangeCursor, ChangeCursorType);
        }

        private bool ValidateSerializedFields()
        {
            cursorImage = GetComponent<Image>();
            if (cursorImage == null)
            {
                Debug.LogWarning($"could not find Image component on {gameObject.name}", this);
                return false;
            }

            cursorObj = cursorImage.gameObject;
            return true;
        }

        private void DisableUserCursor()
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }

        private void ShowCursor(IMessageBody msgBody)
        {
            // TODO: When user is on different screen, consider hiding the custom cursor and showing system cursor as needed
            if (msgBody is not ShowCursorEvent showCursorEvent) return;
            cursorImage.enabled = showCursorEvent.canShow;
        }

        private void ChangeCursorType(IMessageBody msgBody)
        {
            if (msgBody is not ChangeCursorEvent changeCursorEvent) return;

            CursorMapping mappedCursor = cursorMappings.Find(mapping => mapping.cursorType == changeCursorEvent.setCursorType);
            if (mappedCursor.cursorSprite == null)
            {
                throw new ArgumentOutOfRangeException(nameof(changeCursorEvent.setCursorType),
                    changeCursorEvent.setCursorType, $"No entry for cursor type {changeCursorEvent.setCursorType} found");
            }

            cursorImage.sprite = mappedCursor.cursorSprite;
        }
    }
}
