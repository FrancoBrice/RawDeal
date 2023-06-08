using RawDeal.GameLogic.Plays;

namespace RawDeal.Cards.CardEffects.GeneralEffects.DiscardCards;

public class TopCardOfArsenalToRingsidePile : Effect
{
    private readonly CardCollateralDamage _cardDamager;

    public TopCardOfArsenalToRingsidePile(Game game) : base(game.ViewObject)
    {
        _cardDamager = new CardCollateralDamage(game, game.ViewObject);
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        _view.SayThatPlayerDamagedHimself(CurrentPlayer.GetSuperStarName(), damage: 1);
        _cardDamager.BeginCollateralDamage(1);
    }
}