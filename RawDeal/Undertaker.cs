namespace RawDeal;

public class Undertaker : SuperStar
{
    public Undertaker(string name, string logo, int handSize, int superstarValue, string superstarAbility) : base(name, logo, handSize, superstarValue, superstarAbility)
    {
        
    }

    public override void UseAbility(Player player, Player opponentPlayer)
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(player.GetSuperStarName(), player.GetSuperStarAbility());
        MakePlayerDiscardCardsWithSelection(player, 2);
        RecoverCardFromRingide(player);

    }

    public override bool CanUseAbility(Player player)
    {
        if (player.GetHandSize() >= 2 && !player.HasUsedHisAbilityInTheTurn)
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