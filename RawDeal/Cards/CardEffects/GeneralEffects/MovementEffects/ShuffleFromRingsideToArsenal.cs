using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.MovementEffects;

public class ShuffleFromRingsideToArsenal : ShuffleFromRingsideEffect
{

    public ShuffleFromRingsideToArsenal(View view, int pretendedNumberCardsToShuffle) : base(view, 
        pretendedNumberCardsToShuffle)
    {
    }

    protected override void RunShuffleLoop()
    {
        int remainingCardsToShuffle = _actualNumberCardsToShuffle;
        for (int i = 0; i < _actualNumberCardsToShuffle; i++)
        {
            int indexInputByUser = _view.AskPlayerToSelectCardsToRecover(
                _currentPlayer.GetSuperStarName(), remainingCardsToShuffle,
                _currentPlayer.GetCardsInStringFormatFromRingside());
            CardMobilizer.MoveFromRingsideToArsenalBeginningByIndex(_currentPlayer, indexInputByUser);
            remainingCardsToShuffle--;
        }
    }
    
}