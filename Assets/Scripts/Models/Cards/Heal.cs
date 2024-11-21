using Models.AI;
using Services.Cards;
using Services.DependencyInjection;
using Services.Interfaces;
using UnityEngine;

namespace Models.Cards
{
    public class Heal : Card
    {
        public override CardType Type => CardType.Heal;
        [SerializeField] private float heal = 30;

        [Inject] private IObjectPool _objectPool;
        [Inject] private IPlayerService _playerService;
        
        public override void ApplyEffect()
        {
            _playerService.Health += heal;
        }
    }
}