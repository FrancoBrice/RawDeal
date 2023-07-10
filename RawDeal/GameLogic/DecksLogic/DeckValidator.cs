using RawDeal.Cards;
using RawDeal.JsonReaders;
using RawDeal.SuperStars;

namespace RawDeal.GameLogic.DecksLogic;

public class DeckValidator
{
    private const int CorrectNumberOfCards = 60;
    private readonly List<string> _allSuperStarLogosList;
    public List<Card> CardList { get; }
    public SuperStar[] SuperStarsArray { get; }

    public DeckValidator(SuperStar[] superStarsArray, List<Card> cardList)
    {
        CardList = cardList;
        SuperStarsArray = superStarsArray;
        _allSuperStarLogosList = SuperstarsJsonReader.GenerateSuperStarLogosList();
    }

    public bool IsValidDeck()
    {
        return SatisfyRuleOne() && SatisfyRuleTwo() && SatisfyRuleThree() && SatisfyRuleFour();
    }

    private bool SatisfyRuleOne()
    {
        return HasOnlyOneSuperStar() && HasCorrectNumberOfCards();
    }

    private bool HasOnlyOneSuperStar()
    {
        return SuperStarsArray.Length == 1;
    }

    private bool HasCorrectNumberOfCards()
    {
        return CardList.Count == CorrectNumberOfCards;
    }

    private bool SatisfyRuleTwo()
    {
        IEnumerable<IGrouping<string, Card>> groupedCardsByTitle =
            CardList.GroupBy(card => card.Title);
        foreach (IGrouping<string, Card> group in groupedCardsByTitle)
            if (HasMoreThanOneUnique(group) || HasMoreThanThreeSetup(group))
                return false;
        return true;
    }

    private bool HasMoreThanOneUnique(IGrouping<string, Card> groupOfCards)
    {
        int amountEqualCards = groupOfCards.Count();
        return groupOfCards.First().ItsUnique() && amountEqualCards > 1;
    }

    private bool HasMoreThanThreeSetup(IGrouping<string, Card> groupOfCards)
    {
        const int minimumAmountEqualCards = 3;
        int amountEqualCards = groupOfCards.Count();
        return !groupOfCards.First().ItsSetUp() && amountEqualCards > minimumAmountEqualCards;
    }

    private bool SatisfyRuleThree()
    {
        return HasNoHeelAndFaceCards();
    }

    private bool HasNoHeelAndFaceCards()
    {
        bool hasHeel = CardList.Any(card => card.HasSubtypeHeel());
        bool hasFace = CardList.Any(card => card.HasSubtypeFace());
        return !hasHeel || !hasFace;
    }

    private bool SatisfyRuleFour()
    {
        return HasCorrectSuperStarLogo();
    }

    private bool HasCorrectSuperStarLogo()
    {
        IEnumerable<Card> invalidCards = FoundCardsWithIncorrectLogo();
        return !invalidCards.Any();
    }

    private IEnumerable<Card> FoundCardsWithIncorrectLogo()
    {
        string superstarLogoToCheck = SuperStarsArray.First().Logo;
        return CardList.Where(card =>
            card.Subtypes.Any(superstarLogo => superstarLogo != superstarLogoToCheck &&
                                               _allSuperStarLogosList.Contains(superstarLogo)));
    }
}