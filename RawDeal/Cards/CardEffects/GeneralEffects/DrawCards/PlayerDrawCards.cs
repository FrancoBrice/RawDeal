using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.DrawCards;

public class PlayerDrawCards : Effect
{
    private readonly int _numberOfCardsToDraw;
    private readonly Player _playerThatMustDraw;

    public PlayerDrawCards(View view, Player playerThatMustDraw, int numberOfCardsToDraw) : base(view)
    {
        _playerThatMustDraw = playerThatMustDraw;
        _numberOfCardsToDraw = numberOfCardsToDraw;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (_playerThatMustDraw.GetArsenalSize() < _numberOfCardsToDraw) return;
        _view.SayThatPlayerDrawCards(_playerThatMustDraw.GetSuperStarName(),
            _numberOfCardsToDraw);
        CardMobilizer.MoveCardsFromArsenalToHand(_playerThatMustDraw, _numberOfCardsToDraw);
    }
}