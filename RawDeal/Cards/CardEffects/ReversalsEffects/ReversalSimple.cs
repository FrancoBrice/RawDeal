using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ReversalsEffects;

public class ReversalSimple : Effect
{

    public ReversalSimple(View view) : base(view)
    {
    }
    
    public override void ApplyEffect(Play currentPlay)
    {
        SetPlayers(currentPlay);
        _cardMobilizer.MoveCardFromHandToRingside(CurrentPlayer, currentPlay.AttackingCardTuple);
        CurrentPlayer.EndsHisTurn = true;
        _view.SayThatPlayerReversedTheCard(NotCurrentPlayer.GetSuperStarName(), currentPlay.ReversalCard.GetCardInPlayFormat(currentPlay.ReversalCard.PlayedType));
        _cardMobilizer.MoveCardFromHandToRingArea(NotCurrentPlayer, currentPlay.ReversalCardTuple);
    }

    private void SetPlayers(Play currentPlay)
    {
        CurrentPlayer = currentPlay.CurrentPlayer;
        NotCurrentPlayer = currentPlay.NotCurrentPlayer;
    }
}