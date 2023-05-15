using RawDeal.GameLogic;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.Cards.CardEffects.ActionEffects;

public class JockeyingForPositionActionEffect : Effect
{
    public JockeyingForPositionActionEffect(View view) : base(view)
    {
    }

    public override void ApplyEffect(Play currentPlay)
    {
        _view.SayThatPlayerSuccessfullyPlayedACard();
        SelectedEffect selectedEffect = _view.AskUserToSelectAnEffectForJockeyForPosition(currentPlay.CurrentPlayer.GetSuperStarName());
        _cardMobilizer.MoveCardFromHandToRingside(currentPlay.CurrentPlayer, currentPlay.AttackingCardTuple);
        currentPlay.IsAPendingEffect = true;
        switch (selectedEffect)
        {
            case SelectedEffect.NextGrappleIsPlus4D:
                currentPlay.NextCardDamageBonusByTypeAndSubtype(card, "Maneuver", "Grapple" , 4);
                break;
            case SelectedEffect.NextGrapplesReversalIsPlus8F:
                currentPlay.NextReversalHasMoreFortitudeBySubtype("Grapple", 8);
                break;
        }

    }
}