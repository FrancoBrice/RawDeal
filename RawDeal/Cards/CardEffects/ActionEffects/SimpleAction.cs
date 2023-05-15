using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ActionEffects;

public class SimpleAction : Effect
{
    public SimpleAction(View view) : base(view)
    {
    }

    public override void ApplyEffect(Play currentPlay)
    {
        (int, Card) tupleWithIndexInHandAndAttackingCard = currentPlay.AttackingCardTuple;
        Card attackingCard = currentPlay.AttackingCard;
        _cardMobilizer.MoveCardFromHandToRingside(currentPlay.CurrentPlayer, tupleWithIndexInHandAndAttackingCard);
        _view.SayThatPlayerSuccessfullyPlayedACard();
        currentPlay.CurrentPlayer.MoveCardFromArsenalToHand();
        _view.SayThatPlayerMustDiscardThisCard(currentPlay.CurrentPlayer.GetSuperStarName(), attackingCard.Title);
        _view.SayThatPlayerDrawCards(currentPlay.CurrentPlayer.GetSuperStarName(), 1);
    }

}