using Models.AI;
using Services.Cards;
using Services.Interfaces;
using Services.ServiceLocator;
using UnityEngine;

namespace Models.Cards
{
    public class BallLightning : Card
    {
        public override CardType Type => CardType.BallLightning;
        [SerializeField] private GameObject projectile;
        [SerializeField] private float damage = 30;
        [SerializeField] private float projectileSpeed = 5;

        private IObjectPool _objectPool;
        private Transform _player;
        private Camera _mainCamera;

        
        private void Start()
        {
            var services = ServiceLocator.Current;
            _objectPool = services.Get<IObjectPool>();
            _player = services.Get<IPlayerService>().PlayerTransform;
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