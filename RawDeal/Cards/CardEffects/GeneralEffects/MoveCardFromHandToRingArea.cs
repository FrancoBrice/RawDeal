using RawDeal.GameLogic;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class MoveCardFromHandToRingArea : Effect
{
    public MoveCardFromHandToRingArea(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        CardMobilizer.MoveCardFromHandToRingArea(CurrentPlayer, AttackingCardTuple);
    }
}