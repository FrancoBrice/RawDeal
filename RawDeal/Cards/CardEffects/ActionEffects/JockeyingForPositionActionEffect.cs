using RawDeal.Cards.CardEffects.SpecificCardEffects;
using RawDeal.GameLogic;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.Cards.CardEffects.ActionEffects;

public class JockeyingForPositionActionEffect : Effect
{
    public JockeyingForPositionActionEffect(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        _view.SayThatPlayerSuccessfullyPlayedACard();
        _cardMobilizer.MoveCardFromHandToRingArea(CurrentPlayer, AttackingCardTuple);
        currentPlay.IsAPendingEffect = true;
        JockeyingForPositionSelectableEffect selectableEffect = new JockeyingForPositionSelectableEffect(_view);
        selectableEffect.ApplyEffect(currentPlay);
    }
}