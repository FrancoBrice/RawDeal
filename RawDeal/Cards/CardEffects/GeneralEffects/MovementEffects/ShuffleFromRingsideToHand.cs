using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.MovementEffects;

public class ShuffleFromRingsideToHand : ShuffleFromRingsideEffect
{
    public ShuffleFromRingsideToHand(View view, int pretendedNumberCardsToShuffle) : base(view, 
        pretendedNumberCardsToShuffle)
    {
    }

    protected override void RunShuffleLoop()
    {
        int remainingCardsToShuffle = _actualNumberCardsToShuffle;
        for (int i = 0; i < _actualNumberCardsToShuffle; i++)
        {
            int indexInputByUser = _view.AskPlayerToSelectCardsToPutInHisHand(
                CurrentPlayer.GetSuperStarName(), remainingCardsToShuffle,
                CurrentPlayer.GetCardsInStringFormatFromRingside());
            CurrentPlayer.MoveCardFromRingsideToHandByIndex(indexInputByUser);
            remainingCardsToShuffle--;
        }
    }
}