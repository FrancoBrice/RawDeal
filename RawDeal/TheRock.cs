namespace RawDeal;

public class TheRock : SuperStar
{
    public TheRock(string name, string logo, int handSize, int superstarValue, string superstarAbility) : base(name, logo, handSize, superstarValue, superstarAbility)
    {
        
    }

    public override void UseAbility(Player player, Player opponentPlayer)
    {
        bool doesPlayerWantToUseHisAbility = CheckIfPlayerWantToUseHisAbility(player);
        if (doesPlayerWantToUseHisAbility)
        {
            int index = _view.AskPlayerToSelectCardsToRecover(Name, 1, player.Ringside.GetFormattedCards() );
            player.MoveCardByIndexFromRingsideToArsenalBeginning(index);
        }
    }

    public override bool CheckIfCanUseAbility(Player player)
    {
        if (player.GetRingsideSize() > 0 && !player.HasUsedHisAbilityInTheTurn)
        {
            return true;
        }
        return false;
    }

    public override bool CheckIfAbilityIsAutomatic()
    {
        return true;
    }

    private bool CheckIfPlayerWantToUseHisAbility(Player player)
    {
        bool doesPlayerCanUseHisAbility = false;
        if (player.Ringside.CardListSize > 0)
        { 
            doesPlayerCanUseHisAbility = _view.DoesPlayerWantToUseHisAbility(Name);
        }

        return doesPlayerCanUseHisAbility;
    }
}