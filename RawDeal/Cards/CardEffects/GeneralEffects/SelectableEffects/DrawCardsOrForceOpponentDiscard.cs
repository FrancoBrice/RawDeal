using RawDeal.Cards.CardEffects.GeneralEffects.DiscardCards;
using RawDeal.Cards.CardEffects.GeneralEffects.DrawCards;
using RawDeal.GameLogic.Plays;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.Cards.CardEffects.GeneralEffects.SelectableEffects;

public class DrawCardsOrForceOpponentDiscard : Effect
{
    public DrawCardsOrForceOpponentDiscard(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        SelectedEffect selectedEffectBBD =
            _view.AskUserToChooseBetweenDrawingOrForcingOpponentToDiscardCards(
                _currentPlayer.GetSuperStarName());
        if (selectedEffectBBD == SelectedEffect.DrawCards)
        {
            PlayerDrawCards playerDrawCardsEffect =
                new PlayerDrawCards(_view, _currentPlayer, numberOfCardsToDraw: 2);
            playerDrawCardsEffect.ApplyEffect(currentPlay);
        }
        else if (selectedEffectBBD == SelectedEffect.ForceOpponentToDiscard)
        {
            MakePlayerDiscardCard discardCardEffect = new MakePlayerDiscardCard(_view, 
                _notCurrentPlayer, numberOfCardToDiscard: 2);
            discardCardEffect.ApplyEffect(currentPlay);
        }
    }
}