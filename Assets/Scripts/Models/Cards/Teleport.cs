using Models.AI;
using Services.Cards;
using Services.DependencyInjection;
using Services.Interfaces;
using UnityEngine;

namespace Models.Cards
{
    public class Teleport : Card
    {
        public override CardType Type => CardType.Teleport;
        [SerializeField] private float maxDistance = 5f;

        [Inject] private IObjectPool _objectPool;
        private Transform _player;
        private Camera _mainCamera;

        [Inject]
        private void Init(IPlayerService playerService)
        {
            _player = playerService.PlayerTransform;
            _mainCamera = Camera.main; 
        }
        
        public override void ApplyEffect()
        {
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; 
            var offset = (mousePosition - _player.position);
            var direction = offset.normalized;
            var distance = offset.magnitude;

            _player.position += Mathf.Clamp(distance, 1f, maxDistance) * direction;
        }
    }
}