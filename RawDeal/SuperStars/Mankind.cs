namespace RawDeal;

public class Mankind : SuperStar
{
    public Mankind(string name, string logo, int handSize, int superstarValue, string superstarAbility) : base(name, logo, handSize, superstarValue, superstarAbility)
    {
        HasInitialAbility = true;
    }

    public override void UseInitialAbility(Player player)
    {
        player.SetShieldOfDamage(1);
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