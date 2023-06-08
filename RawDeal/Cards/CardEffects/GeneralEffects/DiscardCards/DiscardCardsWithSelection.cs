using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.DiscardCards;

public class DiscardCardsWithSelection : Effect
{
    private readonly int _numberOfCardsToDiscard;
    private readonly Player _playerThatMustDiscard;

    public DiscardCardsWithSelection(View view, Player player, int numberOfCardsToDiscard) :
        base(view)
    {
        _numberOfCardsToDiscard = numberOfCardsToDiscard;
        _playerThatMustDiscard = player;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        int remainingCardToDiscard = _numberOfCardsToDiscard;
        for (int i = 0; i < _numberOfCardsToDiscard; i++)
        {
            int indexCardFromHand = _view.AskPlayerToSelectACardToDiscard(
                _playerThatMustDiscard.GetCardsInStringFormatFromHand(),
                _playerThatMustDiscard.GetSuperStarName(),
                _playerThatMustDiscard.GetSuperStarName(), 
                remainingCardToDiscard);
            _playerThatMustDiscard.MoveCardFromHandToRingsideByIndex(indexCardFromHand);
            remainingCardToDiscard--;
        }
    }
}