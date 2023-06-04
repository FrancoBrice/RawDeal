using RawDeal.Cards.CardEffects.ReversalsEffects;
using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.SpecificCardEffects;

public class KneeToTheGut : Effect
{
    public KneeToTheGut(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        const int maximumDamageReversalKneeToTheGut = 7;
        ReversalWithMaximumDamage reversalWithMaximumDamage = new ReversalWithMaximumDamage(_view, maximumDamageReversalKneeToTheGut);
        ReversalCard.SetCurrentDamage(NotCurrentPlayer.CalculateDamage(AttackingCard));
        reversalWithMaximumDamage.ApplyEffect(currentPlay);
    }
}