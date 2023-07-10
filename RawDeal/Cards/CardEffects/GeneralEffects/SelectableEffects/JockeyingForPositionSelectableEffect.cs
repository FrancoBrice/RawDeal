using RawDeal.Cards.CardEffects.GeneralEffects.EffectsForNextCards;
using RawDeal.GameLogic.Plays;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.Cards.CardEffects.GeneralEffects.SelectableEffects;

public class JockeyingForPositionSelectableEffect : Effect
{
    private Game _game;
    
    public JockeyingForPositionSelectableEffect(View view, Game game) : base(view)
    {
        _game = game;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        SelectedEffect selectedEffect =
            _view.AskUserToSelectAnEffectForJockeyForPosition(_currentPlayer.GetSuperStarName());
        switch (selectedEffect)
        {
            case SelectedEffect.NextGrappleIsPlus4D:
            {
                currentPlay.AddPendingEffect(new NextCardDamageBonusByTypeAndSubtype(_view, _game.GetPlayManager(),
                    type: "Maneuver", subtype: "Grapple", bonus: 4));
                break;
            }
            case SelectedEffect.NextGrapplesReversalIsPlus8F:
            {
                currentPlay.AddPendingEffect(new ReversalsRequiresMoreFortitudeByTypeAndSubtype(
                    _view, _game.GetPlayManager(), type: "Maneuver", subtype: "Grapple", extraFortitude: 8));
                break;
            }
        }
    }
}