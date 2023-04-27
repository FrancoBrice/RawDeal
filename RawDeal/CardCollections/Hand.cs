namespace RawDeal.CardCollections;

public class Hand : CardCollection
{
    public List<(int, Card)> GetTuplesWithPositionInHandAndPlayableCards(int fortitude)
    {
        List<(int, Card)> tuplesWithPositionInHandAndPlayableCards = new List<(int, Card)>();
        for (int indexInHand = 0; indexInHand < CardList.Count; indexInHand++)
        {
            Card card = CardList[indexInHand];
            if (card.GetFortitude() <= fortitude && CheckIfTypeOfCardIsPlayable(card))
            {
                tuplesWithPositionInHandAndPlayableCards.Add((indexInHand, card));
            }
        }
        return tuplesWithPositionInHandAndPlayableCards;
    }

    private bool CheckIfTypeOfCardIsPlayable(Card card)
    {
        if (card.ItsTypeManeuver() || card.IsTypeAction())
        {
            return true;
        }
        return false;
    }
    
}