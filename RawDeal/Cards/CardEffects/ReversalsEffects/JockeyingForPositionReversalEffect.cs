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
        SelectedEffect selectedEffect = _view.AskUserToSelectAnEffectForJockeyForPosition(currentPlay.CurrentPlayer.GetSuperStarName());
        switch (selectedEffect)
        {
            case SelectedEffect.NextGrappleIsPlus4D:
                currentPlay.NextCardDamageBonusByTypeAndSubtype("Maneuver", "Grapple", 4);
                break;
            case SelectedEffect.NextGrapplesReversalIsPlus8F:
                currentPlay.NextReversalHasMoreFortitudeBySubtype("Grapple", 8);
                break;
        }
    }
}