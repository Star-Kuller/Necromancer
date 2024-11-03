using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Models
{
    public class Enemy : MonoBehaviour
    {
        public enum AIState
        {
            Patrol,
            Chase,
            Attack
        }
        
        public AIState currentState;
        [SerializeField] protected float patrolSpeed = 2f;
        [SerializeField] protected float chaseSpeed = 4f;
        [SerializeField] protected float attackRange = 2f;
        [SerializeField] protected Transform[] patrolPoints;
        [SerializeField] protected Transform player;

        protected int CurrentPatrolIndex = 0;
        protected readonly List<Transform> Targets = new();

        private NavMeshAgent _navMeshAgent;

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _navMeshAgent.updateRotation = false;
            _navMeshAgent.updateUpAxis = false;
        }

        private void Start()
        {
            currentState = AIState.Chase;
            StartCoroutine(AIStateMachine());
        }

        protected virtual IEnumerator AIStateMachine()
        {
            while (true)
            {
                yield return currentState switch
                {
                    AIState.Patrol => StartCoroutine(Patrol()),
                    AIState.Chase => StartCoroutine(Chase()),
                    AIState.Attack => StartCoroutine(Attack()),
                    _ => throw new ArgumentOutOfRangeException()
                };
                yield return null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Enter: " + other.name);
            Targets.Add(other.transform);
        }
        
        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Exit: " + other.name);
            Targets.Remove(other.transform);
        }

        protected virtual IEnumerator Patrol()
        {
            yield return null;
        }
        
        protected virtual IEnumerator Chase()
        {
            _navMeshAgent.SetDestination(player.position);
            yield return null;
        }
        
        protected virtual IEnumerator Attack()
        {
            yield return null;
        }
    }
}
