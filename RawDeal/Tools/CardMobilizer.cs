using RawDeal.Cards;
using RawDeal.GameLogic.Players;

namespace RawDeal.Tools;

public static class CardMobilizer
{
    public static void MoveCardFromHandToRingArea(Player player,
        (int, Card) tupleWithIndexInHandAndSelectedCard)
    {
        int indexInHand = TupleManager.ExtractIndex(tupleWithIndexInHandAndSelectedCard);
        player.MoveCardFromHandToRingAreaByIndex(indexInHand);
    }

    public static void MoveSpecificCardFromHandToRingside(Player player,
        (int, Card) tupleWithIndexInHandAndSelectedCard)
    {
        int indexInHand = TupleManager.ExtractIndex(tupleWithIndexInHandAndSelectedCard);
        player.MoveCardFromHandToRingsideByIndex(indexInHand);
    }

    public static void MoveCardsFromArsenalToHand(Player player, int numberOfCards)
    {
        if (player.GetArsenalSize() < 1) return;
        List<Card> drawnCards = player.GetLastCardsFromArsenalReversed(numberOfCards);
        drawnCards.Reverse();
        foreach (Card drawnCard in drawnCards)
        {
            player.AddCardToHand(drawnCard);
            player.RemoveLastCardFromArsenal();
        }
    }

    public static void MoveCardsFromArsenalToRingSideByDamageAmount(Player player,
        int? damageAmount)
    {
        List<Card> cardsList = player.GetLastCardsFromArsenalReversed(damageAmount);
        if (damageAmount >= player.GetArsenalSize()) damageAmount = player.GetArsenalSize();
        for (int? index = damageAmount - 1; index >= 0; index--)
        {
            Card currentCard = cardsList[(int)index];
            player.AddCardToRingside(currentCard);
            player.RemoveLastCardFromArsenal();
        }
    }

    public static void DrawStunValueCards(Player attackingPlayer, int numberOfCardsToDraw)
    {
        for (int i = 0; i < numberOfCardsToDraw; i++) attackingPlayer.MoveCardFromArsenalToHand();
    }
}