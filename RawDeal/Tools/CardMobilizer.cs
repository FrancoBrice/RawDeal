using RawDeal.CardCollections;
using RawDeal.Cards;
using RawDeal.GameLogic.Players;

namespace RawDeal.Tools;

public static class CardMobilizer
{
    
    public static void DistributeOpeningHand(Player player, int superstarHandSize)
    {
        Hand hand = player.Hand;
        Arsenal arsenal = player.Arsenal;
        List<Card> drawnCards = player.Arsenal.GetLastCardsReversed(superstarHandSize);
        for (int index = drawnCards.Count - 1; index >= 0; index--)
        {
            hand.AddCard(drawnCards[index]);
            arsenal.RemoveLastCard();
        }
    }

    private static void FromHandToRingAreaByIndex(Player player, int index)
    {
        Hand hand = player.Hand;
        RingArea ringArea = player.RingArea;
        Card card = hand.GetCardByIndex(index);
        hand.RemoveCardByIndex(index);
        ringArea.AddCard(card);
    }

    public static void FromRingsideToArsenalBeginningByIndex(Player player, int index)
    {
        Ringside ringside = player.Ringside;
        Arsenal arsenal = player.Arsenal;
        Card card = ringside.GetCardByIndex(index);
        ringside.RemoveCardByIndex(index);
        arsenal.AddCardAtTheBeginning(card);
    }

    public static void FromHandToRingsideByIndex(Player player, int indexCardFromHand)
    {
        Hand hand = player.Hand;
        Ringside ringside = player.Ringside;
        Card card = hand.GetCardByIndex(indexCardFromHand);
        hand.RemoveCardByIndex(indexCardFromHand);
        ringside.AddCard(card);
    }

    public static void FromRingsideToHandByIndex(Player player, int indexCardFromRingside)
    {
        Hand hand = player.Hand;
        Ringside ringside = player.Ringside;
        Card card = ringside.GetCardByIndex(indexCardFromRingside);
        ringside.RemoveCardByIndex(indexCardFromRingside);
        hand.AddCard(card);
    }

    public static void FromHandToArsenalBeginningByIndex(Player player, int indexFromHand)
    {
        Hand hand = player.Hand;
        Arsenal arsenal = player.Arsenal;
        Card card = hand.GetCardByIndex(indexFromHand);
        hand.RemoveCardByIndex(indexFromHand);
        arsenal.AddCardAtTheBeginning(card);
    }
    public static void FromHandToRingArea(Player player,
        (int, Card) tupleWithIndexInHandAndSelectedCard)
    {
        int indexInHand = TupleManager.ExtractIndex(tupleWithIndexInHandAndSelectedCard);
        FromHandToRingAreaByIndex(player, indexInHand);
    }

    public static void SpecificCardFromHandToRingside(Player player,
        (int, Card) tupleWithIndexInHandAndSelectedCard)
    {
        int indexInHand = TupleManager.ExtractIndex(tupleWithIndexInHandAndSelectedCard);
        FromHandToRingsideByIndex(player, indexInHand);
    }

    public static void FromArsenalToHand(Player player, int numberOfCards)
    {
        Arsenal arsenal  = player.Arsenal;
        Hand hand = player.Hand;
        if (player.GetArsenalSize() < 1) return;
        List<Card> drawnCards = arsenal.GetLastCards(numberOfCards);
        foreach (Card drawnCard in drawnCards)
        {
            hand.AddCard(drawnCard);
            arsenal.RemoveLastCard();
        }
    }
    
    public static void FromArsenalToHand(Player player)
    {
        Arsenal arsenal = player.Arsenal;
        Hand hand = player.Hand;
        if (arsenal.CardListSize < 1) return;
        List<Card> drawnCards = arsenal.GetLastCardsReversed(1);
        Card drawnCard = drawnCards[0];
        hand.AddCard(drawnCard);
        arsenal.RemoveLastCard();
    }

    public static void FromArsenalToRingSideByDamageAmount(Player player,
        int? damageAmount)
    {
        Arsenal arsenal = player.Arsenal;
        Ringside ringside = player.Ringside;
        List<Card> cardsList = arsenal.GetLastCardsReversed(damageAmount);
        if (damageAmount >= player.GetArsenalSize()) damageAmount = player.GetArsenalSize();
        for (int? index = damageAmount - 1; index >= 0; index--)
        {
            Card currentCard = cardsList[(int)index];
            ringside.AddCard(currentCard);
            arsenal.RemoveLastCard();
        }
    }

    public static void DrawStunValueCards(Player attackingPlayer, int numberOfCardsToDraw)
    {
        for (int i = 0; i < numberOfCardsToDraw; i++) FromArsenalToHand(attackingPlayer);
    }
    
}