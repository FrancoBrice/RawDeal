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
            var effect = new NextCardDamageBonusByTypeAndSubtype(_view);
            effect.SetTypeAndSubtypeThatAppliesBonus(type: "Maneuver", subtype: "Grapple");
            effect.SetDamageBonus(bonus: 4);
            currentPlay.AddPendingEffect(effect);
        }
        else if (selectedEffect == SelectedEffect.NextGrapplesReversalIsPlus8F)
        {
            var effect = new NextReversalRequiresMoreFortitudeByTypeAndSubtype(_view);
            effect.SetPlayedTypeThatAppliesExtraFortitude("Maneuver");
            effect.SetSubtypeThatAppliesExtraFortitude("Grapple");
            effect.SetExtraFortitude(8);
            currentPlay.AddPendingEffect(effect);
        }
    }
}
