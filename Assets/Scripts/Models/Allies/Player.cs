using System;
using System.Collections;
using Models.AI;
using Services.DependencyInjection;
using Services.EventBus;
using Services.Interfaces;
using UnityEngine;

namespace Models.Allies
{
    public class Player : Alive, IPlayerService, IAutoRegistration
    {
        public override float Health
        {
            get => health;
            set
            {
                if(value < health)
                    _eventBus.CallEvent(GameEvent.PlayerReceivedDamage);
                if(value > health)
                    _eventBus.CallEvent(GameEvent.PlayerHealed);

                if (value > 0)
                {
                    health = value > _maxHealth ? _maxHealth : value;
                }
                else
                {
                    health = 0;
                    _eventBus.CallEvent(GameEvent.PlayerDead);
                }
            }
        }
        
        public float Mana
        {
            get => mana;
            set
            {
                if (value > 0)
                    mana = value;
                else
                    throw new ArgumentException("Мана не может быть меньше 0.");
            }
        }

        public Transform PlayerTransform => transform;
        
        [SerializeField] private float mana = 30;
        [SerializeField] private float manaPerSecond = 0.5f;

        [Inject] private IEventBus _eventBus;
        private float _maxHealth;
        private float _maxMana;
        
        public void Register()
        {
            this.Register<IPlayerService>();
        }

        private void Awake()
        {
            _maxHealth = health;
            _maxMana = mana;
        }

        private void Start()
        {
            StartCoroutine(ManaRestore());
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
                _eventBus.CallEvent(GameEvent.NotEnoughMana);
                return false;
            }
        }

        private IEnumerator ManaRestore()
        {
            while (true)
            {
                yield return new WaitUntil(() => mana < _maxMana);
                yield return new WaitForSeconds(1);
                mana += manaPerSecond < _maxMana - mana 
                    ? manaPerSecond 
                    : _maxMana - mana;
            }
        }
        
    }
}