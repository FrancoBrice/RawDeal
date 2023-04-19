namespace RawDeal;

public class Mankind : SuperStar
{
    public Mankind(string name, string logo, int handSize, int superstarValue, string superstarAbility) : base(name, logo, handSize, superstarValue, superstarAbility)
    {
    }

    public override void UseAbility(Player player, Player opponentPlayer)
    {
        player.MoveCardFromArsenalToHand();
        player.SetShieldOfDamage(1);
    }


    public override bool CheckIfCanUseAbility(Player player)
    {
        if (player.HasUsedHisAbilityInTheTurn)
        {
            return false;
        }

        return true;
    }

    public override bool CheckIfAbilityIsAutomatic()
    {
        return true;
    }
}