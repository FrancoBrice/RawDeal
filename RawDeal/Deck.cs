namespace RawDeal;

public class Deck
{
    public List<Card> CardList { get; set; }
    public List<SuperStar> SuperStarList { get; set; }
    public List<string> SuperStarLogosList { get; set; }


    public Deck(List<SuperStar> deckSuperStarList, List<Card> deckCardList, List<string> superStarLogosList)
    {
        CardList = deckCardList;
        SuperStarList = deckSuperStarList;
        SuperStarLogosList = superStarLogosList;
    }
    private bool CheckRule1()
    {
        if (SuperStarList.Count != 1 || CardList.Count != 60)
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
        string superstarLogo = SuperStarList.First().Logo;
        var invalidCards =
            CardList.Where(c => c.Subtypes.Any(s => s != superstarLogo && SuperStarLogosList.Contains(s)));
        if (invalidCards.Any())
        {
            return false;
        }

        return true;
    }
    
    public bool IsValidDeck()
    {
        if (CheckRule1() && CheckRule2() && CheckRule3() && CheckRule4())
        {
            return true;
        }
        return false;
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