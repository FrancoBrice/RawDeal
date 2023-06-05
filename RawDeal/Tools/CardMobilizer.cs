using RawDeal.Cards;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.GameLogic;

public class CardMobilizer
{
    private TupleManager _tupleManager;
    private View _view; 
    
    public CardMobilizer(View view)
    {
        _tupleManager = new TupleManager();
        _view = view;
    }
    
    public void MoveCardFromHandToRingArea(Player player, (int, Card) tupleWithIndexInHandAndSelectedCard)
    {
        int indexInHand = _tupleManager.ExtractCardIndexInHand(tupleWithIndexInHandAndSelectedCard);
        player.MoveCardFromHandToRingAreaByIndex(indexInHand);
    }

    public void MoveSpecificCardFromHandToRingside(Player player, (int, Card) tupleWithIndexInHandAndSelectedCard)
    {
        int indexInHand = _tupleManager.ExtractCardIndexInHand(tupleWithIndexInHandAndSelectedCard);
        player.MoveCardFromHandToRingsideByIndex(indexInHand);
    }

    public void MakePlayerDiscardCards(Player player, int numberOfCardsToDiscard)
    {
        for (int remainingCardsToDiscard = numberOfCardsToDiscard; remainingCardsToDiscard > 0; remainingCardsToDiscard--)
        {
            int indexCardFromPlayerHand = _view.AskPlayerToSelectACardToDiscard(player.GetCardsInStringFormatFromHand(), player.GetSuperStarName(),
                player.GetSuperStarName(), remainingCardsToDiscard);
            player.MoveCardFromHandToRingsideByIndex(indexCardFromPlayerHand);
        }
    }
    
    public void MoveCardsReversedFromArsenalToHand(Player player, int numberOfCards)
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
    
    public void MoveCardsFromArsenalToHand(Player player, int numberOfCards)
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
    
    public void MoveCardsFromArsenalToRingSideByDamageAmount(Player player, int? damageAmount)
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

    public void DrawStunValueCards(Player attackingPlayer, int numberOfCardsToDraw)
    {
        for (int i = 0; i < numberOfCardsToDraw; i++)
        {
            attackingPlayer.MoveCardFromArsenalToHand();
        }
    }
}