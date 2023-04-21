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
        if (CheckIfSatisfyRuleOne() && CheckIfSatisfyRuleTwo() && CheckIfSatisfyRuleThree() && CheckIfSatisfyRuleFour())
        {
            return true;
        }
        return false;
    }

    private bool CheckIfSatisfyRuleOne()
    {
        if (CheckIfOnlyHasOneSuperStar() && CheckIfHasCorrectNumberOfCards())
        {
            return true;
        }
        return false;
    }
    private bool CheckIfOnlyHasOneSuperStar()
    {
        if (SuperStarsList.Count != 1) return false;
        return true;
    }
    private bool CheckIfHasCorrectNumberOfCards()
    {
        if (CardList.Count != 60) return false;
        return true;
    }
    

    private bool CheckIfSatisfyRuleTwo()
    {
        var groupedCardsByTitle = CardList.GroupBy(card => card.Title);
        foreach (IGrouping<string, Card> group in groupedCardsByTitle)
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
        if (groupOfCards.First().ItsUnique() && amountEqualCards > 1) return false;
        return true;
    }

    private bool CheckIfItMeetsTheSetupCardsRule(IGrouping<string, Card> groupOfCards)
    {
        int amountEqualCards = groupOfCards.Count();
        if (!groupOfCards.First().ItsSetUp() && amountEqualCards > 3) return false;
        return true;
    }

    private bool CheckIfSatisfyRuleThree()
    {
        if(CheckIfDeckNotContainsHeelAndFace()) return true;
        return false;
    }
    private bool CheckIfDeckNotContainsHeelAndFace()
    {
        bool hasHeel = CardList.Any(card => card.HasSubtypeHeel());
        bool hasFace = CardList.Any(card => card.HasSubtypeFace());
        if (hasHeel && hasFace) return false; 
        return true;
    }

    private bool CheckIfSatisfyRuleFour()
    {
        if (CheckIfSuperStarLogoIsCorrect()) return true;
        return false;
    }
    private bool CheckIfSuperStarLogoIsCorrect()
    {
        IEnumerable<Card> invalidCards = FoundCardsWithIncorrectLogo();
        if (invalidCards.Any()) return false;
        return true;
    }

    private IEnumerable<Card> FoundCardsWithIncorrectLogo()
    {
        string superstarLogo = SuperStarsList.First().Logo;
        return CardList.Where(card => card.Subtypes.Any(s => s != superstarLogo && _allSuperStarLogosList.Contains(s)));
    }
    
}