using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ReversalsEffects;

public class ReversalWithMaximumDamage : Effect
{
    private int _maximumDamageThatCanReverse;
    public ReversalWithMaximumDamage(View view) : base(view)
    {
        
    }

    public override void ApplyEffect(Play currentPlay)
    {
        int? actualDamage = currentPlay.AttackingCard.GetCurrentDamage();
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