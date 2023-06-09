using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects;

public class ManeuverInitialEffect : Effect
{
    public ManeuverInitialEffect(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        CardMobilizer.FromHandToRingArea(_currentPlayer, _attackingCardTuple);
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }
}