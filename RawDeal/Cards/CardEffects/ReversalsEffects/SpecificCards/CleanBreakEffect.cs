using RawDeal.Cards.CardEffects.ReversalsEffects;
using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.SpecificCardEffects;

public class CleanBreakEffect : Effect
{
    public CleanBreakEffect(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        ReversalByTitle reversalByTitleEffect = new ReversalByTitle(_view, cardTitleThatCanReverse:"Jockeying for Position");
        reversalByTitleEffect.ApplyEffect(currentPlay);
        const int numberOfCardsThatOpponentMustDiscard = 4;
        _cardMobilizer.MakePlayerDiscardCards(CurrentPlayer, numberOfCardsThatOpponentMustDiscard);
        const int numberOfCardsToDrawByCleanBreak = 1;
        _cardMobilizer.MoveCardsFromArsenalToHand(NotCurrentPlayer, numberOfCardsToDrawByCleanBreak);
        _view.SayThatPlayerDrawCards(NotCurrentPlayer.GetSuperStarName(), numberOfCardsToDrawByCleanBreak);
    }
}