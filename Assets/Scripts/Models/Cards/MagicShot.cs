using Services.Cards;
using UnityEngine;

namespace Models.Cards
{
    public class MagicShot : Card
    {
        public override int Cost => 15;
        public override CardType Type => CardType.MagicShot;
        
        public override void ApplyEffect()
        {
            Debug.Log("Каст магического выстрела!");
        }
    }
}