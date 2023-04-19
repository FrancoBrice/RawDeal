namespace RawDeal;

public class Kane : SuperStar
{
    public Kane(string name, string logo, int handSize, int superstarValue, string superstarAbility) : base(name, logo, handSize, superstarValue, superstarAbility)
    {
    }

    public override void UseAbility(Player player, Player opponentPlayer)
    {
        if (!player.HasUsedHisAbilityInTheTurn)
        {
            _view.SayThatPlayerIsGoingToUseHisAbility(player.GetSuperStarName(), player.GetSuperStarAbility());
            Card cardDiscarted = opponentPlayer.GetCardsFromArsenal(1)[0];
            opponentPlayer.ReceiveDamage(1);
            _view.ShowCardOverturnByTakingDamage(cardDiscarted.GetCardFormattedInfo(), 1, 1);
        }
        
   
    }

    public override bool CheckIfCanUseAbility(Player player)
    {
        return true;
    }

    public override bool CheckIfAbilityIsAutomatic()
    {
        return true;
    }
}