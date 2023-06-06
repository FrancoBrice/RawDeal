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
        ReversalSimple effect = new ReversalSimple(_view);
        ReversalCard.SetCurrentDamage(NotCurrentPlayer.CalculateDamage(AttackingCard));
        effect.ApplyEffect(currentPlay);
    }
}