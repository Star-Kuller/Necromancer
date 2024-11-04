using System;
using Services.Interfaces;
using Services.ServiceLocator;
using UnityEngine;

namespace Models.AI
{
    public class Alive : MonoBehaviour, IDamageable
    {
        public virtual float Health
        {
            get => health;
            set
            {
                if (value > 0)
                    health = value;
                else
                    ObjectPool.Destroy(name, gameObject);
            }
        }
        
        
        [SerializeField] protected string name;
        [SerializeField] protected float health = 100;
        protected IObjectPool ObjectPool;
        
        public virtual void GetDamage(float damage)
        {
            Health -= damage;
        }
        
        private void Start()
        {
            var services = ServiceLocator.Current;
            ObjectPool = services.Get<IObjectPool>();
        }
    }
}