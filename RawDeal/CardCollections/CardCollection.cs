using RawDeal.Cards;

namespace RawDeal.CardCollections;

public class CardCollection
{
    public List<Card> CardList;

    public CardCollection()
    {
        CardList = new List<Card>();
    }

    public int CardListSize => CardList.Count;

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
        List<string> formattedCards = new();

        foreach (Card card in CardList) formattedCards.Add(card.GetCardFormattedInfo());

        return formattedCards;
    }

    public List<Card> GetLastCardsReversed(int? numberOfCards)
    {
        List<Card> lastCards = new();
        int index = CardListSize - 1;
        while (index >= 0 && lastCards.Count < numberOfCards)
        {
            lastCards.Add(CardList[index]);
            index += -1;
        }

        lastCards.Reverse();
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
}