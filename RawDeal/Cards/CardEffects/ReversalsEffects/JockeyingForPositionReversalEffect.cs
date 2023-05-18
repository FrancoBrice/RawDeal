using RawDeal.Cards.CardEffects.ActionEffects;
using RawDeal.GameLogic;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.Cards.CardEffects.ReversalsEffects;

public class JockeyingForPositionReversalEffect : Effect
{


    public JockeyingForPositionReversalEffect(View view) : base(view)
    {
    }

    public override void ApplyEffect(Play currentPlay)
    {
        ReversalByTitle cardEffect = new ReversalByTitle(_view);
        cardEffect.SetCardTitleThatCanReverse("Jockeying for Position");
        cardEffect.ApplyEffect(currentPlay);
        JockeyingForPositionActionEffect specialEffect = new JockeyingForPositionActionEffect(_view);
        currentPlay.EndPlay();
        specialEffect.SpecialEffect(currentPlay);
    }
}