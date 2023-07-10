namespace RawDeal.GameLogic.Players;

public class PlayersPackage
{
    public readonly Player CurrentPlayer;
    public readonly Player NotCurrentPlayer;

    public PlayersPackage(Player currentPlayer, Player notCurrentPlayer)
    {
        CurrentPlayer = currentPlayer;
        NotCurrentPlayer = notCurrentPlayer;
    }
}