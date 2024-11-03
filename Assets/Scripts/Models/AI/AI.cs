using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Vector3;

namespace Models.AI
{
    public class AI : MonoBehaviour
    {
        public enum AIState
        {
            Idle,
            Chase,
            Attack
        }
        
        public enum TeamType
        {
            Ally,
            Enemy
        }

        public TeamType Team => team;

        public AIState CurrentState
        {
            get => currentState;
            set => currentState = value;
        }

        [SerializeField] protected TeamType team = TeamType.Enemy;
        [SerializeField] protected AIState currentState;
        [SerializeField] protected float patrolSpeed = 2f;
        [SerializeField] protected float chaseSpeed = 3f;
        [SerializeField] protected float minAttackRange = 1f;
        [SerializeField] protected float maxAttackRange = 1.2f;
        [SerializeField] protected float damage = 5f;
        [SerializeField] protected float attackSpeed = 1f;
        [SerializeField] protected Transform[] patrolWay;
        
        protected int PatrolIndex = 0;
        protected Transform CurrentTarget;
        protected readonly List<Transform> Targets = new();

        private NavMeshAgent _navMeshAgent;
        private List<IDamageable> _targetDamageables = new();

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
                    AIState.Idle => StartCoroutine(Idle()),
                    AIState.Chase => StartCoroutine(Chase()),
                    AIState.Attack => StartCoroutine(Attack()),
                    _ => throw new ArgumentOutOfRangeException()
                };
                yield return null;
            }
        }

        protected virtual IEnumerator Idle()
        {
            _navMeshAgent.speed = patrolSpeed;
            if (patrolWay.Any())
            {
                _navMeshAgent.SetDestination(patrolWay[PatrolIndex].position);
                if (Distance(transform.position, patrolWay[PatrolIndex].position) < 1)
                {
                    PatrolIndex++;
                    if (PatrolIndex >= patrolWay.Length)
                    {
                        patrolWay = patrolWay.Reverse().ToArray();
                        PatrolIndex = 0;
                    }
                }
            }
            if (CurrentTarget is not null)
                ChangeState(AIState.Chase);
            
            yield return null;
        }
        
        protected virtual IEnumerator Chase()
        {
            if (CurrentTarget is not null)
            {
                _navMeshAgent.speed = chaseSpeed;
                _navMeshAgent.SetDestination(CurrentTarget.position);
                if (Distance(transform.position, CurrentTarget.position) < minAttackRange)
                    ChangeState(AIState.Attack);
            }
            else
            {
                ChangeState(AIState.Idle);
            }
            yield return null;
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        protected virtual IEnumerator Attack()
        {
            StartCoroutine(CheckDistance());
            
            while (CurrentTarget is not null && CurrentState == AIState.Attack)
            {
                if (_targetDamageables is null || !_targetDamageables.Any())
                    _targetDamageables = new List<IDamageable>(CurrentTarget.GetComponents<IDamageable>());

                foreach (var damageable in _targetDamageables)
                {
                    damageable.GetDamage(damage);
                }

                yield return new WaitForSeconds(1 / attackSpeed);
            }

            if (CurrentTarget == null)
            {
                ChangeState(AIState.Idle);
            }
        }
        
        private IEnumerator CheckDistance()
        {
            while (CurrentTarget is not null && CurrentState == AIState.Attack)
            {
                if (Distance(transform.position, CurrentTarget.position) > maxAttackRange)
                {
                    ChangeState(AIState.Chase);
                    yield break;
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        protected virtual void ChangeState(AIState state)
        {
            _targetDamageables = new List<IDamageable>();
            currentState = state;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var enemy = team switch
            {
                TeamType.Ally => "Enemy",
                TeamType.Enemy => "Ally",
                _ => throw new ArgumentOutOfRangeException()
            };
            if (!other.CompareTag(enemy)) return;
            
            Targets.Add(other.transform);
            UpdateTarget();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Targets.Remove(other.transform);
            UpdateTarget();
        }
        
        protected void UpdateTarget()
        {
            if (Targets.Any())
            {
                Transform nearest = null;
                var minDistance = float.MaxValue;
                foreach (var target in Targets)
                {
                    var distance = Distance(transform.position, target.position);
                    if (!(distance < minDistance)) continue;
                    minDistance = distance;
                    nearest = target;
                }
                CurrentTarget = nearest;
            }
            else
            {
                CurrentTarget = null;
            }
        }
    }
}
