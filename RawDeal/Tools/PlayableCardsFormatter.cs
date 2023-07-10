using RawDeal.Cards;
using RawDeal.Cards.CardPreConditions;
using RawDeal.GameLogic;
using RawDeal.GameLogic.Plays;

namespace RawDeal.Tools;

public class PlayableCardsFormatter
{
    private Card _currentCard;
    private List<string> _formattedPlayableCards;
    public List<IndexedCard> ListOfIndexedCardsOfPlayableCards;
    public List<string> TypesOfPlayableCards;
    private PlayManager _playManager;
    private Play _currentPlay;

    public List<string> GetPlayableCards(PlayManager playManager , List<IndexedCard> playableIndexedCards, 
        int fortitude)
    {
        TypesOfPlayableCards = new List<string>();
        ListOfIndexedCardsOfPlayableCards = new List<IndexedCard>();
        _formattedPlayableCards = new List<string>();
        _playManager = playManager;
        _currentPlay = playManager.GetCurrentPlay();
        RunCardAdditionLoop(_currentPlay, playableIndexedCards, fortitude);
        return _formattedPlayableCards;
    }

    public static List<string> GetReversalCards(List<IndexedCard> reversalIndexedCards)
    {
        List<string> formattedReversalCards = new List<string>();
        foreach (IndexedCard indexedCardIndexInHandAndCard in reversalIndexedCards)
            formattedReversalCards.Add(GetCardInPlayFormat(indexedCardIndexInHandAndCard, "Reversal"));
        return formattedReversalCards;
    }

    private void RunCardAdditionLoop(Play currentPlay, List<IndexedCard> playableIndexedCards, 
        int fortitude)
    {
        foreach (IndexedCard indexedCardIndexInHandAndCard in playableIndexedCards)
        {
            _currentCard = indexedCardIndexInHandAndCard.Card;
            if (_currentCard.IsHybrid)
                AddHybridCardToPlayableCards(currentPlay, indexedCardIndexInHandAndCard, fortitude);
            else
                AddSimpleCardToPlayableCards(indexedCardIndexInHandAndCard);
        }
    }

    private void AddSimpleCardToPlayableCards(IndexedCard indexedCardIndexInHandAndCard)
    {
        _currentCard.PlayedType = _currentCard.Types[0];
        _formattedPlayableCards.Add(GetCardInPlayFormat(indexedCardIndexInHandAndCard,
            _currentCard.PlayedType));
        TypesOfPlayableCards.Add(_currentCard.PlayedType);
        ListOfIndexedCardsOfPlayableCards.Add(indexedCardIndexInHandAndCard);
    }

    private void AddHybridCardToPlayableCards(Play currentPlay, IndexedCard indexedCardWithIndexInHandAndCard, 
        int fortitude)
    {
        foreach (string type in _currentCard.Types)
        {
            _currentCard.PlayedType = type;
            if (PreConditionChecker.IsPlayableCard(_currentCard, _playManager) 
                && _currentCard.CurrentPlayedTypeIsPlayable() &&
                _currentCard.GetCurrentFortitude(currentPlay, _currentCard.PlayedType) <= fortitude)
            {
                _formattedPlayableCards.Add(GetCardInPlayFormat(indexedCardWithIndexInHandAndCard,
                    _currentCard.PlayedType));
                TypesOfPlayableCards.Add(_currentCard.PlayedType);
                ListOfIndexedCardsOfPlayableCards.Add(indexedCardWithIndexInHandAndCard);
            }
        }
    }

    public static List<string> GetInfoOfListOfCards(List<IndexedCard> indexedCards)
    {
        List<string> infoOfCards = new List<string>();
        foreach (IndexedCard indexedCard in indexedCards)
        {
            Card card = indexedCard.Card; 
            infoOfCards.Add(card.GetCardFormattedInfo());
        }

        return infoOfCards;

    }

    private static string GetCardInPlayFormat(IndexedCard indexedCard, string type)
    {
        Card card = indexedCard.Card;
        return card.GetCardInPlayFormat(type);
    }
}