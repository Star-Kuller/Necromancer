using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models.Cards;
using Services.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Services.Cards
{
    
    public class CardService : MonoBehaviour, ICardService
    {
        public List<CardType> Deck { get; private set; } = new();
        public List<CardType> Discard { get; private set; } = new();
        [SerializeField] private float drawDelay = 10f; 
        [SerializeField] private int maxHandSize = 5;
        [SerializeField] private Transform hand;
        [SerializeField] private List<CardType> initDeck;
        [SerializeField] private List<CardType> cardKey;
        [SerializeField] private List<GameObject> cardValue;

        private IPlayerService _player;
        private Dictionary<CardType, GameObject> _cards;

        private void Awake()
        {
            var services = ServiceLocator.ServiceLocator.Current;
            services.TryRegister<ICardService>(this);
        }

        private void Start()
        {
            Deck = initDeck;
            _cards = new Dictionary<CardType, GameObject>(
                cardKey.Select(
                    (key, i) 
                        => new KeyValuePair<CardType, GameObject>(key, cardValue[i])
                    )
                );
            DrawInitialHand();
            StartCoroutine(DrawCardsWithDelay());
            
            var services = ServiceLocator.ServiceLocator.Current;
            _player = services.Get<IPlayerService>();
        }
        
        
        
        private void ShuffleDeck()
        {
            Deck = Deck.OrderBy(x => Random.value).ToList();
        }

        private void DrawInitialHand()
        {
            while (hand.childCount < maxHandSize && Deck.Count > 0)
            {
                DrawCard();
            }
        }

        public void DrawCard()
        {
            if (Deck.Count == 0)
            {
                ReshuffleDiscard();
            }

            if (Deck.Count > 0)
            {
                var cardType = Deck[0];
                Deck.RemoveAt(0);
                Instantiate(_cards[cardType], hand);
            }
        }

        private void ReshuffleDiscard()
        {
            Deck.AddRange(Discard);
            Discard.Clear();
            ShuffleDeck();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void PlayCard(int handIndex)
        {
            if (handIndex < 0 || handIndex >= hand.childCount) return;
            
            var cardTransform = hand.GetChild(handIndex);
            var card = cardTransform.GetComponent<Card>();
            if(!_player.SpentMana(card.Cost)) return;
            
            card.ApplyEffect();
            Discard.Add(card.Type);
            Destroy(cardTransform.gameObject);
        }
        
        private IEnumerator DrawCardsWithDelay()
        {
            while (true)
            {
                yield return new WaitUntil(() => hand.childCount < maxHandSize);
                yield return new WaitForSeconds(drawDelay);
                DrawCard();
                
            }
        }

        private void Update()
        {
            // Обработка ввода для игры карт
            if (Input.GetKeyDown(KeyCode.Alpha1)) PlayCard(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) PlayCard(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) PlayCard(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) PlayCard(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) PlayCard(4);
        }
    
    }
}