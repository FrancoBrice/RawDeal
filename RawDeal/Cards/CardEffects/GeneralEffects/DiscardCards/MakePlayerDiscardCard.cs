using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.DiscardCards;

public class MakePlayerDiscardCard : Effect
{
    private readonly int _numberOfCardsToDiscard;
    private readonly Player _playerThatMustDiscard;

    public MakePlayerDiscardCard(View view, Player playerThatMustDiscard, 
        int numberOfCardToDiscard) : base(view)
    {
        _playerThatMustDiscard = playerThatMustDiscard;
        _numberOfCardsToDiscard = numberOfCardToDiscard;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        int remainingCardsToDiscard = _numberOfCardsToDiscard;
        for (int i = 0; i < _numberOfCardsToDiscard; i++)
        {
            if (_playerThatMustDiscard.GetHandSize() == 0) return;
            int indexCardFromHand = GetIndexCardFromHand(remainingCardsToDiscard);
            CardMobilizer.FromHandToRingsideByIndex(_playerThatMustDiscard, indexCardFromHand);
            remainingCardsToDiscard--;
        }
    }

    private int GetIndexCardFromHand(int remainingCardsToDiscard)
    {
        return _view.AskPlayerToSelectACardToDiscard(
            _playerThatMustDiscard.GetCardsInStringFormatFromHand(),
            _playerThatMustDiscard.GetSuperStarName(),
            _playerThatMustDiscard.GetSuperStarName(), 
            remainingCardsToDiscard);
    }
}