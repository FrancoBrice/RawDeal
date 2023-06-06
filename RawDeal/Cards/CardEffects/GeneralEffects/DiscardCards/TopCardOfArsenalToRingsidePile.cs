using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class TopCardOfArsenalToRingsidePile : Effect
{
    CardDamager _cardDamager;
    public TopCardOfArsenalToRingsidePile(Game game) : base(game.ViewObject)
    {
        _cardDamager = new CardDamager(game, game.ViewObject);
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        _view.SayThatPlayerDamagedHimself(CurrentPlayer.GetSuperStarName(), damage: 1);
        _cardDamager.ApplyCollateralCardDamage(damageAmount: 1);
    }
}