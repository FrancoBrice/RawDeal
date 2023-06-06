using RawDeal.Cards.CardEffects.ReversalsEffects;
using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.SpecificCardEffects;

public class CleanBreakEffect : Effect
{
    public CleanBreakEffect(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        var reversalByTitleEffect = new ReversalSimple(_view);
        reversalByTitleEffect.ApplyEffect(currentPlay);
        const int numberOfCardsThatOpponentMustDiscard = 4;
        CardMobilizer.MakePlayerDiscardCards(_view, CurrentPlayer, numberOfCardsThatOpponentMustDiscard);
        const int numberOfCardsToDrawByCleanBreak = 1;
        CardMobilizer.MoveCardsReversedFromArsenalToHand(NotCurrentPlayer, numberOfCardsToDrawByCleanBreak);
        _view.SayThatPlayerDrawCards(NotCurrentPlayer.GetSuperStarName(), numberOfCardsToDrawByCleanBreak);
    }
}