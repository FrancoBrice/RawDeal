using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ReversalsEffects.SpecificCards;

public class RollingTakedown : Effect
{
    public RollingTakedown(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        var reversalEffect = new ReversalSimple(_view);
        ReversalCard.SetCurrentDamage(NotCurrentPlayer.CalculateDamage(AttackingCard));
        reversalEffect.ApplyEffect(currentPlay);
    }
}