using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class MoveCardFromHandToRingArea : Effect
{
    public MoveCardFromHandToRingArea(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        _cardMobilizer.MoveCardFromHandToRingArea(CurrentPlayer, AttackingCardTuple);
    }
}