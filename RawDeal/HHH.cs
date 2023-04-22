namespace RawDeal;

public class HHH : SuperStar
{
    public HHH(string name, string logo, int handSize, int superstarValue, string superstarAbility) : base(name, logo, handSize, superstarValue, superstarAbility)
    {
        
    }

    public override void UseAbility(Player player, Player opponentPlayer)
    {
        
    }

    public override bool CanUseAbility(Player player)
    {
        return false;
    }

    public override bool IsAbilityAutomatic()
    {
        return true;
    }
}