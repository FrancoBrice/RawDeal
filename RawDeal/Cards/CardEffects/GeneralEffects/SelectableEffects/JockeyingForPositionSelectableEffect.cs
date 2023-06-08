using RawDeal.Cards.CardEffects.GeneralEffects.EffectsForNextCards;
using RawDeal.GameLogic.Plays;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.Cards.CardEffects.GeneralEffects.SpecificCards;

public class JockeyingForPositionSelectableEffect : Effect
{
    public JockeyingForPositionSelectableEffect(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        SelectedEffect selectedEffect =
            _view.AskUserToSelectAnEffectForJockeyForPosition(CurrentPlayer.GetSuperStarName());
        if (selectedEffect == SelectedEffect.NextGrappleIsPlus4D)
        {
            NextCardDamageBonusByTypeAndSubtype effect =
                new NextCardDamageBonusByTypeAndSubtype(_view);
            effect.SetTypeAndSubtypeThatAppliesBonus(type: "Maneuver", subtype: "Grapple");
            effect.SetDamageBonus(4);
            currentPlay.AddPendingEffect(effect);
        }
        else if (selectedEffect == SelectedEffect.NextGrapplesReversalIsPlus8F)
        {
            NextReversalRequiresMoreFortitudeByTypeAndSubtype effect =
                new NextReversalRequiresMoreFortitudeByTypeAndSubtype(_view);
            effect.SetPlayedTypeThatAppliesExtraFortitude("Maneuver");
            effect.SetSubtypeThatAppliesExtraFortitude("Grapple");
            effect.SetExtraFortitude(8);
            currentPlay.AddPendingEffect(effect);
        }
    }
}