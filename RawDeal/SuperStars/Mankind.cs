using RawDeal.GameLogic.Players;

namespace RawDeal.SuperStars;

public class Mankind : SuperStar
{
    public Mankind(string name, string logo, int handSize, int superstarValue, string superstarAbility) : base(name, logo, handSize, superstarValue, superstarAbility)
    {
        HasInitialAbility = true;
    }

    public override void UseInitialAbility(Player player)
    {
        const int shieldOfDamage = 1;
        player.SetShieldOfDamage(shieldOfDamage);
    }

    public override void UseAbility(Player player, Player opponentPlayer)
    {
        player.MoveCardFromArsenalToHand();

    }
    
    public override bool CanUseAbility(Player player)
    {
        if (player.HasUsedHisAbilityInTheTurn)
        {
            return false;
        }
        return true;
    }

    public override bool IsAbilityAutomatic()
    {
        return true;
    }

}