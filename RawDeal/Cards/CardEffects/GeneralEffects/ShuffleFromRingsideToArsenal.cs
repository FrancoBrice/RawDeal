using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class ShuffleFromRingsideToArsenal: Effect
{
    private int _pretendedNumberCardsToShuffle;
    private int _actualNumberCardsToShuffle;
    public ShuffleFromRingsideToArsenal(View view, int pretendedNumberCardsToShuffle) : base(view)
    {
        _pretendedNumberCardsToShuffle = pretendedNumberCardsToShuffle;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (CurrentPlayer.GetRingsideSize() >= _pretendedNumberCardsToShuffle)
            _actualNumberCardsToShuffle = _pretendedNumberCardsToShuffle;
        else _actualNumberCardsToShuffle = CurrentPlayer.GetRingsideSize();
        int remainingCardsToShuffle = _actualNumberCardsToShuffle;
        for (int i = 0; i < _actualNumberCardsToShuffle; i++)
        {
            int indexInputByUser = _view.AskPlayerToSelectCardsToRecover(CurrentPlayer.GetSuperStarName(), remainingCardsToShuffle, CurrentPlayer.Ringside.GetFormattedCards());
            CurrentPlayer.MoveCardByIndexFromRingsideToArsenalBeginning(indexInputByUser);
            remainingCardsToShuffle--;
        }
    }
}