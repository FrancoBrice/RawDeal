using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ReversalsEffects;

public class ReversalWithMaximumDamage : Effect
{
    private int _maximumDamageThatCanReverse;
    public ReversalWithMaximumDamage(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        int? actualDamage = AttackingCard.GetCurrentDamage();
        if (CanReverseByDamage(_maximumDamageThatCanReverse, actualDamage))
        {
            ReversalSimple cardEffect = new ReversalSimple(_view);
            cardEffect.ApplyEffect(currentPlay);
        }
    }

    public void SetMaximumDamageThatCanReverse(int maximumDamageThatCanReverse)
    {
        _maximumDamageThatCanReverse = maximumDamageThatCanReverse;
    }


}