using RawDeal.Cards;
using RawDeal.GameLogic.Players;
using RawDealView;

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

    public static void MakePlayerDiscardCards(View view, Player player, int numberOfCardsToDiscard)
    {
        for (int remainingCardsToDiscard = numberOfCardsToDiscard;
             remainingCardsToDiscard > 0;
             remainingCardsToDiscard--)
        {
            int indexCardFromPlayerHand = view.AskPlayerToSelectACardToDiscard(
                player.GetCardsInStringFormatFromHand(), player.GetSuperStarName(),
                player.GetSuperStarName(), remainingCardsToDiscard);
            player.MoveCardFromHandToRingsideByIndex(indexCardFromPlayerHand);
        }
    }

    public static void MoveCardsReversedFromArsenalToHand(Player player, int numberOfCards)
    {
        if (player.GetArsenalSize() >= 1)
        {
            List<Card> drawnCards = player.GetLastCardsFromArsenalReversed(numberOfCards);
            foreach (Card drawnCard in drawnCards)
            {
                player.AddCardToHand(drawnCard);
                player.RemoveLastCardFromArsenal();
            }
        }
    }

    public static void MoveCardsFromArsenalToHand(Player player, int numberOfCards)
    {
        if (player.GetArsenalSize() >= 1)
        {
            List<Card> drawnCards = player.GetLastCardsFromArsenalReversed(numberOfCards);
            drawnCards.Reverse();
            foreach (Card drawnCard in drawnCards)
            {
                player.AddCardToHand(drawnCard);
                player.RemoveLastCardFromArsenal();
            }
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