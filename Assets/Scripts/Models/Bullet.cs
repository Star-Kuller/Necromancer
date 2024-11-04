using System;
using System.Collections;
using Models.AI;
using Services.Interfaces;
using Services.ServiceLocator;
using UnityEngine;

namespace Models
{
    public class Bullet : MonoBehaviour, IResetable
    {
        public string BulletKey { get; set; }
        public float Damage { get; set; }
        public TeamType AttackTeam { get; set; } = TeamType.Enemy;
        [SerializeField] private float ttl; 
        private IObjectPool _objectPool;
        
        private void Start()
        {
            var services = ServiceLocator.Current;
            _objectPool = services.Get<IObjectPool>();
        }

        private void OnEnable()
        {
            StartCoroutine(LifeCycle());
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform.CompareTag(AttackTeam.ToString()))
            {
                var damageable = other.transform.GetComponent<IDamageable>();
                damageable.GetDamage(Damage);
            }
            _objectPool.Destroy(BulletKey, gameObject);
        }

        public void Reset()
        {
            BulletKey = "";
            Damage = 0;
            AttackTeam = TeamType.Enemy;
        }
        
        private IEnumerator LifeCycle()
        {
            yield return new WaitForSeconds(ttl);
            _objectPool.Destroy(name, gameObject);
        }
    }
}
