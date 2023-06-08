using RawDeal.GameLogic.Plays;
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
                CurrentPlayer.GetSuperStarName(), remainingCardsToShuffle,
                CurrentPlayer.GetCardsInStringFormatFromRingside());
            CurrentPlayer.MoveCardByIndexFromRingsideToArsenalBeginning(indexInputByUser);
            remainingCardsToShuffle--;
        }
    }
    
}