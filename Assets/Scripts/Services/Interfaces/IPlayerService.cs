using Services.ServiceLocator;
using UnityEngine;

namespace Services.Interfaces
{
    public interface IPlayerService : IService
    {
        public float Health { get; set; }
        public float Mana { get; set; }
        public Transform PlayerTransform { get; }
    }
}