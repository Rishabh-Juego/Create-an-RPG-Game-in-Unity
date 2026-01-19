using UnityEngine;

namespace TGL.RPG.CommunicationBus.Sample
{
    public class CursorOverUiEvent  : IMessageBody
    {
        public bool isOverUI;

        public CursorOverUiEvent(bool overUI)
        {
            isOverUI = overUI;
        }
    }
}
