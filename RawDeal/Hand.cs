namespace RawDeal;

public class Hand : CardCollection
{
    public List<(int, Card)> GetPlayableCards(int fortitude)
    {
        List<(int, Card)> playableCards = new List<(int, Card)>();
        for (int index = 0; index < CardList.Count; index++)
        {
            Card card = CardList[index];
            if (card.Fortitude <= fortitude)
            {
                playableCards.Add((index, card));
            }
        }

        return playableCards;
    }



}