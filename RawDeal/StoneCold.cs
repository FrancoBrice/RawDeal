namespace RawDeal;

public class StoneCold : SuperStar
{
    public StoneCold(string name, string logo, int handSize, int superstarValue, string superstarAbility) : base(name, logo, handSize, superstarValue, superstarAbility)
    {
    }

    public override void UseAbility(Player player, Player opponentPlayer)
    {
        _view.SayThatPlayerDrawCards(player.GetSuperStarName(), 1);
        player.MoveCardFromArsenalToHand();
        int indexCardFromHand =
            _view.AskPlayerToReturnOneCardFromHisHandToHisArsenal(player.GetSuperStarName(),
                player.GetFormattedCardsFromHand());
        player.MoveCardByIndexFromHandToArsenalBeginning(indexCardFromHand);
    }

    public override bool CheckIfCanUseAbility(Player player)
    {
        if (player.GetArsenalSize() >= 1 && !player.HasUsedHisAbilityInTheTurn)
        {
            return true;
        }

        return false;
    }

    public override bool CheckIfAbilityIsAutomatic()
    {
        return false;
    }
}