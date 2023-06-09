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
        if (_reversalCard.Damage != "#") _reversalCard.SetDefaultValues();
        if (_reversalCard.PlayedFrom == "Hand")
        {
            CardMobilizer.MoveSpecificCardFromHandToRingside(_currentPlayer, _attackingCardTuple);
            _currentPlayer.HasEndsHisTurn = true;
            _view.SayThatPlayerReversedTheCard(_notCurrentPlayer.GetSuperStarName(),
                _reversalCard.GetCardInPlayFormat(_reversalCard.PlayedType));
            CardMobilizer.MoveCardFromHandToRingArea(_notCurrentPlayer, _reversalCardTuple);
        }
        else if (_reversalCard.PlayedFrom == "Deck")
        {
            _currentPlayer.HasEndsHisTurn = true;
        }
    }
}