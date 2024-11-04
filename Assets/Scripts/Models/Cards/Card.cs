using Services.Cards;
using UnityEngine;

namespace Models.Cards
{
    public abstract class Card : MonoBehaviour
    {
        public abstract int Cost { get; } 
        public abstract CardType Type { get; }
        public abstract void ApplyEffect();
    }
}