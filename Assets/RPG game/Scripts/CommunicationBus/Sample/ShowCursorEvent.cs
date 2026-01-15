using TGL.RPG.CommunicationBus;

namespace RPG_Game.Scripts.CommunicationBus.Sample
{
    public class ShowCursorEvent : IMessageBody
    {
        public bool canShow;

        public ShowCursorEvent(bool showCursor)
        {
            canShow = showCursor;
        }
    }
}