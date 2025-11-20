using Services.Cards;
using Services.DependencyInjection;
using Services.Interfaces;
using UnityEngine;

namespace Models
{
    public class PickupСard : MonoBehaviour
    {
        [SerializeField] private CardType cardType;
        [Inject] private ICardController _cardController;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.CompareTag("Player")) return;
            _cardController.Deck.Add(cardType);
            Destroy(gameObject);
        }
    }
}
