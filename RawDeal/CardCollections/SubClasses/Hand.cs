using RawDeal.Cards;
using RawDeal.Cards.CardPreConditions;
using RawDeal.GameLogic;
using RawDeal.GameLogic.Plays;

namespace RawDeal.CardCollections.SubClasses;

public class Hand : CardCollection
{
    private PlayManager _playManager;
    private List<IndexedCard> _positionInHandAndPlayableCards;

    public Hand()
    {
        _positionInHandAndPlayableCards = new List<IndexedCard>();
    }
    public List<IndexedCard> GetIndexedCardsWithPositionInHandAndPlayableCards(PlayManager playManager)
    {
        _playManager = playManager;
        _positionInHandAndPlayableCards = new List<IndexedCard>();
        for (int indexInHand = 0; indexInHand < CardList.Count; indexInHand++)
        {
            Card card = CardList[indexInHand];
            card.PlayedType = "";
            AddPlayableCardIfPossible(card, _positionInHandAndPlayableCards, indexInHand);
        }
        return _positionInHandAndPlayableCards;
    }

    private void AddPlayableCardIfPossible(Card card,
        List<IndexedCard> indexedCardsWithPositionInHandAndPlayableCards, int indexInHand)
    {
        if (PreConditionChecker.IsPlayableCard(card, _playManager))
        {
            indexedCardsWithPositionInHandAndPlayableCards.Add(new IndexedCard(indexInHand, card));
        }
    }

    public List<IndexedCard> GetIndexedCardsWithPositionInHandAndReversalCards(PlayManager playManager)
    {
        _playManager = playManager;
        _positionInHandAndPlayableCards = new List<IndexedCard>();
        for (int indexInHand = 0; indexInHand < CardList.Count; indexInHand++)
        {
            Card reversalCard = CardList[indexInHand];
            reversalCard.PlayedFrom = "Hand";
            AddReversalCardIfPossible(reversalCard, indexInHand);
        }

        return _positionInHandAndPlayableCards;
    }

    private void AddReversalCardIfPossible(Card reversalCard, int indexInHand)
    {
        if (ReversalsChecker.IsCorrectReversalCard(_playManager, reversalCard))
            _positionInHandAndPlayableCards.Add(new IndexedCard(indexInHand, reversalCard));
    }
}