using RawDeal.CardCollections;
using RawDeal.CardCollections.SubClasses;
using RawDeal.Cards;
using RawDeal.Cards.CardPreConditions;
using RawDeal.GameLogic.Plays;
using RawDeal.SuperStars;

namespace RawDeal.GameLogic.Players;

public class Player
{
    public List<IndexedCard> PlayIdAndPlayedCards;
    public int Fortitude;
    public bool HasEndsHisTurn;
    public bool HasUsedHisAbilityInTheTurn;
    public readonly List<int> DamagesReceived;
    private int _damageReducedByShield;
    private readonly List<Card> _allCardsList;
    private SuperStar SuperStar { get; }
    public Arsenal Arsenal { get; }
    public Ringside Ringside { get; }
    public Hand Hand { get; }
    public RingArea RingArea { get; }

    public Player(SuperStar superstar, List<Card> cardList)
    {
        SuperStar = superstar;
        Fortitude = 0;
        Ringside = new Ringside();
        RingArea = new RingArea();
        Arsenal = new Arsenal();
        Hand = new Hand();
        _allCardsList = cardList;
        DamagesReceived = new List<int>();
        PlayIdAndPlayedCards = new List<IndexedCard>();
        HasUsedHisAbilityInTheTurn = false;
        HasEndsHisTurn = false;
        _damageReducedByShield = 0;
    }

    public List<string> GetCardsInStringFormatFromHand()
    {
        return Hand.GetFormattedCards();
    }
    
    public List<string> GetCardsInStringFormatFromArsenal()
    {
        return Arsenal.GetFormattedCards();
    }

    public List<string> GetCardsInStringFormatFromRingside()
    {
        return Ringside.GetFormattedCards();
    }

    public List<string> GetCardsInStringFormatFromRingArea()
    {
        return RingArea.GetFormattedCards();
    }

    public CardCollection GetCardsFromArsenal(int damage)
    {
        if (damage > Arsenal.Count) damage = Arsenal.Count;
        CardCollection topCardsOfArsenal = Arsenal.GetLastCardsReversed(damage);
        return topCardsOfArsenal;
    }
    
    public List<Card> GetAllArsenalCards()
    {
        return Arsenal.CardList;
    }
    
    public List<Card> GetAllRingAreaCards()
    {
        return RingArea.CardList;
    }
    
    public List<Card> GetAllRingsideCards()
    {
        return Ringside.CardList;
    }
    
    public List<Card> GetAllHandCards()
    {
        return Hand.CardList;
    }

    public int GetHandSize()
    {
        return Hand.Count;
    }

    public int GetArsenalSize()
    {
        return Arsenal.Count;
    }

    public string GetSuperStarName()
    {
        return SuperStar.Name;
    }

    public int GetSuperStarValue()
    {
        return SuperStar.SuperstarValue;
    }

    public void SetDefaultValuesInCards()
    {
        foreach (Card card in _allCardsList) card.SetDefaultValues();
    }

    public List<IndexedCard> GetPlayableCardsFromPlayer(PlayManager playManager)
    {
        return Hand.GetIndexedCardsWithPositionInHandAndPlayableCards(playManager);
    }

    public List<IndexedCard> GetReversalIndexedCardsFromHand(PlayManager playManager)
    {
        return Hand.GetIndexedCardsWithPositionInHandAndReversalCards(playManager);
    }

    public List<Card> GetReversalsFromArsenal(PlayManager playManager)
    {
        List<Card> validReversals = new List<Card>();
        foreach (Card card in Arsenal.CardList)
            if (ReversalsChecker.IsCorrectReversalCard(playManager, card))
            {
                validReversals.Add(card);
                card.SetDefaultValues();
            }
        return validReversals;
    }

    public int CalculateDamage(Card card)
    {
        int actualDamage = Math.Max((int)(card.GetCurrentDamage() - _damageReducedByShield), 0);
        return actualDamage;
    }

    public void UseSuperStarAbility(Player opponentPlayer)
    {
        SuperStar.UseAbility(this, opponentPlayer);
        HasUsedHisAbilityInTheTurn = true;
    }
    
    public void UpdateFortitude()
    {
        Fortitude = RingArea.GetFortitude();
    }

    public bool HasZeroCardsInArsenal()
    {
        return Arsenal.Count == 0;
    }

    public bool CanUseHisAbility()
    {
        return SuperStar.CanUseAbility(this);
    }

    public bool IsAbilityAutomatic()
    {
        return SuperStar.IsAbilityAutomatic();
    }

    public int GetRingsideSize()
    {
        return Ringside.Count;
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
        if (SuperStar.HasInitialAbility()) SuperStar.UseInitialAbility(this);
    }

    public List<Card> GetAllReversalCards()
    {
        return _allCardsList.Where(card => card.IsTypeReversal).ToList();
    }

    public int AmountOfDamagesReceived()
    {
        return DamagesReceived.Count;
    }

    public int LastDamageReceived()
    {
        return DamagesReceived[^1];
    }
}