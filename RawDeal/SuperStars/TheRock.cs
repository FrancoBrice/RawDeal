namespace RawDeal.SuperStars;

public class TheRock : SuperStar
{
    public TheRock(string name, string logo, int handSize, int superstarValue, string superstarAbility) : base(name, logo, handSize, superstarValue, superstarAbility)
    {
        
    }

    public override void UseAbility(Player player, Player opponentPlayer)
    {
        if (!DoesPlayerWantToUseAbility(player))
        {
            player.HasUsedHisAbilityInTheTurn = true;
            return;
        }
        int numberOfCards = 1; 
        int indexInputByUser = _view.AskPlayerToSelectCardsToRecover(Name, numberOfCards, player.Ringside.GetFormattedCards() );
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