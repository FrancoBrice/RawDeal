using RawDeal.Cards.CardEffects.GeneralEffects.DrawCards;
using RawDeal.GameLogic.Plays;
using RawDealView.Options;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class DrawCardsOrForceOpponentDiscardSelectable : Effect
{
    private readonly Game _game;

    public DrawCardsOrForceOpponentDiscardSelectable(Game game) : base(game.ViewObject)
    {
        _game = game;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        SelectedEffect selectedEffectBBD =
            _view.AskUserToChooseBetweenDrawingOrForcingOpponentToDiscardCards(
                CurrentPlayer.GetSuperStarName());
        if (selectedEffectBBD == SelectedEffect.DrawCards)
        {
            DrawCards.PlayerDrawCards playerDrawCardsEffect =
                new PlayerDrawCards(_view, CurrentPlayer, numberOfCardsToDraw: 2);
            playerDrawCardsEffect.ApplyEffect(currentPlay);
        }
        else if (selectedEffectBBD == SelectedEffect.ForceOpponentToDiscard)
        {
            MakePlayerDiscardCard discardCardEffect =
                new MakePlayerDiscardCard(_game, NotCurrentPlayer, numberOfCardToDiscard: 2);
            discardCardEffect.ApplyEffect(currentPlay);
        }
    }
}