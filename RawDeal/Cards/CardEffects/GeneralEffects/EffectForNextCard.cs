using RawDeal.GameLogic;
using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class EffectForNextCard : Effect
{
    private Effect _effect;
    public EffectForNextCard(View view, Effect effect) : base(view)
    {
        _effect = effect;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        currentPlay.AddPendingEffect(_effect);
    }
}