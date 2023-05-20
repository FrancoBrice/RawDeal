using RawDeal.Cards.CardEffects.ReversalsEffects;
using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.SpecificCardEffects;

public class ChynaInterferesEffect : Effect
{
    public ChynaInterferesEffect(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        ReversalSimple simpleReversalEffect = new ReversalSimple(_view);
        simpleReversalEffect.ApplyEffect(currentPlay);
        const int numberOfCardsToDrawByChynaInterferes = 2;
        _cardMobilizer.MoveCardsFromArsenalToHand(NotCurrentPlayer, numberOfCardsToDrawByChynaInterferes);
        _view.SayThatPlayerDrawCards(NotCurrentPlayer.GetSuperStarName(), numberOfCardsToDrawByChynaInterferes);
    }
}