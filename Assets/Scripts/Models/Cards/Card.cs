using System;
using Services.Cards;
using TMPro;
using UnityEngine;

namespace Models.Cards
{
    public abstract class Card : MonoBehaviour
    {
        [SerializeField] private int cost;
        public int Cost => cost;
        public abstract CardType Type { get; }
        public abstract void ApplyEffect();

        private void Start()
        {
            var costTransform = transform.Find("Cost");
            var costText = costTransform.GetComponent<TMP_Text>();
            costText.text = cost.ToString();
        }
    }
}