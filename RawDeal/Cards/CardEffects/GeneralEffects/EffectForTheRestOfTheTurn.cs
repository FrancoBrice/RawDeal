using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class EffectForTheRestOfTheTurn : Effect
{
    private int _playIdThatApliesBonus;
    private Effect _effect;
    public EffectForTheRestOfTheTurn(View view, Play currentPlay, Effect effect) : base(view)
    {
        _playIdThatApliesBonus = currentPlay.Id;
        _effect = effect;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (_playIdThatApliesBonus != currentPlay.Id) return;
        _effect.ApplyEffect(currentPlay);
        currentPlay.AddPendingEffect(_effect);
    }
}