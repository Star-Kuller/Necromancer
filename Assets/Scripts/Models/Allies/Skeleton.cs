using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models.Allies
{
    public class Skeleton : AI.DefencePointAI
    {
        private static readonly int IsMoving = Animator.StringToHash("IsMoving");
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
            _animator.SetBool(IsMoving, NavMeshAgent.velocity.sqrMagnitude > _threshold);
        }

        public override void Reset()
        {
            base.Reset();
            DefencePoint = null;
        }
        
        protected override IEnumerator DealDamage()
        {
            _animator.SetTrigger(AttackAnimation);
            yield return new WaitForSeconds(0.3f);
            yield return base.DealDamage();
        }
    }
}