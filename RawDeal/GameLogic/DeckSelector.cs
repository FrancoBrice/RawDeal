using RawDeal.Cards;
using RawDeal.SuperStars;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.GameLogic;

public class DeckSelector
{
    private Game _game;
    private View _view;

    public DeckSelector(Game game, View view)
    {
        _game = game;
        _view = view;
    }

    public void AskUsersToSelectDecks(string deckFolder)
    {
        for (int i = 0; i < 2; i++)
        {
            string deckPath = _view.AskUserToSelectDeck(deckFolder);
            DeckValidator deck = GetDeckFromPath(deckPath);
            if (IsInvalidDeck(deck)) return;
            _game.SelectedDecks.Add(deck);
        }
    }
    
    private DeckValidator GetDeckFromPath(string path)
    {
        List<Card> cards = GetCardsFromDeck(path);
        List<SuperStar> superStars = GetSuperStarsListFromDeck(path);
        return new DeckValidator(superStars, cards);
    }
    
    private bool IsInvalidDeck(DeckValidator deck)
    {
        if (deck.IsValidDeck()) return false;
        _view.SayThatDeckIsInvalid();
        return true;
    }
    
    private List<Card> GetCardsFromDeck(string path)
    {
        var cardStrings = File.ReadAllLines(path).Where(line => !line.Contains("(Superstar Card)"));
        List<Card> cardsList = new List<Card>();
        foreach (var cardString in cardStrings)
        {
            var card = _game.AllCardsList.FirstOrDefault(card => card.Title == cardString);
            cardsList.Add(card);
        }
        return cardsList;
    }
    
    private List<SuperStar> GetSuperStarsListFromDeck(string path)
    {
        var superStarStringsList = File.ReadAllLines(path).Where(line => line.Contains("(Superstar Card)"));
        List<SuperStar> superStarsList = new List<SuperStar>();

        foreach (var superstarString in superStarStringsList)
        {
            string cardName = superstarString.Replace(" (Superstar Card)", "");
            var superstar = _game.AllSuperStarList.FirstOrDefault(superstar => superstar.Name == cardName);
            superStarsList.Add(superstar);
        }

        return superStarsList;
    }

    public bool AreDecksValid()
    {
        const int correctNumberOfDecks = 2;
        return _game.SelectedDecks.Count == correctNumberOfDecks;
    }
    
    

}