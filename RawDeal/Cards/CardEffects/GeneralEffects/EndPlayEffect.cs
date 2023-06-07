using RawDeal.GameLogic;
using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class EndPlayEffect : Effect
{

    public EndPlayEffect(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        currentPlay.EndPlay();
    }
}