using TGL.RPG.CommunicationBus;
using TGL.RPG.GameCursor;

namespace RPG_Game.Scripts.CommunicationBus.Sample
{
    public class ChangeCursorEvent : IMessageBody
    {
        public CursorTypes setCursorType;

        public ChangeCursorEvent(CursorTypes cursorType)
        {
            setCursorType = cursorType;
        }
    }
}