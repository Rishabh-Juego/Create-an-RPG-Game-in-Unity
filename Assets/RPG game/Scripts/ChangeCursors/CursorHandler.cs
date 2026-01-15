
using System;
using System.Collections.Generic;
using RPG_Game.Scripts.ChangeCursors;
using RPG_Game.Scripts.CommunicationBus.Sample;
using TGL.RPG.CommunicationBus;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CursorHandler : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private List<CursorMapping> cursorMappings;
    [SerializeField] private Sprite cursorBasic; // CursorTypes.Basic = 0;
    [SerializeField] private Sprite cursorMoveCamera; // CursorTypes.MoveCamera = 1;
    
    
    private Image cursorImage;
    private GameObject cursorObj;
    
    private void Awake()
    {
        cursorImage = GetComponent<Image>();
        cursorObj = cursorImage.gameObject;
        MessageBus.RegisterMessageListener(MessageTypes.ShowCursor, ShowCursor);
        MessageBus.RegisterMessageListener(MessageTypes.ChangeCursor, ChangeCursorType);


        Cursor.lockState  = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        // TODO: Replace with input system
        cursorObj.transform.position = Input.mousePosition; 
    }

    private void ShowCursor(IMessageBody msgBody)
    {
        if(msgBody is not ShowCursorEvent showCursorEvent) return;
        cursorImage.enabled = showCursorEvent.canShow;
    }

    private void ChangeCursorType(IMessageBody msgBody)
    {
        if(msgBody is not ChangeCursorEvent changeCursorEvent) return;
        
        CursorMapping mappedCursor = cursorMappings.Find(mapping => mapping.cursorType == changeCursorEvent.setCursorType);
        if (mappedCursor.cursorSprite == null)
        {
            throw new ArgumentOutOfRangeException(nameof(changeCursorEvent.setCursorType), 
                changeCursorEvent.setCursorType, $"No entry for cursor type {changeCursorEvent.setCursorType} found");
        }
        cursorImage.sprite = mappedCursor.cursorSprite;
    }

    private void OnDestroy()
    {
        MessageBus.UnregisterMessageListener(MessageTypes.ShowCursor, ShowCursor);
        MessageBus.UnregisterMessageListener(MessageTypes.ChangeCursor, ChangeCursorType);
    }
}
