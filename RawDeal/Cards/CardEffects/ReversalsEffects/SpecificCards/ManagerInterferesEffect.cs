using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ReversalsEffects.SpecificCards;

public class ManagerInterferesEffect : Effect
{
    public ManagerInterferesEffect(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        ReversalSimple reversalSimpleEffect = new ReversalSimple(_view);
        reversalSimpleEffect.ApplyEffect(currentPlay);
        const int numberOfCardsToDrawByManagerInterferes = 1;
        _view.SayThatPlayerDrawCards(NotCurrentPlayer.GetSuperStarName(),
            numberOfCardsToDrawByManagerInterferes);
        CardMobilizer.MoveCardsReversedFromArsenalToHand(NotCurrentPlayer,
            numberOfCardsToDrawByManagerInterferes);
    }
}