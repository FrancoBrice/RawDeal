using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class OpponentDrawCards : Effect
{
    private Player _playerThatMustDraw;
    private int _numberOfCardsToDraw; 
    public OpponentDrawCards(View view, Player playerThatMustDraw, int numberOfCardsToDraw) : base(view)
    {
        _playerThatMustDraw = playerThatMustDraw;
        _numberOfCardsToDraw = numberOfCardsToDraw;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (_playerThatMustDraw.GetArsenalSize() >= _numberOfCardsToDraw)
        {
            _cardMobilizer.MoveCardsFromArsenalToHand(_playerThatMustDraw, _numberOfCardsToDraw);
        }
    }
}