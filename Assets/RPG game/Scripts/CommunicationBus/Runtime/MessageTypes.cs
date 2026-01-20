namespace TGL.RPG.CommunicationBus
{
    public enum MessageTypes
    {
        NONE = 0,
        ChangeCursor = 1,
        ShowCursor = 2, // Currently unused. no publish calls for this message type.
        ActivateSingleScene = 3,
        PlayerMove = 4,
        CursorOverUI = 5,
        AddItemToInventory = 6,
        AddItemsToInventory = 7, // if we want to pick multiple items at once in the future.
        
    }
}