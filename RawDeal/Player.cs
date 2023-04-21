using System.Runtime.InteropServices;
using RawDealView;

namespace RawDeal;

public class Player
{
    private List<Card> CardList { get; set; }
    public SuperStar SuperStar { get; set; }
    private Arsenal Arsenal { get; set; }
    public Ringside Ringside  { get; set; }
    private Hand Hand { get; set; }
    private RingArea RingArea { get; set; }
    public int Fortitude;
    private View _view;
    public bool HasUsedHisAbilityInTheTurn;
    private int _damageReducedByShield;


    public Player(SuperStar superStar, List<Card> cardList, View view)
    {
        _view = view;
        CardList = cardList;
        foreach (var VARIABLE in CardList)
        {
            Console.WriteLine($"viendo en player id {VARIABLE.Id}");
        }
        SuperStar = superStar;
        Fortitude = 0;
        Ringside = new Ringside();
        RingArea = new RingArea();
        Arsenal = new Arsenal();
        AsignIdToCardsInCardList();
        InitializeArsenal();
        Hand = new Hand();
        DistributeOpeningHand();
        AddViewToSuperStar();
        HasUsedHisAbilityInTheTurn = false;
        _damageReducedByShield = 0;
    }

    private void AsignIdToCardsInCardList()
    {
        int nextId = 1;
        foreach (Card card in CardList)
        {
            Console.WriteLine($"{card.Title},next id {nextId}");
            card.Id = nextId;
            Console.WriteLine(card.Id);
            nextId++;
        }
    }
    
    private void InitializeArsenal()
    {
        foreach (Card card in CardList)
        {
            Arsenal.AddCard(card);
        }
    }

    public void DistributeOpeningHand()
    {
        foreach (var VARIABLE in Arsenal.GetLastCardsReversed(60))
        {
            Console.WriteLine($"revisando arsenal ids: {VARIABLE.Id}");
        }
        List<Card> drawnCards = Arsenal.GetLastCardsReversed(SuperStar.HandSize);
        for (int index = drawnCards.Count - 1; index >= 0; index--)
        {
            Console.WriteLine($"añadiendo a hand la carta {drawnCards[index].Title} con id {drawnCards[index].Id}");
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

    public List<string> GetCardsFromHandInStringFormat()
    {
        return Hand.GetFormattedCards();
    }
    public List<string> GetCardsFromRingsideInStringFormat()
    {
        return Ringside.GetFormattedCards();
    }
    public List<string> GetCardsFromRingAreaInStringFormat()
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

    public List<Card> GetPlayableCardsFromPlayer()
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
        HasUsedHisAbilityInTheTurn = true;
    }

    public void MoveCardFromHandToRingAreaById(int cardId)
    {
        Card card = Hand.GetCardById(cardId);
        Hand.RemoveCardById(cardId);
        AddCardToRingArea(card);
    }
    public void MoveCardFromRingsideToArsenalBeginningByIndex(int index)
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

    public bool PlayerHasLost()
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