using UnityEngine;

namespace TGL.RPG.CommunicationBus.Sample
{
    public class PlayerMoveEvent : IMessageBody
    {
        public readonly Vector3 targetPosition;

        public PlayerMoveEvent(Vector3 position)
        {
            targetPosition = position;
        }
    }
}
