using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class DiscardCardFromOpponentsHand : Effect
{
    public DiscardCardFromOpponentsHand(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        List<string> opponentsHand =  NotCurrentPlayer.GetCardsInStringFormatFromHand();
        int indexInHand = _view.AskPlayerToSelectACardToDiscard(opponentsHand, NotCurrentPlayer.GetSuperStarName(),
            CurrentPlayer.GetSuperStarName(), totalCardsToDiscard: 1);
        NotCurrentPlayer.MoveCardFromHandToRingsideByIndex(indexInHand);

    }
}