using System.Collections.Generic;
using Services.Cards;
using Services.DependencyInjection;

namespace Services.Interfaces
{
    public interface ICardController : IInjectable
    {
        List<CardType> Deck { get; }
        List<CardType> Discard { get; }
        
        void DrawCard();
        void PlayCard(int handIndex);
    }
}