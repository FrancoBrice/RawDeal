using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.DiscardCards;

public class TopCardOfArsenalToRingsidePile : Effect
{
    private readonly CollateralDamageController _damageController;

    public TopCardOfArsenalToRingsidePile(View view, Game game) : base(view)
    {
        _damageController = new CollateralDamageController(game, view);
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        _view.SayThatPlayerDamagedHimself(_currentPlayer.GetSuperStarName(), damage: 1);
        _damageController.BeginCollateralDamage(1);
    }
}