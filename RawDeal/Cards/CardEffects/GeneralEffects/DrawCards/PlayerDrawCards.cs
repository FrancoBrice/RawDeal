using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.DrawCards;

public class PlayerDrawCards : Effect
{
    private readonly int _numberOfCardsToDraw;
    private readonly Player _player;

    public PlayerDrawCards(View view, Player player, int numberOfCardsToDraw) : base(view)
    {
        _player = player;
        _numberOfCardsToDraw = numberOfCardsToDraw;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (_player.GetArsenalSize() < _numberOfCardsToDraw) return;
        _view.SayThatPlayerDrawCards(_player.GetSuperStarName(),
            _numberOfCardsToDraw);
        CardMobilizer.FromArsenalToHand(_player, _numberOfCardsToDraw);
    }
}