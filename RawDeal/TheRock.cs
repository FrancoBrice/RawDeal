namespace RawDeal;

public class TheRock : SuperStar
{
    public TheRock(string name, string logo, int handSize, int superstarValue, string superstarAbility) : base(name, logo, handSize, superstarValue, superstarAbility)
    {
        
    }

    public override void UseAbility(Player player, Player opponentPlayer)
    {
        if (!DoesPlayerWantToUseAbility(player)) return;
        int indexInputByUser = _view.AskPlayerToSelectCardsToRecover(Name, 1, player.Ringside.GetFormattedCards() );
        player.MoveCardByIndexFromRingsideToArsenalBeginning(indexInputByUser);
    }

    public override bool CanUseAbility(Player player)
    {
        return player.GetRingsideSize() > 0 && !player.HasUsedHisAbilityInTheTurn;
    }

    public override bool IsAbilityAutomatic()
    {
        return true;
    }
}