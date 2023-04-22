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
    
    public Player(SuperStar superstar, List<Card> cardList, View view)
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

    public List<string> GetCardsInStringFormatFromHand()
    {
        return Hand.GetFormattedCards();
    }
    
    public List<string> GetCardsInStringFormatFromRingside()
    {
        return Ringside.GetFormattedCards();
    }
    
    public List<string> GetCardsInStringFormatFromRingArea()
    {
        return RingArea.GetFormattedCards();
    }
    public List<Card> GetCardsFromArsenal(int damage)
    {
        if (damage > Arsenal.CardListSize) damage = Arsenal.CardListSize;
        List<Card> topCardsOfArsenal = Arsenal.GetLastCardsReversed(damage);
        return topCardsOfArsenal; 
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
        return Hand.GetTuplesWithPositionInHandAndPlayableCards(Fortitude);
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
    
    public void UseSuperStarAbility(Player opponentPlayer)
    {
        SuperStar.UseAbility(this, opponentPlayer);
        HasUsedHisAbilityInTheTurn = true;
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
        if (Arsenal.CardListSize == 0) return true;
        return false;
    }
    
    public bool CanUseAbility()
    {
        return SuperStar.CanUseAbility(this);
    }
    
    public bool IsAbilityAutomatic()
    {
        return SuperStar.IsAbilityAutomatic();
    }

    public int GetRingsideSize()
    {
        return Ringside.CardListSize;
    }

    public string GetSuperStarAbility()
    {
        return SuperStar.SuperstarAbility;
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

    private void InitializeArsenal(List<Card> deck)
    {
        foreach (Card card in deck)
        {
            Arsenal.AddCard(card);
        }
    }
    private void MoveCardsFromArsenalToRingSideByDamageAmount(int damageAmount)
    {
        List<Card> cardsList = Arsenal.GetLastCardsReversed(damageAmount);
        if (damageAmount >= GetArsenalSize()) damageAmount = GetArsenalSize();
        for (int index = damageAmount - 1; index >= 0; index--)
        {
            AddCardToRingside(cardsList[index]);
            Arsenal.RemoveLastCard();
        }
    }

    private void AddViewToSuperStar()
    {
        SuperStar.AddView(_view);
    }
    
}