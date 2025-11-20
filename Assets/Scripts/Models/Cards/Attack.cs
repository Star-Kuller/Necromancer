using System.Linq;
using Models.AI;
using Services.Cards;
using Services.DependencyInjection;
using Services.Interfaces;
using UnityEngine;

namespace Models.Cards
{
    public class Attack : Card
    {
        public override CardType Type => CardType.Attack;
        [SerializeField] public float radius = 1f;
        [SerializeField] private float damage = 30;

        [Inject] private IObjectPool _objectPool;
        private Transform _player;

        [Inject]
        private void Init(IPlayerService playerService)
        {
            _player = playerService.PlayerTransform;
        }

        public override void ApplyEffect()
        {
            var objects = GameObject.FindGameObjectsWithTag("Enemy");

            GameObject closestObj = null;
            var minDistanceSqr = Mathf.Infinity;
            
            foreach (var obj in objects)
            {
                if (!obj.CompareTag(nameof(TeamType.Enemy))) continue;
                
                var distance = (_player.position - obj.transform.position).sqrMagnitude;
                if (distance >= minDistanceSqr) continue;
                
                minDistanceSqr = distance;
                closestObj = obj;
            }

            if (closestObj is null || Vector3.Distance(closestObj.transform.position, _player.position) > radius)
                return;
            
            var damagables = closestObj.GetComponents<IDamageable>();
            foreach (var damageable in damagables)
            {
                damageable.DealDamage(damage);
            }
        }
    }
}