using RawDeal.GameLogic.Players;
using RawDeal.Tools;

namespace RawDeal.SuperStars;

public class StoneCold : SuperStar
{
    public StoneCold(string name, string logo, int handSize, int superstarValue,
        string superstarAbility) : base(name, logo, handSize, superstarValue, superstarAbility)
    {
    }

    public override void UseAbility(Player player, Player opponentPlayer)
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(player.GetSuperStarName(),
            player.GetSuperStarAbility());
        _view.SayThatPlayerDrawCards(player.GetSuperStarName(), 1);
        CardMobilizer.MoveFromArsenalToHand(player);
        int indexCardFromHand =
            _view.AskPlayerToReturnOneCardFromHisHandToHisArsenal(player.GetSuperStarName(),
                player.GetCardsInStringFormatFromHand());
        CardMobilizer.MoveFromHandToArsenalBeginningByIndex(player, indexCardFromHand);
    }

    public override bool CanUseAbility(Player player)
    {
        return player.GetArsenalSize() >= 1 && !player.HasUsedHisAbilityInTheTurn;
    }

    public override bool IsAbilityAutomatic()
    {
        return false;
    }
}