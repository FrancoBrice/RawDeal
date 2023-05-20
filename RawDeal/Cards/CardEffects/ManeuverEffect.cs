using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects;

public class ManeuverEffect : Effect
{
    public ManeuverEffect(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        _cardMobilizer.MoveCardFromHandToRingArea(currentPlay.CurrentPlayer, currentPlay.AttackingCardTuple);
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }
}