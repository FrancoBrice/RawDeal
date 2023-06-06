using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects;

public class ManeuverInitialEffect : Effect
{
    public ManeuverInitialEffect(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        CardMobilizer.MoveCardFromHandToRingArea(currentPlay.CurrentPlayer, currentPlay.AttackingCardTuple);
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }
}