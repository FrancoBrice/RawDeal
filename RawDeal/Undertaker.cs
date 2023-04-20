namespace RawDeal;

public class Undertaker : SuperStar
{
    public Undertaker(string name, string logo, int handSize, int superstarValue, string superstarAbility) : base(name, logo, handSize, superstarValue, superstarAbility)
    {
    }

    public override void UseAbility(Player player, Player opponentPlayer)
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(player.GetSuperStarName(), player.GetSuperStarAbility());
        int cardsRemainingToDiscard = 2;
        for (int i = 0; i < 2; i++)
        {
            int indexCardFromHand = _view.AskPlayerToSelectACardToDiscard(player.GetFormattedCardsFromHand(), player.GetSuperStarName(),
                player.GetSuperStarName(), cardsRemainingToDiscard);
            player.MoveCardFromHandToRingsideByIndex(indexCardFromHand);
            cardsRemainingToDiscard--;
        }

        int indexCardFromRingside =
            _view.AskPlayerToSelectCardsToPutInHisHand(player.GetSuperStarName(), 1, player.GetCardTilesFromRingside());
        player.MoveCardFromRingsideToHandByIndex(indexCardFromRingside);

    }

    public override bool CheckIfCanUseAbility(Player player)
    {
        if (player.GetHandSize() >= 2 && !player.HasUsedHisAbilityInTheTurn)
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