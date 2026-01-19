using System;
using TGL.RPG.CameraManagement;
using TGL.RPG.CommunicationBus;
using TGL.RPG.CommunicationBus.Sample;
using TGL.ServiceLocator;
using UnityEngine;
using UnityEngine.AI;

namespace TGL.RPG.Navigation.PTM
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerMovementPtm : MonoBehaviour
    {
        private NavMeshAgent _agent; // agent which will move using the current script

        private void OnEnable()
        {
            MessageBus.RegisterMessageListener(MessageTypes.PlayerMove, OnPlayerMoveMessage);
        }

        private void OnDisable()
        {
            MessageBus.UnregisterMessageListener(MessageTypes.PlayerMove, OnPlayerMoveMessage);
        }

        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        
        private void OnPlayerMoveMessage(IMessageBody obj)
        {
            if(obj is not PlayerMoveEvent hit) return;
            
            _agent.SetDestination(hit.targetPosition);
        }
    }
    
}