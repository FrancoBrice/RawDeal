using RawDeal.GameLogic;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.MovementEffects;

public class PlaceCardFromHandAtArsenalEnd : Effect
{
    private IndexedCard _indexedCardToMove; 
    public PlaceCardFromHandAtArsenalEnd(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        _indexedCardToMove = currentPlay.AttackingIndexedCard;
        int index = _indexedCardToMove.Index; 
        Card cardToMove = _indexedCardToMove.Card; 
        CardMobilizer.MoveFromHandToArsenalBeginningByIndex(_currentPlayer, index);
        _view.SayThatPlayerPutsThisCardAtTheBottomOfHisArsenal(_currentPlayer.GetSuperStarName(), 
            cardToMove.Title);
    }
}