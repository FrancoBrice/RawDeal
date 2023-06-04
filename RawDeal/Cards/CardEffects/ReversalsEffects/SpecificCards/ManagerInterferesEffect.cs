using RawDeal.Cards.CardEffects.ReversalsEffects;
using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.SpecificCardEffects;

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
        _view.SayThatPlayerDrawCards(NotCurrentPlayer.GetSuperStarName(), numberOfCardsToDrawByManagerInterferes);
        _cardMobilizer.MoveCardsFromArsenalToHand(NotCurrentPlayer, numberOfCardsToDrawByManagerInterferes);
    }
}