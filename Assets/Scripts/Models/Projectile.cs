using System.Collections;
using Models.AI;
using Services.DependencyInjection;
using Services.Interfaces;
using UnityEngine;

namespace Models
{
    public class Projectile : MonoBehaviour, IResetable
    {
        public string ProjectileKey { get; set; }
        public float Damage { get; set; }
        public TeamType AttackTeam { get; set; } = TeamType.Enemy;
        [SerializeField] private float ttl; 
        [Inject] private IObjectPool _objectPool;

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
            _objectPool.Destroy(ProjectileKey, gameObject);
        }

        public void Reset()
        {
            Damage = 0;
            AttackTeam = TeamType.Enemy;
        }
        
        private IEnumerator LifeCycle()
        {
            yield return new WaitForSeconds(ttl);
            _objectPool.Destroy(ProjectileKey, gameObject);
        }
    }
}
