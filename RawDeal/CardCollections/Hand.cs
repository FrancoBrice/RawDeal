using RawDeal.Cards;

namespace RawDeal.CardCollections;

public class Hand : CardCollection
{
    
    public List<string> TypesOfPlayableCards { get; set; }
    public List<(int, Card)> GetTuplesWithPositionInHandAndPlayableCards(int fortitude)
    {
        List<(int, Card)> tuplesWithPositionInHandAndPlayableCards = new List<(int, Card)>();
        TypesOfPlayableCards = new List<string>();
        for (int indexInHand = 0; indexInHand < CardList.Count; indexInHand++)
        {
            Card card = CardList[indexInHand];
            if (card.GetFortitude() <= fortitude && TypeOfCardIsPlayableInTurn(card))
            {
                tuplesWithPositionInHandAndPlayableCards.Add((indexInHand, card));
            }
        }
        return tuplesWithPositionInHandAndPlayableCards;
    }

    public string GetTypeOfPlayedCard(int indexOfPlayableCards)
    {
        return TypesOfPlayableCards[indexOfPlayableCards];
    }


    private bool TypeOfCardIsPlayableInTurn(Card card)
    {
        return card.ItsTypeManeuver || card.IsTypeAction;
    }

    private bool TypeOfCardIsHybrid(Card card)
    {
        return card.IsHybrid;
    }
}