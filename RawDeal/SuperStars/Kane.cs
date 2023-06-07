using RawDeal.Cards;
using RawDeal.GameLogic.Players;

namespace RawDeal.SuperStars;

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
            Card discartedCard = ApplyDamageToOpponent(opponentPlayer, 1);
            _view.ShowCardOverturnByTakingDamage(discartedCard.GetCardFormattedInfo(), 1, 1);
        }
    }

    public override bool CanUseAbility(Player player)
    {
        return true;
    }

    public override bool IsAbilityAutomatic()
    {
        return true;
    }
}