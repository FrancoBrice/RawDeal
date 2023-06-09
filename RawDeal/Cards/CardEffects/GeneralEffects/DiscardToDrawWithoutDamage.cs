using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class DiscardToDrawWithoutDamage : Effect
{
    public DiscardToDrawWithoutDamage(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        _view.SayThatPlayerMustDiscardThisCard(_currentPlayer.GetSuperStarName(),
            _attackingCard.Title);
        CardMobilizer.MoveSpecificCardFromHandToRingside(_currentPlayer, _attackingCardTuple);
        _currentPlayer.MoveCardFromArsenalToHand();
        _view.SayThatPlayerDrawCards(_currentPlayer.GetSuperStarName(), numberOfCardsToDraw: 1);
    }
}