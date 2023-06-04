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
        _cardMobilizer.MoveCardFromHandToRingArea(currentPlay.CurrentPlayer, currentPlay.AttackingCardTuple);
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }
}