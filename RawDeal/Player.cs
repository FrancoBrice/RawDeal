using RawDealView;

namespace RawDeal;

public class Player
{
    public List<Card> CardList { get; set; }
    public SuperStar SuperStar { get; set; }
    public Arsenal Arsenal { get; set; }
    public Ringside Ringside  { get; set; }
    public Hand Hand { get; set; }
    public RingArea RingArea { get; set; }
    public int Fortitude;
    private View _view;
    public bool HasUsedHisAbilityInTheTurn;
    private int _damageReducedByShield;

    private void InitializeArsenal(List<Card> deck)
    {
        foreach (Card card in deck)
        {
            Arsenal.AddCard(card);
        }
    }
    
    public Player(List<Card> cardList, SuperStar superstar, View view)
    {
        _view = view;
        CardList = cardList;
        SuperStar = superstar;
        Fortitude = 0;
        Ringside = new Ringside();
        RingArea = new RingArea();
        Arsenal = new Arsenal();
        InitializeArsenal(cardList);
        Hand = new Hand();
        DistributeOpeningHand();
        AddViewToSuperStar();
        HasUsedHisAbilityInTheTurn = false;
        _damageReducedByShield = 0;
}

    public void DistributeOpeningHand()
    {
        List<Card> drawnCards = Arsenal.GetLastCardsReversed(SuperStar.HandSize);
        for (int index = drawnCards.Count - 1; index >= 0; index--)
        {
            AddCardToHand(drawnCards[index]);
            Arsenal.RemoveLastCard();
        }
    }
    
    public void MoveCardFromArsenalToHand()
    {
        if (Arsenal.CardListSize >= 1)
        {
            List<Card> drawnCards = Arsenal.GetLastCardsReversed(1);
            Card drawnCard = drawnCards[0];
            Hand.AddCard(drawnCard);
            Arsenal.RemoveLastCard();
        }
    }
    
    public void DrawLastCardFromHand()
    {
        if (Hand.CardListSize >= 1)
        {
            Hand.RemoveLastCard();
        }
    }

    public List<string> GetFormattedCardsFromHand()
    {
        return Hand.GetFormattedCards();
    }
    public List<string> GetCardTilesFromArsenal()
    {
        return Arsenal.GetFormattedCards();
    }
    public List<string> GetCardTilesFromRingside()
    {
        return Ringside.GetFormattedCards();
    }
    public List<string> GetCardTitlesFromRingArea()
    {
        return RingArea.GetFormattedCards();
    }
    public int GetHandSize()
    {
        return Hand.CardListSize;
    }

    public int GetArsenalSize()
    {
        return Arsenal.CardListSize;
    }
    public string GetSuperStarName()
    {
        return SuperStar.Name;
    }

    public List<(int, Card)> GetPlayableCardsFromPlayer()
    {
        return Hand.GetPlayableCards(Fortitude);
    }

    public void ReceiveDamage(int damage)
    {
        MoveCardsFromArsenalToRingSideByDamageAmount(damage);
        _view.SayThatOpponentWillTakeSomeDamage(GetSuperStarName(), damage);
    }

    public int CalculateDamage(int damage)
    {
        int actualDamage = Math.Max(damage - _damageReducedByShield, 1);
        return actualDamage;
    }

    public List<Card> GetCardsFromArsenal(int damage)
    {
        if (damage > Arsenal.CardListSize)
        {
            damage = Arsenal.CardListSize;
        }
        List<Card> topCards = Arsenal.GetLastCardsReversed(damage);
        return topCards; 
    }

    private void MoveCardsFromArsenalToRingSideByDamageAmount(int damageAmount)
    {
        List<Card> cardsList = Arsenal.GetLastCardsReversed(damageAmount);
        if (damageAmount >= GetArsenalSize())
        {
            damageAmount = GetArsenalSize();
        }
        for (int index = damageAmount - 1; index >= 0; index--)
        {
            AddCardToRingside(cardsList[index]);
            Arsenal.RemoveLastCard();
        }
        
    }

    public void UseSuperStarAbility(Player opponentPlayer)
    {
        SuperStar.UseAbility(this, opponentPlayer);
    }

    public void MoveCardFromHandToRingAreaByIndex(int index)
    {
        Card card = Hand.GetCardByIndex(index);
        Hand.RemoveCardByIndex(index);
        AddCardToRingArea(card);
    }
    public void MoveCardByIndexFromRingsideToArsenalBeginning(int index)
    {
        Card card = Ringside.GetCardByIndex(index);
        Ringside.RemoveCardByIndex(index);
        AddCardToArsenalAtTheBeginning(card);
    }

    public void AddCardToRingArea(Card card)
    {
        RingArea.AddCard(card);
    }
    public void AddCardToArsenalAtTheBeginning(Card card)
    {
        Arsenal.AddCardAtTheBeginning(card);
    }
    public void AddCardToHand(Card card)
    {
        Hand.AddCard(card);
    }

    public void AddCardToRingside(Card card)
    {
        Ringside.AddCard(card);
    }

    public void UpdateFortitude()
    {
        Fortitude = RingArea.GetFortitude();
    }

    public bool CheckIfPlayerLose()
    {
        if (Arsenal.CardListSize == 0)
        {
            return true;
        }
        return false;
    }

    private void AddViewToSuperStar()
    {
        SuperStar.AddView(_view);
    }

    public bool CheckIfCanUseAbility()
    {
        return SuperStar.CheckIfCanUseAbility(this);
    }
    public bool CheckIfAbilityIsAutomatic()
    {
        return SuperStar.CheckIfAbilityIsAutomatic();
    }

    public int GetRingsideSize()
    {
        return Ringside.CardListSize;
    }

    public string GetSuperStarAbility()
    {
        return SuperStar.SuperstarAbility;
    }

    public void MoveCardFromHandToRingsideByIndex(int indexCardFromHand)
    {
        Card card = Hand.GetCardByIndex(indexCardFromHand);
        Hand.RemoveCardByIndex(indexCardFromHand);
        AddCardToRingside(card);
    }

    public void MoveCardFromRingsideToHandByIndex(int indexCardFromRingside)
    {
        Card card = Ringside.GetCardByIndex(indexCardFromRingside);
        Ringside.RemoveCardByIndex(indexCardFromRingside);
        AddCardToHand(card);
    }

    public void MoveCardFromHandToArsenalBeginningByIndex(int indexCardFromHand)
    {
        Card card = Hand.GetCardByIndex(indexCardFromHand);
        Hand.RemoveCardByIndex(indexCardFromHand);
        AddCardToArsenalAtTheBeginning(card);
    }

    public void SetShieldOfDamage(int amountOfDamageShield)
    {
        _damageReducedByShield = amountOfDamageShield;
    }

    public void ExecuteInitialAbility()
    {
        if (SuperStar.HasInitialAbility)
        {
            SuperStar.UseInitialAbility(this);
        }
    }
}