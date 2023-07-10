using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class ManeuverInitialEffect : Effect
{
    public ManeuverInitialEffect(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        CardMobilizer.MoveFromHandToRingArea(_currentPlayer, _attackingIndexedCard);
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }
}