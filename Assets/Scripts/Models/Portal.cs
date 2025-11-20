using System;
using Services.DependencyInjection;
using Services.EventBus;
using Services.Interfaces;
using UnityEngine;

namespace Models
{
    public class Portal : MonoBehaviour
    {
        private SpriteRenderer _render;
        [SerializeField] private GameObject buttons;
        private bool _isBossDead = false;
        public void Awake()
        {
            _render = GetComponent<SpriteRenderer>();
        }
        
        [Inject]
        private void Init(IEventBus eventBus)
        {
            eventBus.Subscribe(GameEvent.BossDead, _ =>
            {
                _isBossDead = true;
                _render.enabled = true;
            });
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.CompareTag("Player") || !_isBossDead) return;
            buttons.SetActive(true);
        }
    }
}
