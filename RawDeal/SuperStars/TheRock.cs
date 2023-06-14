using RawDeal.Cards;
using RawDeal.GameLogic.Players;
using RawDeal.Tools;

namespace RawDeal.SuperStars;

public class TheRock : SuperStar
{
    public TheRock(string name, string logo, int handSize, int superstarValue,
        string superstarAbility) : base(name, logo, handSize, superstarValue, superstarAbility)
    {
    }

    public override void UseAbility(Player player, Player opponentPlayer)
    {
        if (!DoesPlayerWantToUseAbility(player))
        {
            player.HasUsedHisAbilityInTheTurn = true;
            return;
        }
        const int numberOfCards = 1;
        int indexInputByUser = _view.AskPlayerToSelectCardsToRecover(Name, numberOfCards,
            player.GetCardsInStringFormatFromRingside());
        CardMobilizer.MoveFromRingsideToArsenalBeginningByIndex(player, indexInputByUser);
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