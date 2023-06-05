using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ReversalsEffects;

public class ReversalWithMaximumDamage : Effect
{
    private int _maximumDamageThatCanReverse;
    public ReversalWithMaximumDamage(View view, int maximumDamageThatCanReverse) : base(view)
    {
        _maximumDamageThatCanReverse = maximumDamageThatCanReverse;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        int? currentCardDamage = AttackingCard.GetCurrentDamage(AttackingCard.PlayedType);
        int actualDamage = currentPlay.CurrentPlayer.CalculateDamage(AttackingCard);
        Console.WriteLine($"En reversalwithmd");
        Console.WriteLine(currentCardDamage);
        Console.WriteLine(actualDamage);
        if (CanReverseByDamage(_maximumDamageThatCanReverse, actualDamage))
        {
            ReversalSimple cardEffect = new ReversalSimple(_view);
            cardEffect.ApplyEffect(currentPlay);
        }
    }
}