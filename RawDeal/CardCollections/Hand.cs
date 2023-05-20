using RawDeal.Cards;

namespace RawDeal.CardCollections;

public class Hand : CardCollection
{
    
    public List<string> TypesOfPlayableCards { get; set; }
    public List<(int, Card)> GetTuplesWithPositionInHandAndPlayableCards(int? fortitude)
    {
        List<(int, Card)> tuplesWithPositionInHandAndPlayableCards = new List<(int, Card)>();
        TypesOfPlayableCards = new List<string>();
        for (int indexInHand = 0; indexInHand < CardList.Count; indexInHand++)
        {
            Card card = CardList[indexInHand];
            if (card.GetCurrentFortitude() <= fortitude && card.TypeIsPlayable())
            {
                tuplesWithPositionInHandAndPlayableCards.Add((indexInHand, card));
            }
        }
        return tuplesWithPositionInHandAndPlayableCards;
    }
    
    public List<(int, Card)> GetTuplesWithPositionInHandAndReversalCards(Player player, Card attackingCard)
    {
        List<(int, Card)> tuplesWithPositionInHandAndReversalCards = new List<(int, Card)>();
        TypesOfPlayableCards = new List<string>();
        for (int indexInHand = 0; indexInHand < CardList.Count; indexInHand++)
        {
            Card reversalCard = CardList[indexInHand];

            if (player.IsCorrectReversalCard(attackingCard, reversalCard))
            {
                tuplesWithPositionInHandAndReversalCards.Add((indexInHand, reversalCard));
            }
        }

        return tuplesWithPositionInHandAndReversalCards;
    }


    public string GetTypeOfPlayedCard(int indexOfPlayableCards)
    {
        return TypesOfPlayableCards[indexOfPlayableCards];
    }
    
    private bool TypeOfCardIsHybrid(Card card)
    {
        return card.IsHybrid;
    }
}