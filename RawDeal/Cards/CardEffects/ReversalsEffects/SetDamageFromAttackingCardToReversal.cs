using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ReversalsEffects;

public class SetDamageFromAttackingCardToReversal : Effect
{
    public SetDamageFromAttackingCardToReversal(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        _reversalCard.SetCurrentDamage(_notCurrentPlayer.CalculateDamage(_attackingCard));
    }
}