using RawDeal.CardCollections;
using RawDeal.CardCollections.SubClasses;
using RawDeal.Cards;
using RawDeal.GameLogic;
using RawDeal.GameLogic.Players;

namespace RawDeal.Tools;

public static class CardMobilizer
{
    
    public static void DistributeOpeningHand(Player player, int superstarHandSize)
    {
        Hand hand = player.Hand;
        Arsenal arsenal = player.Arsenal;
        CardCollection drawnCards = arsenal.GetLastCardsReversed(superstarHandSize);
        for (int index = drawnCards.Count - 1; index >= 0; index--)
        {
            hand.Add(drawnCards.CardList[index]);
            arsenal.RemoveLastCard();
        }
    }

    private static void MoveFromHandToRingAreaByIndex(Player player, int index)
    {
        Hand hand = player.Hand;
        RingArea ringArea = player.RingArea;
        Card card = hand.GetCardByIndex(index);
        hand.RemoveCardByIndex(index);
        ringArea.Add(card);
    }

    public static void MoveFromRingAreaToRingSideByIndex(Player player, int index)
    {
        RingArea ringArea = player.RingArea;
        Ringside ringside = player.Ringside;
        Card card = ringArea.GetCardByIndex(index);
        ringArea.RemoveCardByIndex(index);
        ringside.Add(card);
    }

    public static void MoveFromHandToArsenalByIndex(Player player, int index)
    {
        Hand hand = player.Hand;
        Arsenal arsenal = player.Arsenal;
        Card card = hand.GetCardByIndex(index);
        hand.RemoveCardByIndex(index);
        arsenal.Add(card);
    }
    

    public static void MoveFromRingsideToArsenalBeginningByIndex(Player player, int index)
    {
        Ringside ringside = player.Ringside;
        Arsenal arsenal = player.Arsenal;
        Card card = ringside.GetCardByIndex(index);
        ringside.RemoveCardByIndex(index);
        arsenal.AddCardAtTheBeginning(card);
    }

    public static void MoveFromHandToRingsideByIndex(Player player, int indexCardFromHand)
    {
        Hand hand = player.Hand;
        Ringside ringside = player.Ringside;
        Card card = hand.GetCardByIndex(indexCardFromHand);
        hand.RemoveCardByIndex(indexCardFromHand);
        ringside.Add(card);
    }
    
    public static void MoveFromRingsideToHandByIndex(Player player, int indexCardFromRingside)
    {
        Hand hand = player.Hand;
        Ringside ringside = player.Ringside;
        Card card = ringside.GetCardByIndex(indexCardFromRingside);
        ringside.RemoveCardByIndex(indexCardFromRingside);
        hand.Add(card);
    }
    
    public static void MoveFromArsenalToHandByIndex(Player player, int indexCardFromRingside)
    {
        Hand hand = player.Hand;
        Arsenal arsenal = player.Arsenal;
        Card card = arsenal.GetCardByIndex(indexCardFromRingside);
        arsenal.RemoveCardByIndex(indexCardFromRingside);
        hand.Add(card);
    }

    public static void MoveFromHandToArsenalBeginningByIndex(Player player, int indexFromHand)
    {
        Hand hand = player.Hand;
        Arsenal arsenal = player.Arsenal;
        Card card = hand.GetCardByIndex(indexFromHand);
        hand.RemoveCardByIndex(indexFromHand);
        arsenal.AddCardAtTheBeginning(card);
    }
    
    public static void MoveFromHandToRingArea(Player player,
        IndexedCard indexedCardWithIndexInHandAndSelectedCard)
    {
        int indexInHand = indexedCardWithIndexInHandAndSelectedCard.Index;
        MoveFromHandToRingAreaByIndex(player, indexInHand);
    }

    public static void MoveSpecificCardFromHandToRingside(Player player,
        IndexedCard indexedCardWithIndexInHandAndSelectedCard)
    {
        int indexInHand = indexedCardWithIndexInHandAndSelectedCard.Index;
        MoveFromHandToRingsideByIndex(player, indexInHand);
    }

    public static void MoveFromArsenalToHandByAmount(Player player, int numberOfCards)
    {
        Arsenal arsenal  = player.Arsenal;
        Hand hand = player.Hand;
        if (player.GetArsenalSize() < 1) return;
        CardCollection drawnCards = arsenal.GetLastCards(numberOfCards);
        foreach (Card drawnCard in drawnCards)
        {
            hand.Add(drawnCard);
            arsenal.RemoveLastCard();
        }
    }
    
    public static void MoveFromArsenalToHand(Player player)
    {
        Arsenal arsenal = player.Arsenal;
        Hand hand = player.Hand;
        if (arsenal.Count < 1) return;
        CardCollection drawnCards = arsenal.GetLastCardsReversed(1);
        Card drawnCard = drawnCards.CardList[0];
        hand.Add(drawnCard);
        arsenal.RemoveLastCard();
    }

    public static void MoveFromArsenalToRingsideByDamageAmount(Player player,
        int? damageAmount)
    {
        Arsenal arsenal = player.Arsenal;
        Ringside ringside = player.Ringside;
        CardCollection cards = arsenal.GetLastCardsReversed(damageAmount);
        if (damageAmount >= player.GetArsenalSize()) damageAmount = player.GetArsenalSize();
        for (int? index = damageAmount - 1; index >= 0; index--)
        {
            Card currentCard = cards.CardList[(int)index];
            ringside.Add(currentCard);
            arsenal.RemoveLastCard();
        }
    }

    public static void DrawStunValueCards(Player attackingPlayer, int numberOfCardsToDraw)
    {
        for (int i = 0; i < numberOfCardsToDraw; i++) MoveFromArsenalToHand(attackingPlayer);
    }
    
}