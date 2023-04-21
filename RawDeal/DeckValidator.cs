namespace RawDeal;

public class DeckValidator
{
    public List<Card> CardList { get; set; }
    public List<SuperStar> SuperStarsList { get; set; }
    private List<string> _allSuperStarLogosList;


    public DeckValidator(List<SuperStar> superStarsList, List<Card> cardList)
    {
        CardList = cardList;
        SuperStarsList = superStarsList;
        _allSuperStarLogosList = JsonReader.GenerateSuperStarLogosList();
    }

    public bool IsValidDeck()
    {
        if (CheckIfRule1IsMet() && CheckIfRule2IsMet() && CheckIfDeckNotContainsHeelAndFace() &&
            CheckIfSuperStarLogoIsCorrect())
        {
            return true;
        }

        return false;
    }

    private bool CheckIfRule1IsMet()
    {
        if (SuperStarsList.Count != 1 || CardList.Count != 60)
        {
            return false;
        }
        return true;
    }

    private bool CheckIfRule2IsMet()
    {
        var groupedCards = CardList.GroupBy(card => card.Title);
        foreach (IGrouping<string, Card> group in groupedCards)
        {
            if (!CheckIfItMeetsTheUniqueCardsRule(group) || !CheckIfItMeetsTheSetupCardsRule(group))
            {
                return false;
            }
        }
        return true;

    }

    private bool CheckIfItMeetsTheUniqueCardsRule(IGrouping<string, Card> groupOfCards)
    {
        int amountEqualCards = groupOfCards.Count();
        if (groupOfCards.First().ItsUnique() && amountEqualCards > 1)
        {
            return false;
        }
        return true;
    }

    private bool CheckIfItMeetsTheSetupCardsRule(IGrouping<string, Card> groupOfCards)
    {
        int amountEqualCards = groupOfCards.Count();
        if (!groupOfCards.First().ItsSetUp() && amountEqualCards > 3)
        {
            return false;
        }
        return true;
    }

    private bool CheckIfDeckNotContainsHeelAndFace()
    {
        bool hasHeel = CardList.Any(card => card.HasSubtypeHeel());
        bool hasFace = CardList.Any(card => card.HasSubtypeFace());
        if (hasHeel && hasFace)
        {
            return false;
        }
        return true;
    }

    private bool CheckIfSuperStarLogoIsCorrect()
    {
        string superstarLogo = SuperStarsList.First().Logo;
        var invalidCards =
            CardList.Where(card => card.Subtypes.Any(s => s != superstarLogo && _allSuperStarLogosList.Contains(s)));
        if (invalidCards.Any())
        {
            return false;
        }
        return true;
    }
    
}