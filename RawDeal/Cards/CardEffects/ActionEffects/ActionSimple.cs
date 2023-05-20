using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ActionEffects;

public class ActionSimple : Effect
{
    public ActionSimple(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        _cardMobilizer.MoveSpecificCardFromHandToRingside(CurrentPlayer, AttackingCardTuple);
        _view.SayThatPlayerSuccessfullyPlayedACard();
        CurrentPlayer.MoveCardFromArsenalToHand();
        _view.SayThatPlayerMustDiscardThisCard(CurrentPlayer.GetSuperStarName(), AttackingCard.Title);
        _view.SayThatPlayerDrawCards(CurrentPlayer.GetSuperStarName(), 1);
    }

}