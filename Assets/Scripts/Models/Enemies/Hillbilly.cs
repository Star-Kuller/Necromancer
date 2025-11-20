using System.Collections;
using UnityEngine;

namespace Models.Enemies
{
    public class Hillbilly : AI.AI
    {
        private static readonly int IsWalk = Animator.StringToHash("IsWalk");
        private static readonly int IsRun = Animator.StringToHash("IsRun");
        private static readonly int AttackAnimation = Animator.StringToHash("Attack");
        private Animator _animator;
        private float _threshold = 0.05f;
        
        public override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        public override void Update()
        {
            base.Update();
            _animator.SetBool(IsWalk, NavMeshAgent.velocity.sqrMagnitude > _threshold);
            _animator.SetBool(IsRun, currentState == AIState.Chase);
        }

        protected override IEnumerator DealDamage()
        {
            _animator.SetTrigger(AttackAnimation);
            yield return new WaitForSeconds(0.3f);
            yield return base.DealDamage();
        }
    }
}