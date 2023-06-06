using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ActionEffects;

public class DiscardToDrawWithoutDamage : Effect
{
    public DiscardToDrawWithoutDamage(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        _view.SayThatPlayerMustDiscardThisCard(CurrentPlayer.GetSuperStarName(), AttackingCard.Title);
        CardMobilizer.MoveSpecificCardFromHandToRingside(CurrentPlayer, AttackingCardTuple);
        CurrentPlayer.MoveCardFromArsenalToHand();
        _view.SayThatPlayerDrawCards(CurrentPlayer.GetSuperStarName(), numberOfCardsToDraw: 1);
    }

}