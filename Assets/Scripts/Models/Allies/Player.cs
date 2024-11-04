using System;
using Models.AI;
using Services.EventBus;
using Services.Interfaces;
using Services.ServiceLocator;
using UnityEngine;

namespace Models.Allies
{
    public class Player : Alive, IPlayerService
    {
        public override float Health
        {
            get => health;
            set
            {
                if(value < health)
                    _eventBus.CallEvent(EventList.PlayerReceivedDamage);
                
                if (health > 0)
                    health = value;
                else
                    _eventBus.CallEvent(EventList.PlayerDead);

                if (health < 0)
                    health = 0;
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
        
        [SerializeField] private float mana = 20;

        private IEventBus _eventBus;

        private void Awake()
        {
            var services = ServiceLocator.Current;
            services.TryRegister<IPlayerService>(this);
        }

        private void Start()
        {
            var services = ServiceLocator.Current;
            _eventBus = services.Get<IEventBus>();
        }

        public bool SpentMana(float value)
        {
            try
            {
                Mana -= value;
                return true;
            }
            catch (ArgumentException e)
            {
                return false;
            }
        }
    }
}