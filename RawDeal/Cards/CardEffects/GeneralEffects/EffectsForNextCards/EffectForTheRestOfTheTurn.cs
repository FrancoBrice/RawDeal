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
        if (_playIdThatAppliesBonus != currentPlay.Id)
        {
            currentPlay.PendingEffects.Remove(this);
            return;
        }

        _effect.ApplyEffect(currentPlay);
        currentPlay.PendingEffects.Remove(this);
        currentPlay.AddPendingEffect(this);
    }
}