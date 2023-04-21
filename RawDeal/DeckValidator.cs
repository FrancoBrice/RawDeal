namespace RawDeal;

public class DeckValidator
{
    public List<Card> CardList { get; set; }
    public List<SuperStar> SuperStarsList { get; set; }
    private List<string> _AllSuperStarLogosList;


    public DeckValidator(List<SuperStar> deckSuperStarsList, List<Card> deckCardList)
    {
        CardList = deckCardList;
        SuperStarsList = deckSuperStarsList;
        _AllSuperStarLogosList = JsonReader.GenerateSuperStarLogosList();
    }
    
    public bool IsValidDeck()
    {
        if (CheckRule1() && CheckRule2() && CheckRule3() && CheckRule4())
        {
            return true;
        }
        return false;
    }
    private bool CheckRule1()
    {
        if (SuperStarsList.Count != 1 || CardList.Count != 60)
        {
            return false;
        }
        return true;
    }

    private bool CheckRule2()
    {
        var groupedCards = CardList.GroupBy(c => c.Title);
        foreach (var group in groupedCards)
        {
            int amountEqualCards = group.Count();
            if (amountEqualCards > 1)
            {
                if (IsUniqueCard(group.First()))
                {
                    return false;
                }

                if ((!IsSetupCard(group.First())) && (amountEqualCards > 3))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private bool CheckRule3()
    {
        bool hasHeel = CardList.Any(c => c.Subtypes.Contains("Heel"));
        bool hasFace = CardList.Any(c => c.Subtypes.Contains("Face"));
        if (hasHeel && hasFace)
        {
            return false;
        }

        return true;
    }

    private bool CheckRule4()
    {
        string superstarLogo = SuperStarsList.First().Logo;
        var invalidCards =
            CardList.Where(c => c.Subtypes.Any(s => s != superstarLogo && _AllSuperStarLogosList.Contains(s)));
        if (invalidCards.Any())
        {
            return false;
        }

        return true;
    }

    private bool IsUniqueCard(Card card)
    {
        return card.Subtypes.Contains("Unique");
    }
    private bool IsSetupCard(Card card)
    {
        return card.Subtypes.Contains("SetUp");
    }
}