using RawDeal.Cards;

namespace RawDeal.GameLogic;

public class CardMobilizer
{
    private TupleManager _tupleManager;
    
    public CardMobilizer()
    {
        _tupleManager = new TupleManager();
    }
    
    public void MoveCardFromHandToRingArea(Player player, (int, Card) tupleWithIndexInHandAndSelectedCard)
    {
        int indexInHand = _tupleManager.ExtractCardIndexInHand(tupleWithIndexInHandAndSelectedCard);
        player.MoveCardFromHandToRingAreaByIndex(indexInHand);
    }

    public void MoveCardFromHandToRingside(Player player, (int, Card) tupleWithIndexInHandAndSelectedCard)
    {
        int indexInHand = _tupleManager.ExtractCardIndexInHand(tupleWithIndexInHandAndSelectedCard);
        player.MoveCardFromHandToRingsideByIndex(indexInHand);
    }
    
    public void MoveCardsFromArsenalToHand(Player player, int numberOfCards)
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
    
    public void MoveCardsFromArsenalToRingSideByDamageAmount(Player player, int damageAmount)
    {
        List<Card> cardsList = player.GetLastCardsFromArsenalReversed(damageAmount);
        if (damageAmount >= player.GetArsenalSize()) damageAmount = player.GetArsenalSize();
        for (int index = damageAmount - 1; index >= 0; index--)
        {
            Card currentCard = cardsList[index];
            player.AddCardToRingside(currentCard);
            player.RemoveLastCardFromArsenal(); 
        }
    }
}