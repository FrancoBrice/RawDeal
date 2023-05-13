namespace RawDeal.Cards;

public class CardsEffects
{
    public Player CurrentPlayer;
    public Player NotCurrentPlayer;
    public Card SelectedCard;
    
    public CardsEffects(List<Player> playersList, Card selectedCard)
    {
        CurrentPlayer = playersList[0];
        NotCurrentPlayer = playersList[1];
        SelectedCard = selectedCard;
    }
    
    public void EndOpponentsTurn()
    {
        NotCurrentPlayer.EndsHisTurn = true;
    }

    public void ApplyReversalBySubtype(Card selectedCard, Card selectedReversalCard)
    {
        
    }
}