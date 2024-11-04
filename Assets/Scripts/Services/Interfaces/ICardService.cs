using System.Collections.Generic;
using Services.Cards;
using Services.ServiceLocator;

namespace Services.Interfaces
{
    public interface ICardService : IService
    {
        List<CardType> Deck { get; }
        List<CardType> Discard { get; }
        
        void DrawCard();
        void PlayCard(int handIndex);
    }
}