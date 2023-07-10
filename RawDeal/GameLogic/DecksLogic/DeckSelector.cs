using RawDeal.CardCollections;
using RawDeal.Cards;
using RawDeal.JsonReaders;
using RawDeal.SuperStars;
using RawDealView;

namespace RawDeal.GameLogic.DecksLogic;

public class DeckSelector
{
    private readonly Game _game;
    private readonly View _view;
    private readonly List<Card> _allCardsList;
    private readonly SuperStar[] _allSuperStarArray;

    public DeckSelector(Game game, View view)
    {
        _game = game;
        _view = view;
        _allCardsList = CardsJsonReader.GenerateAllCardsListFromCardsFromJson();
        _allSuperStarArray = SuperstarsJsonReader.GenerateAllSuperStarsArrayFromJson();
    }

    public void AskUsersToSelectDecks(string deckFolder)
    {
        for (int i = 0; i < 2; i++)
        {
            string deckPath = _view.AskUserToSelectDeck(deckFolder);
            DeckValidator deck = GetDeckFromPath(deckPath);
            if (IsInvalidDeck(deck)) return;
            _game.AddDeckValidator(deck);
        }
    }

    public bool AreDecksValid()
    {
        const int correctNumberOfDecks = 2;
        return _game.GetSelectedDecksSize() == correctNumberOfDecks;
    }

    private DeckValidator GetDeckFromPath(string path)
    {
        List<Card> cards = GetCardsFromDeck(path);
        SuperStar[] superStars = GetSuperStarsArrayFromDeck(path);
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
        IEnumerable<string> cardStrings = GetCardsThatNotContainsWordSuperstarCard(path);
        return GenerateCardList(cardStrings);
    }

    private List<Card> GenerateCardList(IEnumerable<string> cardStrings)
    {
        List<Card> cardsList = new List<Card>();
        foreach (string cardString in cardStrings)
        {
            Card? card = _allCardsList.FirstOrDefault(card => card.Title == cardString);
            cardsList.Add(card);
        }
        return cardsList;
    }

    private SuperStar[] GetSuperStarsArrayFromDeck(string path)
    {
        IEnumerable<string> superStarStringsList = GetCardsThatContainsWordSuperstarCard(path);
        SuperStar[] superStarsArray = new SuperStar[superStarStringsList.Count()];
        superStarsArray = AddSuperStarsToArray(superStarStringsList, superStarsArray);
        return superStarsArray;
    }

    private SuperStar[] AddSuperStarsToArray(IEnumerable<string> superStarStringsList, SuperStar[] superStarsArray)
    {
        int index = 0;
        foreach (string superstarString in superStarStringsList)
        {
            string cardName = DeleteWordSuperstarCardInString(superstarString);
            SuperStar superstar = _allSuperStarArray.FirstOrDefault(superstar => superstar.Name == cardName);
            if (superstar == null) continue;
            superStarsArray[index] = superstar;
            index++;
        }
        Array.Resize(ref superStarsArray, index);
        return superStarsArray;
    }

    private static string DeleteWordSuperstarCardInString(string superstarString)
    {
        return superstarString.Replace(" (Superstar Card)", "");
    }

    private static IEnumerable<string> GetCardsThatContainsWordSuperstarCard(string path)
    {
        return File.ReadAllLines(path).Where(line => line.Contains("(Superstar Card)"));
    }
    
    private static IEnumerable<string> GetCardsThatNotContainsWordSuperstarCard(string path)
    {
        return File.ReadAllLines(path).Where(line => !line.Contains("(Superstar Card)"));
    }

}