using Services.Cards;
using Services.DependencyInjection;
using Services.Interfaces;
using UnityEngine;

namespace Models.Cards
{
    public class SummonSkeletons : Card
    {
        public override CardType Type => CardType.SummonSkeletons;
        [SerializeField] private GameObject skeleton;
        
        [Inject] private IObjectPool _objectPool;
        private Transform _player;

        [Inject]
        private void Init(IPlayerService playerService)
        {
            _player = playerService.PlayerTransform;
        }
        
        public override void ApplyEffect()
        {
            var positionSkeletonOne = _player.position + Vector3.right * 2;
            var positionSkeletonTwo = _player.position + Vector3.left  * 2;

            var skeletonOne = _objectPool.Create($"{Type.ToString()}_Skeletons", skeleton);
            var skeletonTwo = _objectPool.Create($"{Type.ToString()}_Skeletons", skeleton);

            skeletonOne.transform.position = positionSkeletonOne;
            skeletonTwo.transform.position = positionSkeletonTwo;
        }
    }
}
