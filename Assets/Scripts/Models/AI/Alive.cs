using Services.DependencyInjection;
using Services.Interfaces;
using UnityEngine;

namespace Models.AI
{
    public class Alive : MonoBehaviour, IDamageable, IResetable
    {
        public virtual float Health
        {
            get => health;
            set
            {
                if (value > 0)
                    health = value;
                else
                    ObjectPool.Destroy(poolKey, gameObject);
            }
        }
        
        
        [SerializeField] protected string poolKey;
        [SerializeField] protected float health = 100;
        [Inject] protected IObjectPool ObjectPool;
        
        public void Reset()
        {
            health = 100;
        }

        public virtual void DealDamage(float damage)
        {
            Health -= damage;
        }

        
    }
}