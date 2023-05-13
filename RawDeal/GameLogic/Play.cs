using RawDeal.Cards;

namespace RawDeal.GameLogic;

public class Play
{
    private Player _currentPlayer;
    private Player _notCurrentPlayer;
    private Card _selectedCard;
    public bool IsReversed;
    
    
    public Play(Player currentPlayer, Player  notCurrentPlayer, Card selectedCard)
    {
        _currentPlayer = currentPlayer;
        _notCurrentPlayer = notCurrentPlayer;
        _selectedCard = selectedCard;
    }
    
}