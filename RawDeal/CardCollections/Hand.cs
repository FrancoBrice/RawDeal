using RawDeal.Cards;
using RawDeal.Cards.CardPreConditions;
using RawDeal.GameLogic;

namespace RawDeal.CardCollections;

public class Hand : CardCollection
{
    private List<string> TypesOfPlayableCards { get; set; }
    public List<(int, Card)> GetTuplesWithPositionInHandAndPlayableCards(PlayManager playManager, int? fortitude)
    {
        List<(int, Card)> tuplesWithPositionInHandAndPlayableCards = new List<(int, Card)>();
        TypesOfPlayableCards = new List<string>();
        for (int indexInHand = 0; indexInHand < CardList.Count; indexInHand++)
        {
            Card card = CardList[indexInHand];
            if (card.GetCurrentFortitude("") <= fortitude && card.TypeIsPlayable() && CardPreConditionChecker.IsPlayableCard(card, playManager))
            {
                tuplesWithPositionInHandAndPlayableCards.Add((indexInHand, card));
            }
        }
        return tuplesWithPositionInHandAndPlayableCards;
    }
    
    public List<(int, Card)> GetTuplesWithPositionInHandAndReversalCards(Player player, PlayManager playManager)
    {
        List<(int, Card)> tuplesWithPositionInHandAndReversalCards = new List<(int, Card)>();
        TypesOfPlayableCards = new List<string>();
        for (int indexInHand = 0; indexInHand < CardList.Count; indexInHand++)
        {
            Card reversalCard = CardList[indexInHand];
            if (ReversalsChecker.IsCorrectReversalCard(playManager, reversalCard, "Hand"))
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
}