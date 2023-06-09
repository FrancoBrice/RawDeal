using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.EffectsForNextCards;

public class EffectForTheRestOfTheTurn : Effect
{
    private readonly Effect _effect;
    private readonly int _playIdThatAppliesBonus;

    public EffectForTheRestOfTheTurn(View view, Play currentPlay, Effect effect) : base(view)
    {
        _playIdThatAppliesBonus = currentPlay.Id;
        _effect = effect;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        currentPlay.RemoveAPendingEffect(this);
        if (_playIdThatAppliesBonus != currentPlay.Id) return;
        _effect.ApplyEffect(currentPlay);
        currentPlay.AddPendingEffect(this);
    }
}