using TGL.RPG.CommunicationBus;

namespace TGL.RPG.CommunicationBus.Sample
{
    public class ShowCursorEvent : IMessageBody
    {
        public readonly bool canShow;

        public ShowCursorEvent(bool showCursor)
        {
            canShow = showCursor;
        }
    }
}