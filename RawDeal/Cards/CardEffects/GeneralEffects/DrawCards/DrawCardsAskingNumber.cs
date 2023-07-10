using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.DrawCards;

public class DrawCardsAskingNumber : Effect
{
    private readonly int _numberOfCardsToDraw;
    private readonly Player _playerThatMustDraw;

    public DrawCardsAskingNumber(View view, Player playerThatMustDraw, int numberOfCardsToDraw) :
        base(view)
    {
        _playerThatMustDraw = playerThatMustDraw;
        _numberOfCardsToDraw = numberOfCardsToDraw;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        int actualNumberOfCardToDraw =
            _view.AskHowManyCardsToDrawBecauseOfACardEffect(_playerThatMustDraw.GetSuperStarName(),
                _numberOfCardsToDraw);
        _view.SayThatPlayerDrawCards(_playerThatMustDraw.GetSuperStarName(),
            actualNumberOfCardToDraw);
        if (_playerThatMustDraw.GetArsenalSize() < _numberOfCardsToDraw) return;
        CardMobilizer.MoveFromArsenalToHandByAmount(_playerThatMustDraw, actualNumberOfCardToDraw);
    }
}