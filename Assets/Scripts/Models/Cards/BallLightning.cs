using Models.AI;
using Services.Cards;
using Services.DependencyInjection;
using Services.Interfaces;
using UnityEngine;

namespace Models.Cards
{
    public class BallLightning : Card
    {
        public override CardType Type => CardType.BallLightning;
        [SerializeField] private GameObject projectile;
        [SerializeField] private float damage = 30;
        [SerializeField] private float projectileSpeed = 5;

        [Inject] private IObjectPool _objectPool;
        private Transform _player;
        private Camera _mainCamera;

        [Inject]
        private void Init(IPlayerService playerService)
        {
            _player = playerService.PlayerTransform;
            _mainCamera = Camera.main; 
        }
        
        public override void ApplyEffect()
        {
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; 
            var direction = (mousePosition - _player.position).normalized;
            
            var newProjectile = _objectPool.Create($"{Type}_Projectile", projectile);
            newProjectile.transform.position = _player.position;
            
            var rb = newProjectile.GetComponent<Rigidbody2D>();
            var projectileModel = newProjectile.GetComponent<Projectile>();
            projectileModel.ProjectileKey = $"{Type}_Projectile";
            projectileModel.Damage = damage;
            projectileModel.AttackTeam = TeamType.Enemy;
            rb.velocity = direction * projectileSpeed;
        }
    }
}