using System.Diagnostics.CodeAnalysis;

namespace RawDeal;

public class Hand : CardCollection
{
    public List<(int, Card)> GetPlayableCards(int fortitude)
    {
        List<(int, Card)> playableCards = new List<(int, Card)>();
        for (int index = 0; index < CardList.Count; index++)
        {
            Card card = CardList[index];
            if (card.Fortitude <= fortitude && CheckIfTypeOfCardIsPlayable(card))
            {
                playableCards.Add((index, card));
            }
        }

        return playableCards;
    }

    private bool CheckIfTypeOfCardIsPlayable(Card card)
    {

        foreach (string type in card.Types)
        {
            if (type is "Maneuver" or "Action")
            {
                return true;
            }
        }
        return false;
    }


}