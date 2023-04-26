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

    public override bool CanUseAbility(Player player)
    {
        if (player.GetHandSize() >= 1 && !player.HasUsedHisAbilityInTheTurn)
        {
            return true;
        }
        return false;
    }

    public override bool IsAbilityAutomatic()
    {
        return false;
    }
}