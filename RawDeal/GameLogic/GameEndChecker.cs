namespace RawDeal.GameLogic;

public static class GameEndChecker
{
    public static bool PlayerRanOutOfCardsDuringDamage(Player player, int damage)
    {
        int maximumDamage = player.GetArsenalSize();
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