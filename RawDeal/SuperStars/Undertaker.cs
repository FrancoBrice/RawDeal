namespace RawDeal.SuperStars;

public class Undertaker : SuperStar
{
    public Undertaker(string name, string logo, int handSize, int superstarValue, string superstarAbility) : base(name, logo, handSize, superstarValue, superstarAbility)
    {
        
    }

    public override void UseAbility(Player player, Player opponentPlayer)
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(player.GetSuperStarName(), player.GetSuperStarAbility());
        int numberOfCardsToDiscard = 2;
        MakePlayerDiscardCardsWithSelection(player, numberOfCardsToDiscard);
        RecoverCardFromRingide(player);

    }

    public override bool CanUseAbility(Player player)
    {
        int minimumCardsInHand = 2;
        return player.GetHandSize() >= minimumCardsInHand && !player.HasUsedHisAbilityInTheTurn;
    }

    public override bool IsAbilityAutomatic()
    {
        return false;
    }
    
}