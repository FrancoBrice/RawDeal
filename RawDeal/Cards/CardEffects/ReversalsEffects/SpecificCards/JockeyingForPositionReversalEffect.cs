using RawDeal.Cards.CardEffects.ActionEffects;
using RawDeal.Cards.CardEffects.SpecificCardEffects;
using RawDeal.GameLogic;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.Cards.CardEffects.ReversalsEffects;

public class JockeyingForPositionReversalEffect : Effect
{


    public JockeyingForPositionReversalEffect(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        ReversalByTitle cardEffect = new ReversalByTitle(_view,  cardTitleThatCanReverse: "Jockeying for Position");
        cardEffect.ApplyEffect(currentPlay);
        currentPlay.EndPlay();
        var selectableEffect = new JockeyingForPositionSelectableEffect(_view);
        selectableEffect.ApplyEffect(currentPlay);
    }
}