namespace RawDeal;

public class Jericho : SuperStar
{
    public Jericho(string name, string logo, int handSize, int superstarValue, string superstarAbility) : base(name, logo, handSize, superstarValue, superstarAbility)
    {
    }

    public override void UseAbility(Player player, Player opponentPlayer)
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(player.GetSuperStarName(), player.GetSuperStarAbility());
        MakePlayerDiscardACard(player);
        MakePlayerDiscardACard(opponentPlayer);
    }

    public override bool CheckIfCanUseAbility(Player player)
    {
        if (player.GetHandSize() >= 1 && !player.HasUsedHisAbilityInTheTurn)
        {
            return true;
        }

        return false;
    }

    public override bool CheckIfAbilityIsAutomatic()
    {
        return false;
    }

    private void MakePlayerDiscardACard(Player player)
    {
        int indexCardFromPlayerHand = _view.AskPlayerToSelectACardToDiscard(player.GetCardsStringsFromHand(), player.GetSuperStarName(),
            player.GetSuperStarName(), 1);
        player.MoveCardFromHandToRingsideByIndex(indexCardFromPlayerHand);
    }
}