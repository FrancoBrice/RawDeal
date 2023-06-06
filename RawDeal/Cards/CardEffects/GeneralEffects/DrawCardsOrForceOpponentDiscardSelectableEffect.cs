using RawDeal.Cards.CardEffects.GeneralEffects;
using RawDeal.GameLogic;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.Cards.CardEffects.SpecificCardEffects;

public class DrawCardsOrForceOpponentDiscardSelectableEffect : Effect
{
    private Game _game;
    public DrawCardsOrForceOpponentDiscardSelectableEffect(Game game) : base(game.ViewObject)
    {
        _game = game;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        SelectedEffect selectedEffectBBD = _view.AskUserToChooseBetweenDrawingOrForcingOpponentToDiscardCards(CurrentPlayer.GetSuperStarName());
        if (selectedEffectBBD == SelectedEffect.DrawCards)
        {
            DrawCards drawCardsEffect = new DrawCards(_view, playerThatMustDraw: CurrentPlayer, numberOfCardsToDraw: 2);
            drawCardsEffect.ApplyEffect(currentPlay);
        }
        else if (selectedEffectBBD == SelectedEffect.ForceOpponentToDiscard)
        {
            MakePlayerDiscardCard discardCardEffect = new MakePlayerDiscardCard( _game,  playerThatMustDiscard: NotCurrentPlayer, numberOfCardToDiscard: 2);
            discardCardEffect.ApplyEffect(currentPlay);
        }
    }
}