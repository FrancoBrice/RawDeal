using RawDeal.Cards;

namespace RawDeal.Tools;

public class GameFormatter
{
    private TupleManager _tupleManager;

    public GameFormatter()
    {
        _tupleManager = new TupleManager();
    }

    public List<string> GetFormattedPlayableCards(List<(int, Card)> playableCardsTuples)
    {
        List<string> _typesOfPlayableCards = new List<string>();
        List<(int, Card)> _listOfTuplesOfPlayableCards = new List<(int, Card)>();
        List<string> formattedPlayableCards = new List<string>();
        foreach (var tupleIndexInHandAndCard in playableCardsTuples)
        {
            Card currentCard = _tupleManager.ExtractCard(tupleIndexInHandAndCard);
            if (currentCard.IsHybrid)
            {
                for (int j = 0; j < currentCard.AmountOfTypes; j++)
                {
                    currentCard.PlayedType = currentCard.Types[j];
                    if (currentCard.CurrentPlayedTypeIsPlayable())
                    {
                        formattedPlayableCards.Add(GetCardInPlayFormat(tupleIndexInHandAndCard, currentCard.PlayedType));
                        _typesOfPlayableCards.Add(currentCard.PlayedType);
                        _listOfTuplesOfPlayableCards.Add(tupleIndexInHandAndCard);
                    }
                }
            }
            else
            {
                currentCard.PlayedType = currentCard.Types[0];
                formattedPlayableCards.Add(GetCardInPlayFormat(tupleIndexInHandAndCard, currentCard.PlayedType));
                _typesOfPlayableCards.Add(currentCard.PlayedType);
                _listOfTuplesOfPlayableCards.Add(tupleIndexInHandAndCard);
            }
        }
        
        return formattedPlayableCards;
    }
    
    
    private List<string> GetFormattedReversalCards(List<(int, Card)> reversalCardsTuples)
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