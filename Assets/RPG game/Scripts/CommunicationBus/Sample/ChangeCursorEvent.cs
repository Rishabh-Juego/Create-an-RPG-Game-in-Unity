using RPG_Game.Scripts.ChangeCursors;
using TGL.RPG.CommunicationBus;

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