namespace RawDeal.GameLogic;

public static class GameEndChecker
{
    public static bool OpponentRanOutOfCardsDuringDamage(Player opponentPlayer, int damage)
    {
        int maximumDamage = opponentPlayer.GetArsenalSize();
        return damage > maximumDamage;
    }
    
    public static void CheckForGameOver(Game game)
    {
        if (game.NotCurrentPlayer.HasZeroCardsInArsenal() && game.CurrentPlayer.HasEndsHisTurn )
        {
            game.EndGame(winnerPlayer: game.CurrentPlayer);
        }
    }
}