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
        const int maximumDamageReversalRollingTakeDown = 7;
        ReversalWithMaximumDamage reversalWithMaximumDamage = new ReversalWithMaximumDamage(_view, maximumDamageReversalRollingTakeDown);
        ReversalCard.SetCurrentDamage(NotCurrentPlayer.CalculateDamage(AttackingCard));
        reversalWithMaximumDamage.ApplyEffect(currentPlay);
    }
}