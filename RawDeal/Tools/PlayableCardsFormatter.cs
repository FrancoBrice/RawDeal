using RawDeal.Cards;

namespace RawDeal.Tools;

public class PlayableCardsFormatter
{
    private Card _currentCard;
    private List<string> _formattedPlayableCards;
    public List<(int, Card)> ListOfTuplesOfPlayableCards;
    public List<string> TypesOfPlayableCards;

    public List<string> GetPlayableCards(List<(int, Card)> playableCardsTuples, int fortitude)
    {
        TypesOfPlayableCards = new List<string>();
        ListOfTuplesOfPlayableCards = new List<(int, Card)>();
        _formattedPlayableCards = new List<string>();
        RunCardAdditionLoop(playableCardsTuples, fortitude);
        return _formattedPlayableCards;
    }

    public static List<string> GetReversalCards(List<(int, Card)> reversalCardsTuples)
    {
        List<string> formattedReversalCards = new();
        foreach ((int, Card) tupleIndexInHandAndCard in reversalCardsTuples)
            formattedReversalCards.Add(GetCardInPlayFormat(tupleIndexInHandAndCard, "Reversal"));

        return formattedReversalCards;
    }

    private void RunCardAdditionLoop(List<(int, Card)> playableCardsTuples, int fortitude)
    {
        foreach ((int, Card) tupleIndexInHandAndCard in playableCardsTuples)
        {
            _currentCard = TupleManager.ExtractCard(tupleIndexInHandAndCard);
            if (_currentCard.IsHybrid)
                AddHybridCardToPlayableCards(tupleIndexInHandAndCard, fortitude);
            else
                AddSimpleCardToPlayableCards(tupleIndexInHandAndCard);
        }
    }

    private void AddSimpleCardToPlayableCards((int, Card) tupleIndexInHandAndCard)
    {
        _currentCard.PlayedType = _currentCard.Types[0];
        _formattedPlayableCards.Add(GetCardInPlayFormat(tupleIndexInHandAndCard,
            _currentCard.PlayedType));
        TypesOfPlayableCards.Add(_currentCard.PlayedType);
        ListOfTuplesOfPlayableCards.Add(tupleIndexInHandAndCard);
    }

    private void AddHybridCardToPlayableCards((int, Card) tupleIndexInHandAndCard, int fortitude)
    {
        foreach (string type in _currentCard.Types)
        {
            _currentCard.PlayedType = type;
            if (_currentCard.CurrentPlayedTypeIsPlayable() &&
                _currentCard.GetCurrentFortitude(_currentCard.PlayedType) <= fortitude)
            {
                _formattedPlayableCards.Add(GetCardInPlayFormat(tupleIndexInHandAndCard,
                    _currentCard.PlayedType));
                TypesOfPlayableCards.Add(_currentCard.PlayedType);
                ListOfTuplesOfPlayableCards.Add(tupleIndexInHandAndCard);
            }
        }
    }

    private static string GetCardInPlayFormat((int, Card) tuple, string type)
    {
        Card card = TupleManager.ExtractCard(tuple);
        return card.GetCardInPlayFormat(type);
    }
}