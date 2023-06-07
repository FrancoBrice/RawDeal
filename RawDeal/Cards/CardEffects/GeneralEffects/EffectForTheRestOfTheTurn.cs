using RawDeal.GameLogic;
using RawDeal.GameLogic.Plays;
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
        if (_playIdThatApliesBonus != currentPlay.Id)
        {
            currentPlay.PendingEffects.Remove(this);
            return;
        }
        _effect.ApplyEffect(currentPlay);
        currentPlay.PendingEffects.Remove(this);
        currentPlay.AddPendingEffect(this);
    }
}