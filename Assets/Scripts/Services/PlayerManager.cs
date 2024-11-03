using System;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class PlayerManager : MonoBehaviour, IPlayerManager
    {
        public float Health
        {
            get => health;
            set
            {
                if (health >= 0)
                    health = value;
                else
                    throw new ArgumentException("Здоровье не может быть меньше 0.");
            }
        }

        public float Mana
        {
            get => mana;
            set
            {
                if (mana >= 0)
                    mana = value;
                else
                    throw new ArgumentException("Мана не может быть меньше 0.");
            }
        }

        public Transform PlayerTransform => transform;
        
        [SerializeField] private float health = 100;
        [SerializeField] private float mana = 20;

        private void Awake()
        {
            var services = ServiceLocator.ServiceLocator.Current;
            services.Register<IPlayerManager>(this);
        }
    }
}