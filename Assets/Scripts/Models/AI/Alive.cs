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
                if (health > 0)
                    health = value;
                else
                    Destroy(gameObject);
            }
        }
        
        [SerializeField] protected float health = 100;
        
        public virtual void GetDamage(float damage)
        {
            Health -= damage;
        }
    }
}