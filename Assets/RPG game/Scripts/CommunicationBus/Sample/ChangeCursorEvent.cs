using TGL.RPG.CommunicationBus;
using TGL.RPG.GameCursor;

namespace TGL.RPG.CommunicationBus.Sample
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