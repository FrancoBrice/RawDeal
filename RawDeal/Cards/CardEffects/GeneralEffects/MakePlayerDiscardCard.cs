using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class MakePlayerDiscardCard : Effect
{
    private Player _playerThatMustDiscard;
    private int _numberOfCardsToDiscard;
    public MakePlayerDiscardCard(View view, Player playerThatMustDiscard, int numberOfCardToDiscard) : base(view)
    {
        _playerThatMustDiscard = playerThatMustDiscard;
        _numberOfCardsToDiscard = numberOfCardToDiscard;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (_playerThatMustDiscard.GetHandSize() == 0)
        {
            return;
        }
        int remainingCardsToDiscard = _numberOfCardsToDiscard;
        for (int i = 0; i < _numberOfCardsToDiscard; i++)
        {
            int indexCardFromHand = _view.AskPlayerToSelectACardToDiscard(_playerThatMustDiscard.GetCardsInStringFormatFromHand(), _playerThatMustDiscard.GetSuperStarName(),
                _playerThatMustDiscard.GetSuperStarName(), remainingCardsToDiscard);
            _playerThatMustDiscard.MoveCardFromHandToRingsideByIndex(indexCardFromHand);
            remainingCardsToDiscard--;
        }
    }
}