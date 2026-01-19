using System;
using System.Collections.Generic;
using UnityEngine;

namespace TGL.RPG.CommunicationBus
{
    public static class MessageBus
    {
        private static Dictionary<MessageTypes, List<Action<IMessageBody>>> messageSubscribers = new Dictionary<MessageTypes, List<Action<IMessageBody>>>();
        private static Action<MessageTypes, IMessageBody> OnPublishMessage;
        private static Action<MessageTypes, IMessageBody> OnMessageReceived;

        
        public static void PublishMessage(MessageTypes msgType, IMessageBody msgBody)
        {
            if (messageSubscribers.ContainsKey(msgType))
            {
                messageSubscribers[msgType]?.ForEach(subscriber => subscriber?.Invoke(msgBody));
            }
            else
            {
                Debug.LogWarning($"We are trying to publish a message type which is not present: {msgType}");
            }
        }
        
        public static void RegisterMessageListener(MessageTypes msgType, Action<IMessageBody> listener)
        {
            if (messageSubscribers.ContainsKey(msgType))
            {
                if (messageSubscribers[msgType] != null)
                {
                    messageSubscribers[msgType].Add(listener);
                }
                else
                {
                    messageSubscribers[msgType] = new List<Action<IMessageBody>> { listener };
                }
            }
            else
            {
                messageSubscribers[msgType] = new List<Action<IMessageBody>> { listener };
            }
        }

        public static void UnregisterMessageListener(MessageTypes msgType, Action<IMessageBody> listener)
        {
            if (messageSubscribers.ContainsKey(msgType))
            {
                if (messageSubscribers[msgType] != null && messageSubscribers[msgType].Contains(listener))
                {
                    messageSubscribers[msgType].Remove(listener);
                }
                else
                {
                    Debug.LogWarning($"We are trying to unregister a listener that was not registered for message type: {msgType}");
                }
            }
            else
            {
                Debug.LogWarning($"We are trying to unregister a listener when the type was not registered: {msgType}");
            }
        }

    }
}