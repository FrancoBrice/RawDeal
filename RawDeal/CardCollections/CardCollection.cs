using System.Collections;
using RawDeal.Cards;

namespace RawDeal.CardCollections;

public class CardCollection : IEnumerable<Card>
{
    public List<Card> CardList;

    public CardCollection()
    {
        CardList = new List<Card>();
    }

    public int Count => CardList.Count;

    public Card GetPenultimateCard
    {
        get
        {
            try
            {
                if (CardList.Count >= 2) return CardList[^2];
                throw new InvalidOperationException("Insufficient number of cards.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }

            throw new InvalidOperationException("Insufficient number of cards.");
        }
    }

    public void Add(Card card)
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
            CardList.RemoveAt(Count - 1);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public List<string> GetFormattedCards()
    {
        List<string> formattedCards = new List<string>();
        foreach (Card card in CardList) formattedCards.Add(card.GetCardFormattedInfo());
        return formattedCards;
    }

    public CardCollection GetLastCards(int? numberOfCards)
    {
        CardCollection lastCards = new CardCollection();
        int index = Count - 1;
        while (index >= 0 && lastCards.Count < numberOfCards)
        {
            lastCards.Add(CardList[index]);
            index += -1;
        }
        return lastCards;
    }
    
    public CardCollection GetLastCardsReversed(int? numberOfCards)
    {
        CardCollection lastCards = GetLastCards(numberOfCards);
        lastCards.CardList.Reverse();
        return lastCards;
    }

    public Card GetLastCard()
    {
        try
        {
            if (CardList.Count == 0)
                throw new InvalidOperationException("La lista de cartas está vacía.");
            return CardList[^1];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerator<Card> GetEnumerator()
    {
        return CardList.GetEnumerator();
    }

    
}