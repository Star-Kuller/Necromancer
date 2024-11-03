using Services.ServiceLocator;
using UnityEngine;

namespace Services.Interfaces
{
    public interface IPlayerManager : IService
    {
        public float Health { get; set; }
        public float Mana { get; set; }
        public Transform PlayerTransform { get; }
    }
}