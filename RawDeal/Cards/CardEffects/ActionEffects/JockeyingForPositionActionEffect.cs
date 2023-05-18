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
        _cardMobilizer.MoveCardFromHandToRingArea(currentPlay.CurrentPlayer, currentPlay.AttackingCardTuple);
        currentPlay.IsAPendingEffect = true;
        SpecialEffect(currentPlay);
    }

    public override void SpecialEffect(Play currentPlay)
    {
        SelectedEffect selectedEffect = _view.AskUserToSelectAnEffectForJockeyForPosition(currentPlay.CurrentPlayer.GetSuperStarName());
        JockeyingForPositionActionEffect effect = new JockeyingForPositionActionEffect(_view);
        currentPlay.SetPendingEffect(effect);
        switch (selectedEffect)
        {
            case SelectedEffect.NextGrappleIsPlus4D:
                currentPlay.NextCardDamageBonusByTypeAndSubtype(currentPlay.GetLastCard(), "Maneuver", "Grapple" , 4);
                break;
            case SelectedEffect.NextGrapplesReversalIsPlus8F:
                currentPlay.NextReversalRequiresMoreFortitudeBySubtype(currentPlay.GetLastCard(), "Maneuver", "Grapple" , 8);
                break; 
        }
        currentPlay.IsAPendingEffect = true;
    }

}