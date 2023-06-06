using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class DrawCards : Effect
{
    private Player _playerThatMustDraw;
    private int _numberOfCardsToDraw; 
    public DrawCards(View view, Player playerThatMustDraw, int numberOfCardsToDraw) : base(view)
    {
        _playerThatMustDraw = playerThatMustDraw;
        _numberOfCardsToDraw = numberOfCardsToDraw;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (_playerThatMustDraw.GetArsenalSize() >= _numberOfCardsToDraw)
        {
            _view.SayThatPlayerDrawCards(_playerThatMustDraw.GetSuperStarName(), numberOfCardsToDraw: _numberOfCardsToDraw);
            CardMobilizer.MoveCardsFromArsenalToHand(_playerThatMustDraw, _numberOfCardsToDraw);
        }
    }
}