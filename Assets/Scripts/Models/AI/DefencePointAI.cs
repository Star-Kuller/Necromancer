using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Models.AI
{
    public class DefencePointAI : AI, IResetable
    {
        public Transform DefencePoint { get => defencePoint; set => defencePoint = value; }
        
        [SerializeField] protected Transform defencePoint;
        [SerializeField] private LayerMask rayCastObstacleLayer;
        [SerializeField] protected float defenceMaxDistance = 5f;
        [SerializeField] protected int defenceMaxAttempts = 50;
        protected Vector3 CurrentPatrolDefencePosition;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform == defencePoint)
                CurrentPatrolDefencePosition = GetRandomVisiblePoint();
        }
        
        public virtual void Reset()
        {
            CurrentPatrolDefencePosition = new Vector3();
        }

        protected override IEnumerator Idle()
        {
            StartCoroutine(CheckState());
            StartCoroutine(ChangePosition());
            while (CurrentState == AIState.Idle)
            {
                SetDestination(DefencePoint.position + CurrentPatrolDefencePosition);
                yield return new WaitForSeconds(0.5f);
            }
        }
        private IEnumerator ChangePosition()
        {
            while (CurrentState == AIState.Idle)
            {
                CurrentPatrolDefencePosition = GetRandomVisiblePoint();
                yield return new WaitForSeconds(10);
            }
        }

        private IEnumerator CheckState()
        {
            while (CurrentState == AIState.Idle)
            {
                if (CurrentTarget is not null)
                    ChangeState(AIState.Chase);
                yield return null;
            }
        }

        protected virtual Vector3 GetRandomVisiblePoint()
        {
            for (var i = 0; i < defenceMaxAttempts; i++)
            {
                var randomPoint2D = Random.insideUnitCircle * defenceMaxDistance;
                if(randomPoint2D.magnitude < 1) continue;
                
                var randomPoint = new Vector3(randomPoint2D.x, randomPoint2D.y, 0);
                var randomPointNearTarget = defencePoint.position + randomPoint;
                
                if (IsPointVisible(randomPointNearTarget, defencePoint.position))
                    return randomPoint;
            }
            
            return new Vector3();
        }
        
        protected virtual bool IsPointVisible(Vector3 targetPoint, Vector3 observer)
        {
            var direction = targetPoint - observer;
            var distance = direction.magnitude;
            
            return !Physics.Raycast(observer, direction, distance, rayCastObstacleLayer);
        }
    }
}