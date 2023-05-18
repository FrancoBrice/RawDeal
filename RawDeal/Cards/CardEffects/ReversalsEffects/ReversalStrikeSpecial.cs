using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ReversalsEffects;

public class ReversalStrikeSpecial : Effect
{
    public ReversalStrikeSpecial(View view) : base(view)
    {
        
    }
    
    public override void ApplyEffect(Play currentPlay)
    {
        int maximumDamageThatCanReverse = 7;
        int? actualDamage = currentPlay.AttackingCard.GetCurrentDamage();
        if (CanReverseByDamage(maximumDamageThatCanReverse, actualDamage))
        {
            ReversalSimple cardEffect = new ReversalSimple(_view);
            cardEffect.ApplyEffect(currentPlay);
            
        }
    }

}