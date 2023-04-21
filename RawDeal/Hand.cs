using System.Diagnostics.CodeAnalysis;

namespace RawDeal;

public class Hand : CardCollection
{
    public List<Card> GetPlayableCards(int fortitude)
    {
        List<Card> playableCards = new List<Card>();
        for (int index = 0; index < CardList.Count; index++)
        {
            Card card = CardList[index];
            Console.WriteLine($"id actual{card.Id}, index {index}");
            if (card.Fortitude <= fortitude && CheckIfTypeOfCardIsPlayable(card))
            {
                Console.WriteLine($"getplayable: {card.Title}, {card.Id}");
                playableCards.Add(card);
            }
        }
        return playableCards;
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