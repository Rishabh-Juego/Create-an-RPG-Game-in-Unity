using System;
using TGL.RPG.Constants.Sample;
using UnityEngine;
using UnityEngine.AI;

namespace TGL.RPG.Navigation.PTM.Sample
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerAnimationController : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Animator _animator;
        private float velX, velZ, velocityXZ;

        private void Awake()
        {
            _agent = GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            _animator = GetComponent(typeof(Animator)) as Animator;
        }

        private void Update()
        {
            velX = _agent.velocity.x;
            velZ = _agent.velocity.z;
            velocityXZ = (float)Math.Sqrt(velX * velX + velZ * velZ); // x^2 + y^2 = magnitude^2
        }

        private void LateUpdate()
        {
            _animator.SetBool(GameConstants.PlayerAnimConstants.SPRINT_BOOL, !Mathf.Approximately(velocityXZ,  0));
        }
    }
}