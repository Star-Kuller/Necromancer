using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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

        public AIState CurrentState
        {
            get => currentState;
            set => currentState = value;
        }

        [SerializeField] protected TeamType attackTeam = TeamType.Ally;
        [SerializeField] protected AIState currentState;
        [SerializeField] protected float patrolSpeed = 2f;
        [SerializeField] protected float chaseSpeed = 3f;
        [SerializeField] protected float minAttackRange = 1f;
        [SerializeField] protected float maxAttackRange = 1.7f;
        [SerializeField] protected float damage = 5f;
        [SerializeField] protected float attackSpeed = 1f;
        [SerializeField] protected Transform[] patrolWay;
        
        protected int PatrolIndex = 0;
        [CanBeNull] protected Transform CurrentTarget;
        protected Vector3 LastPositionOfTarget;
        protected readonly Dictionary<Transform, IDamageable> Targets = new();

        protected NavMeshAgent NavMeshAgent;
        private SpriteRenderer _render;

        public virtual void Awake()
        {
            NavMeshAgent = GetComponent<NavMeshAgent>();
            _render = GetComponent<SpriteRenderer>();
            NavMeshAgent.updateRotation = false;
            NavMeshAgent.updateUpAxis = false;
        }

        public virtual void Update()
        {
            _render.flipX = NavMeshAgent.velocity.x switch
            {
                > 0 => false,
                < 0 => true,
                _ => _render.flipX
            };
        }

        private void OnEnable()
        {
            currentState = AIState.Chase;
            StartCoroutine(AIStateMachine());
            CheckExistingColliders();
        }
        
        private void CheckExistingColliders()
        {
            var triggerColliders = GetComponents<Collider2D>();
            foreach (var triggerCollider in triggerColliders.Where(x => x.isTrigger))
            {
                var colliders = new Collider2D[15];
                var filter = new ContactFilter2D().NoFilter();
                var count = triggerCollider.Overlap(filter, colliders);
                for (var i = 0; i < count; i++)
                {
                    if (!colliders.Contains(colliders[i]))
                    {
                        OnTriggerEnter2D(colliders[i]);
                    }
                }
            }
        }

        protected void SetDestination(Vector3 position)
        {
            if (!(Distance(position, LastPositionOfTarget) > 1)) return;
            
            LastPositionOfTarget = position;
            NavMeshAgent.SetDestination(LastPositionOfTarget);
        }

        private IEnumerator AIStateMachine()
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
            if (patrolWay.Any())
            {
                SetDestination(patrolWay[PatrolIndex].position);
                if (Distance(transform.position, patrolWay[PatrolIndex].position) < 0.2)
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
                SetDestination(CurrentTarget.position);
                if (Distance(transform.position, CurrentTarget.position) <= minAttackRange)
                    ChangeState(AIState.Attack);
                UpdateTarget();
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
                StartCoroutine(DealDamage());
                yield return new WaitForSeconds(1 / attackSpeed);
            }

            if (CurrentTarget is null)
            {
                ChangeState(AIState.Idle);
            }
        }

        protected virtual IEnumerator DealDamage()
        {
            if(CurrentTarget is not null) 
                Targets[CurrentTarget].DealDamage(damage);
            yield break;
        }
        
        protected virtual IEnumerator CheckDistance()
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
            currentState = state;
            switch (state)
            {
                case AIState.Idle:
                    NavMeshAgent.speed = patrolSpeed;
                    break;
                case AIState.Chase:
                    NavMeshAgent.speed = chaseSpeed;
                    break;
                case AIState.Attack:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            
            if (other.isTrigger) return;
            if (!(other.CompareTag(attackTeam.ToString()) 
                || (other.CompareTag("Player") && attackTeam == TeamType.Ally))) return;
            
            Targets.TryAdd(other.transform, other.transform.GetComponent<IDamageable>());
            UpdateTarget();
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            if(other.isTrigger) return;
            Targets.Remove(other.transform);
            UpdateTarget();
        }
        
        protected virtual void UpdateTarget()
        {
            if (Targets.Any())
            {
                Transform nearest = null;
                var minDistance = float.MaxValue;
                foreach (var target in Targets.Keys)
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
                ChangeState(AIState.Idle);
                CurrentTarget = null;
            }
        }
    }
}
