using TGL.RPG.CommunicationBus;

namespace TGL.RPG.CommunicationBus.Sample
{
    /// <summary>
    /// Currently un-used, but intended to signal whether the cursor can be shown or not.
    /// </summary>
    public class ShowCursorEvent : IMessageBody
    {
        public readonly bool canShow;

        public ShowCursorEvent(bool showCursor)
        {
            canShow = showCursor;
        }
    }
}