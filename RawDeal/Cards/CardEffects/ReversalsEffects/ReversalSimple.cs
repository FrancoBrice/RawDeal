using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ReversalsEffects;

public class ReversalSimple : Effect
{
    public ReversalSimple(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (_reversalCard.Damage != "#" && !_reversalCard.HasPendingEffect) _reversalCard.SetDefaultValues();
        _currentPlayer.HasEndsHisTurn = true;
        if (_reversalCard.PlayedFrom == "Hand") HandleReversalFromHand();
    }

    private void HandleReversalFromHand()
    {
        CardMobilizer.MoveSpecificCardFromHandToRingside(_currentPlayer, _attackingIndexedCard);
        _view.SayThatPlayerReversedTheCard(_notCurrentPlayer.GetSuperStarName(),
            _reversalCard.GetCardInPlayFormat(_reversalCard.PlayedType));
        CardMobilizer.MoveFromHandToRingArea(_notCurrentPlayer, _reversalIndexedCard);
    }
}