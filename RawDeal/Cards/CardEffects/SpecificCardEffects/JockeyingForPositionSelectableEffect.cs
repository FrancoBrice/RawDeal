using RawDeal.Cards.CardEffects.ActionEffects;
using RawDeal.Cards.CardEffects.GeneralEffects;
using RawDeal.GameLogic;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.Cards.CardEffects.SpecificCardEffects;

public class JockeyingForPositionSelectableEffect : Effect
{
    public JockeyingForPositionSelectableEffect(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        SelectedEffect selectedEffect = _view.AskUserToSelectAnEffectForJockeyForPosition(CurrentPlayer.GetSuperStarName());
        if (selectedEffect == SelectedEffect.NextGrappleIsPlus4D)
        {
            NextCardDamageBonusByTypeAndSubtype effect = new NextCardDamageBonusByTypeAndSubtype(_view);
            effect.SetPlayedTypeThatAppliesBonus("Maneuver");
            effect.SetSubtypeThatAppliesBonus("Grapple");
            effect.SetDamageBonus(4);
            currentPlay.SetPendingEffect(effect);
        }
        else if (selectedEffect == SelectedEffect.NextGrapplesReversalIsPlus8F)
        {
            NextReversalRequiresMoreFortitudeByTypeAndSubtype effect = new NextReversalRequiresMoreFortitudeByTypeAndSubtype(_view);
            effect.SetPlayedTypeThatAppliesExtraFortitude("Maneuver");
            effect.SetSubtypeThatAppliesExtraFortitude("Grapple");
            effect.SetExtraFortitude(8);
            currentPlay.SetPendingEffect(effect);
        }
    }
}
