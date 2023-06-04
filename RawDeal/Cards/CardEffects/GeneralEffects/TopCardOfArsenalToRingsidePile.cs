using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class TopCardOfArsenalToRingsidePile : Effect
{
    CardDamager _cardDamager;
    public TopCardOfArsenalToRingsidePile(View view, Game game) : base(view)
    {
        _cardDamager = new CardDamager(game, _view);
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        _view.SayThatPlayerDamagedHimself(CurrentPlayer.GetSuperStarName(), damage: 1);
        _cardDamager.ApplyCollateralCardDamage(damageAmount: 1);
    }
}