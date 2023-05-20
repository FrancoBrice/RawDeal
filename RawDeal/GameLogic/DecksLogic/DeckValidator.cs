using RawDeal.Cards;
using RawDeal.JsonReader;
using RawDeal.SuperStars;

namespace RawDeal.Tools;

public class DeckValidator
{
    public List<Card> CardList { get; set; }
    public List<SuperStar> SuperStarsList { get; set; }
    private List<string> _allSuperStarLogosList;
    
    public DeckValidator(List<SuperStar> superStarsList, List<Card> cardList)
    {
        CardList = cardList;
        SuperStarsList = superStarsList;
        _allSuperStarLogosList = SuperstarsJsonReader.GenerateSuperStarLogosList();
    }

    public bool IsValidDeck()
    {
        if (SatisfyRuleOne() && SatisfyRuleTwo() && SatisfyRuleThree() && SatisfyRuleFour())
        {
            return true;
        }
        return false;
    }

    private bool SatisfyRuleOne()
    {
        if (HasOnlyOneSuperStar() && HasCorrectNumberOfCards())
        {
            return true;
        }
        return false;
    }
    private bool HasOnlyOneSuperStar()
    {
        if (SuperStarsList.Count != 1) return false;
        return true;
    }

    private const int CorrectNumberOfCards = 60;
    private bool HasCorrectNumberOfCards()
    {
        if (CardList.Count != CorrectNumberOfCards) return false;
        return true;
    }
    
    private bool SatisfyRuleTwo()
    {
        var groupedCardsByTitle = CardList.GroupBy(card => card.Title);
        foreach (IGrouping<string, Card> group in groupedCardsByTitle)
        {
            if (HasMoreThanOneUnique(group) || HasMoreThanThreeSetup(group))
            {
                return false;
            }
        }
        return true;
    }

    private bool HasMoreThanOneUnique(IGrouping<string, Card> groupOfCards)
    {
        int amountEqualCards = groupOfCards.Count();
        if (groupOfCards.First().ItsUnique() && amountEqualCards > 1) return true;
        return false;
    }

    private bool HasMoreThanThreeSetup(IGrouping<string, Card> groupOfCards)
    {
        int amountEqualCards = groupOfCards.Count();
        if (!groupOfCards.First().ItsSetUp() && amountEqualCards > 3) return true;
        return false;
    }

    private bool SatisfyRuleThree()
    {
        if(HasNoHeelAndFaceCards()) return true;
        return false;
    }
    private bool HasNoHeelAndFaceCards()
    {
        bool hasHeel = CardList.Any(card => card.HasSubtypeHeel());
        bool hasFace = CardList.Any(card => card.HasSubtypeFace());
        if (hasHeel && hasFace) return false; 
        return true;
    }

    private bool SatisfyRuleFour()
    {
        if (HasCorrectSuperStarLogo()) return true;
        return false;
    }
    
    private bool HasCorrectSuperStarLogo()
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