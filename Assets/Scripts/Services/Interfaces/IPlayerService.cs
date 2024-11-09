using Services.DependencyInjection;
using UnityEngine;

namespace Services.Interfaces
{
    public interface IPlayerService : IInjectable
    {
        public float Health { get; set; }
        public float Mana { get; set; }
        public Transform PlayerTransform { get; }
        bool SpentMana(float value);
    }
}