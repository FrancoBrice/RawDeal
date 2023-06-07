using RawDeal.GameLogic;
using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class MakePlayerDiscardCard : Effect
{
    private Player _playerThatMustDiscard;
    private int _numberOfCardsToDiscard;
    private Game _game;
    public MakePlayerDiscardCard(Game game, Player playerThatMustDiscard, int numberOfCardToDiscard) : base(game.ViewObject)
    {
        _playerThatMustDiscard = playerThatMustDiscard;
        _numberOfCardsToDiscard = numberOfCardToDiscard;
        _game = game;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {

        int remainingCardsToDiscard = _numberOfCardsToDiscard;
        for (int i = 0; i < _numberOfCardsToDiscard; i++)
        {
            if (_playerThatMustDiscard.GetHandSize() == 0) return;
            int indexCardFromHand = _view.AskPlayerToSelectACardToDiscard(_playerThatMustDiscard.GetCardsInStringFormatFromHand(), _playerThatMustDiscard.GetSuperStarName(),
                _playerThatMustDiscard.GetSuperStarName(), remainingCardsToDiscard);
            _playerThatMustDiscard.MoveCardFromHandToRingsideByIndex(indexCardFromHand);
            remainingCardsToDiscard--;
        }
    }
}