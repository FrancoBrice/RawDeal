using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class DrawCardsAskingNumber : Effect
{
    private Player _playerThatMustDraw;
    private int _numberOfCardsToDraw; 
    public DrawCardsAskingNumber(View view, Player playerThatMustDraw, int numberOfCardsToDraw) : base(view)
    {
        _playerThatMustDraw = playerThatMustDraw;
        _numberOfCardsToDraw = numberOfCardsToDraw;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        int actualNumberOfCardToDraw =
            _view.AskHowManyCardsToDrawBecauseOfACardEffect(_playerThatMustDraw.GetSuperStarName(),
                _numberOfCardsToDraw);
        if (_playerThatMustDraw.GetArsenalSize() >= _numberOfCardsToDraw)
        {
            _view.SayThatPlayerDrawCards(_playerThatMustDraw.GetSuperStarName(), actualNumberOfCardToDraw);
            _cardMobilizer.MoveCardsFromArsenalToHand(_playerThatMustDraw, actualNumberOfCardToDraw);
        }
    }
}