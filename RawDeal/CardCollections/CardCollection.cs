namespace RawDeal.CardCollections;

public class CardCollection
{
    protected List<Card> CardList;
    public int CardListSize => CardList.Count;

    protected CardCollection()
    {
        CardList = new List<Card>();
    }
    
    public void AddCard(Card card)
    {
        CardList.Add(card);
    }

    public void AddCardAtTheBeginning(Card card)
    {
        CardList.Insert(0, card);
    }
    
    public void RemoveLastCard()
    {
        try
        {
            CardList.RemoveAt(CardListSize - 1);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    public List<string> GetFormattedCards()
    {
        List<string> formattedCards = new List<string>();

        foreach (Card card in CardList)
        {
            formattedCards.Add(card.GetCardFormattedInfo());
        }

        return formattedCards;
    }
    
    public List<Card> GetLastCardsReversed(int numberOfCards)
    {
        List<Card> lastCards = new List<Card>();
        int index = CardListSize - 1;
        while (index >= 0  &&  lastCards.Count < numberOfCards)
        {
            lastCards.Add(CardList[index]);
            index += -1;
        }
        lastCards.Reverse();
        return lastCards;
    }
    public Card GetCardByIndex(int index)
    {
        Card card = CardList[index];
        return card;
    }

    public void RemoveCardByIndex(int index)
    {
            CardList.RemoveAt(index);
    }

    
}