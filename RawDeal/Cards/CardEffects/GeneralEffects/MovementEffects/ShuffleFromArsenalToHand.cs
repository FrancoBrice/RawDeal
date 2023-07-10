using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.MovementEffects;

public class ShuffleFromArsenalToHand : Effect
{
    private int _actualNumberCardsToShuffle;
    private readonly int _pretendedNumberCardsToShuffle;

    public ShuffleFromArsenalToHand(View view, int pretendedNumberCardsToShuffle) : base(view)
    {
        _pretendedNumberCardsToShuffle = pretendedNumberCardsToShuffle;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (_currentPlayer.GetArsenalSize() >= _pretendedNumberCardsToShuffle)
            _actualNumberCardsToShuffle = _pretendedNumberCardsToShuffle;
        else _actualNumberCardsToShuffle = _currentPlayer.GetArsenalSize();
        RunShuffleLoop();
    }

    private void RunShuffleLoop()
    {
        int remainingCardsToShuffle = _actualNumberCardsToShuffle;
        for (int i = 0; i < _actualNumberCardsToShuffle; i++)
        {
            int indexInputByUser = _view.AskPlayerToSelectCardsToPutInHisHand(
                _currentPlayer.GetSuperStarName(), remainingCardsToShuffle,
                _currentPlayer.GetCardsInStringFormatFromArsenal());
            CardMobilizer.MoveFromArsenalToHandByIndex(_currentPlayer, indexInputByUser);
            remainingCardsToShuffle--;
        }
    }
}