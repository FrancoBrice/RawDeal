using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class ShuffleFromRingsideToHand : Effect
{
    private int _pretendedNumberCardsToShuffle;
    private int _actualNumberCardsToShuffle;
    public ShuffleFromRingsideToHand(View view, int pretendedNumberCardsToShuffle) : base(view)
    {
        _pretendedNumberCardsToShuffle = pretendedNumberCardsToShuffle;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (CurrentPlayer.GetRingsideSize() >= _pretendedNumberCardsToShuffle)
            _actualNumberCardsToShuffle = _pretendedNumberCardsToShuffle;
        else _actualNumberCardsToShuffle = CurrentPlayer.GetRingsideSize();
        for (int i = 0; i < _actualNumberCardsToShuffle; i++)
        {
            int indexInputByUser = _view.AskPlayerToSelectCardsToRecover(CurrentPlayer.GetSuperStarName(), _actualNumberCardsToShuffle, CurrentPlayer.Ringside.GetFormattedCards());
            CurrentPlayer.MoveCardByIndexFromRingsideToArsenalBeginning(indexInputByUser);    
        }
    }
}