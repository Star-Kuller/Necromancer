using Models.AI;
using Services.Cards;
using Services.Interfaces;
using Services.ServiceLocator;
using UnityEngine;

namespace Models.Cards
{
    public class MagicShotStrong : Card
    {
        public override CardType Type => CardType.MagicShotStrong;
        [SerializeField] private GameObject bullet;
        [SerializeField] private float damage = 30;
        [SerializeField] private float bulletSpeed = 5;

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
            
            var newBullet = _objectPool.Create($"{Type}_bullet", bullet);
            newBullet.transform.position = _player.position;
            
            var rb = newBullet.GetComponent<Rigidbody2D>();
            var bulletModel = newBullet.GetComponent<Bullet>();
            bulletModel.BulletKey = $"{Type}_bullet";
            bulletModel.Damage = damage;
            bulletModel.AttackTeam = TeamType.Enemy;
            rb.velocity = direction * bulletSpeed;
        }
    }
}