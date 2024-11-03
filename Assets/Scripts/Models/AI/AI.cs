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

        [SerializeField] protected TeamType team;
        [SerializeField] protected AIState currentState;
        [SerializeField] protected float patrolSpeed = 2f;
        [SerializeField] protected float chaseSpeed = 3f;
        [SerializeField] protected float minAttackRange = 4f;
        [SerializeField] protected float maxAttackRange = 5f;
        [SerializeField] protected float damage = 5f;
        [SerializeField] protected float attackSpeed = 5f;
        [SerializeField] protected Transform[] patrolWay;
        
        protected int PatrolIndex = 0;
        protected Transform CurrentTarget;
        protected List<Transform> Tagets = new();

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
            if (CurrentTarget is not null)
            {
                if(!_targetDamageables.Any())
                    _targetDamageables = new List<IDamageable>(CurrentTarget.GetComponents<IDamageable>());
                
                foreach (var damageable in _targetDamageables)
                {
                    damageable.GetDamage(damage);
                }
                if (Distance(transform.position, CurrentTarget.position) > maxAttackRange)
                    ChangeState(AIState.Chase);

                yield return new WaitForSeconds( 1 / attackSpeed );
            }
            else
            {
                ChangeState(AIState.Idle);
            }
            yield return null;
        }

        protected virtual void ChangeState(AIState state)
        {
            _targetDamageables = new List<IDamageable>();
            currentState = state;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(team.ToString())) return;
            
            Tagets.Add(other.transform);
            UpdateTarget();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Tagets.Remove(other.transform);
            UpdateTarget();
        }
        
        protected void UpdateTarget()
        {
            if (Tagets.Any())
            {
                Transform nearest = null;
                var minDistance = float.MaxValue;
                foreach (var target in Tagets)
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
