using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class DiscardCardsWithSelection : Effect
{
    private int _numberOfCardsToDiscard;
    private Player _playerThatMustDiscard;
    public DiscardCardsWithSelection(View view, Player player, int numberOfCardsToDiscard) : base(view)
    {
        _numberOfCardsToDiscard = numberOfCardsToDiscard;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        for (int i = 0; i < _numberOfCardsToDiscard; i++)
        {
            int indexCardFromHand = _view.AskPlayerToSelectACardToDiscard(_playerThatMustDiscard.GetCardsInStringFormatFromHand(), _playerThatMustDiscard.GetSuperStarName(),
                _playerThatMustDiscard.GetSuperStarName(), _numberOfCardsToDiscard);
            _playerThatMustDiscard.MoveCardFromHandToRingsideByIndex(indexCardFromHand);
            _numberOfCardsToDiscard--;
        }
    }
}