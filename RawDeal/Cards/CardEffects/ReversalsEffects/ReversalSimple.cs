using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ReversalsEffects;

public class ReversalSimple : Effect
{

    public ReversalSimple(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (ReversalCard.Damage != "#") ReversalCard.SetDefaultValues();
        if (ReversalCard.PlayedFrom == "Hand")
        {
            _cardMobilizer.MoveSpecificCardFromHandToRingside(CurrentPlayer, AttackingCardTuple);
            CurrentPlayer.HasEndsHisTurn = true;
            _view.SayThatPlayerReversedTheCard(NotCurrentPlayer.GetSuperStarName(), ReversalCard.GetCardInPlayFormat(ReversalCard.PlayedType));
            _cardMobilizer.MoveCardFromHandToRingArea(NotCurrentPlayer, ReversalCardTuple);
        }
        else if (ReversalCard.PlayedFrom == "Deck")
        {
            CurrentPlayer.HasEndsHisTurn = true;
        }
        
    }


}