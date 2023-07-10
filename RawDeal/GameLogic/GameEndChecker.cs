using RawDeal.GameLogic.Players;

namespace RawDeal.GameLogic;

public static class GameEndChecker
{
    public static bool CheckForGameOver(Game game)
    {
        if (game.NotCurrentPlayer.HasZeroCardsInArsenal() && game.CurrentPlayer.HasEndsHisTurn)
        {
            game.EndGame(game.CurrentPlayer);
            return true;
        }
        return false;
    }

    public static bool PlayerRanOutOfCardsDuringDamage(Player player, int damage)
    {
        int maximumDamage = player.GetArsenalSize();
        return damage > maximumDamage;
    }
}