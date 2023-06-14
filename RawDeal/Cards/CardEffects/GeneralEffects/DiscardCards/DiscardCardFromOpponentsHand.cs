using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.DiscardCards;

public class DiscardCardFromOpponentsHand : Effect
{
    public DiscardCardFromOpponentsHand(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        List<string> opponentsHand = _notCurrentPlayer.GetCardsInStringFormatFromHand();
        int indexInHand = _view.AskPlayerToSelectACardToDiscard(opponentsHand,
            _notCurrentPlayer.GetSuperStarName(),
            _currentPlayer.GetSuperStarName(), totalCardsToDiscard: 1);
        CardMobilizer.MoveFromHandToRingsideByIndex(_notCurrentPlayer, indexInHand);
    }
}