using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.MovementEffects;

public abstract class ShuffleFromRingsideEffect : Effect
{
    private protected int _actualNumberCardsToShuffle;
    private readonly int _pretendedNumberCardsToShuffle;

    protected ShuffleFromRingsideEffect(View view, int pretendedNumberCardsToShuffle) : base(view)
    {
        _pretendedNumberCardsToShuffle = pretendedNumberCardsToShuffle;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (CurrentPlayer.GetRingsideSize() >= _pretendedNumberCardsToShuffle)
            _actualNumberCardsToShuffle = _pretendedNumberCardsToShuffle;
        else _actualNumberCardsToShuffle = CurrentPlayer.GetRingsideSize();
        RunShuffleLoop();
    }

    protected abstract void RunShuffleLoop();
}