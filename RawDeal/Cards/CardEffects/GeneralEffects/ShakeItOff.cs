using RawDeal.GameLogic;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class ShakeItOff : Effect
{
    public ShakeItOff(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        List<Card>? opponentRingAreaCards = _notCurrentPlayer.GetAllRingAreaCards();
        List<IndexedCard> cardsThatCanBeDiscarded = GetCardsThatCanBeDiscarded(opponentRingAreaCards);
        if (!AreCardsThatCanBeDiscarded(cardsThatCanBeDiscarded)) return;
        int indexInput = _view.AskPlayerToSelectACardToDiscardFromRingArea(_currentPlayer.GetSuperStarName(),
            _notCurrentPlayer.GetSuperStarName(),
            PlayableCardsFormatter.GetInfoOfListOfCards(cardsThatCanBeDiscarded));
        CardMobilizer.MoveFromRingAreaToRingSideByIndex(_notCurrentPlayer, indexInput);
    }

    private List<IndexedCard> GetCardsThatCanBeDiscarded(List<Card> opponentRingAreaCards)
    {
        List<IndexedCard> possibleCards = new List<IndexedCard>();
        for (int indexInRingArea = 0; indexInRingArea < opponentRingAreaCards.Count; indexInRingArea++)
        {
            Card card = opponentRingAreaCards[indexInRingArea];
            if (card.GetDefaultDamage() < _currentPlayer.Fortitude) 
                possibleCards.Add(new IndexedCard(indexInRingArea, card));
        }
        return possibleCards;
    }

    private bool AreCardsThatCanBeDiscarded(List<IndexedCard> possibleCards)
    {
        if (possibleCards.Count != 0) return true;
        _view.SayThatNoCardMeetsTheConditionsToBeRemoved();
        return false;
    }
}