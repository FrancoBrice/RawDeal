using RawDeal.Cards;

namespace RawDeal.Tools;

public class PlayableCardsFormatter
{
    private TupleManager _tupleManager;
    public List<string> TypesOfPlayableCards;
    public List<(int, Card)> ListOfTuplesOfPlayableCards;
    private Card _currentCard;
    private List<string> _formattedPlayableCards;
    
    public PlayableCardsFormatter()
    {
        _tupleManager = new TupleManager();
    }

    public List<string> GetPlayableCards(List<(int, Card)> playableCardsTuples)
    {
        TypesOfPlayableCards = new List<string>();
        ListOfTuplesOfPlayableCards = new List<(int, Card)>();
        _formattedPlayableCards = new List<string>();
        RunCardAdditionLoop(playableCardsTuples);
        return _formattedPlayableCards;
    }

    private void RunCardAdditionLoop(List<(int, Card)> playableCardsTuples)
    {
        foreach (var tupleIndexInHandAndCard in playableCardsTuples)
        {
            _currentCard = _tupleManager.ExtractCard(tupleIndexInHandAndCard);
            if (_currentCard.IsHybrid)
            {
                AddHybridCardToPlayableCards(tupleIndexInHandAndCard);
            }
            else
            {
                AddSimpleCardToPlayableCards(tupleIndexInHandAndCard);
            }
        }
    }

    private void AddSimpleCardToPlayableCards((int, Card) tupleIndexInHandAndCard)
    {
        _currentCard.PlayedType = _currentCard.Types[0];
        _formattedPlayableCards.Add(GetCardInPlayFormat(tupleIndexInHandAndCard, _currentCard.PlayedType));
        TypesOfPlayableCards.Add(_currentCard.PlayedType);
        ListOfTuplesOfPlayableCards.Add(tupleIndexInHandAndCard);
    }

    private void AddHybridCardToPlayableCards((int, Card) tupleIndexInHandAndCard)
    {
        for (int j = 0; j < _currentCard.AmountOfTypes; j++)
        {
            _currentCard.PlayedType = _currentCard.Types[j];
            if (_currentCard.CurrentPlayedTypeIsPlayable())
            {
                _formattedPlayableCards.Add(GetCardInPlayFormat(tupleIndexInHandAndCard, _currentCard.PlayedType));
                TypesOfPlayableCards.Add(_currentCard.PlayedType);
                ListOfTuplesOfPlayableCards.Add(tupleIndexInHandAndCard);
            }
        }
    }


    public List<string> GetReversalCards(List<(int, Card)> reversalCardsTuples)
    {
        List<string> formattedReversalCards = new List<string>();
        foreach (var tupleIndexInHandAndCard in reversalCardsTuples)
        {
            formattedReversalCards.Add(GetCardInPlayFormat(tupleIndexInHandAndCard, "Reversal"));
        }
        
        return formattedReversalCards;
    }
    
    private string GetCardInPlayFormat((int, Card) tuple, string type)
    {
        Card card = _tupleManager.ExtractCard(tuple);
        return card.GetCardInPlayFormat(type);
    }

}