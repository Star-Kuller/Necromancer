using Models.AI;
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
        [SerializeField] private string poolKey = "Skeleton";
        private Transform _player;

        [Inject]
        private void Init(IPlayerService playerService)
        {
            _player = playerService.PlayerTransform;
        }
        
        public override void ApplyEffect()
        {
            SpawnSkeletons(Vector3.right, Vector3.left);
        }

        private void SpawnSkeletons(params Vector3[] offsets)
        {
            foreach (var offset in offsets)
            {
                SpawnSkeleton(_player.position + offset);
            }
        }

        private void SpawnSkeleton(Vector3 position)
        {
            var skeletonObject = _objectPool.Create(poolKey, skeleton);
            var skeletonAI = skeletonObject.GetComponent<DefencePointAI>();
            skeletonAI.DefencePoint = _player;

            skeletonObject.transform.position = position;
        }
    }
}
