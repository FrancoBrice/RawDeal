using RawDeal.CardCollections;
using RawDeal.Cards;
using RawDeal.Cards.CardPreConditions;
using RawDeal.GameLogic.Plays;
using RawDeal.SuperStars;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.GameLogic.Players;

public class Player
{
    public readonly List<int> DamagesReceived;
    public readonly List<(int, Card)> TuplesWithPlayIdAndPlayedCards;
    public int Fortitude;
    public bool HasEndsHisTurn;
    public bool HasUsedHisAbilityInTheTurn;
    private int _damageReducedByShield;
    private readonly List<Card> _allCardsList;
    private readonly View _view;
    private SuperStar SuperStar { get; }
    private Arsenal Arsenal { get; }
    private Ringside Ringside { get; }
    private Hand Hand { get; }
    private RingArea RingArea { get; }
    
    public Player(SuperStar superstar, List<Card> cardList, View view)
    {
        _view = view;
        SuperStar = superstar;
        _allCardsList = cardList;
        Fortitude = 0;
        Ringside = new Ringside();
        RingArea = new RingArea();
        Arsenal = new Arsenal();
        InitializeArsenal(cardList);
        Hand = new Hand();
        DistributeOpeningHand();
        AddViewToSuperStar();
        HasUsedHisAbilityInTheTurn = false;
        HasEndsHisTurn = false;
        _damageReducedByShield = 0;
        DamagesReceived = new List<int>();
        TuplesWithPlayIdAndPlayedCards = new List<(int, Card)>();
    }

    private void DistributeOpeningHand()
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
        if (Arsenal.CardListSize < 1) return;
        List<Card> drawnCards = Arsenal.GetLastCardsReversed(1);
        Card drawnCard = drawnCards[0];
        Hand.AddCard(drawnCard);
        Arsenal.RemoveLastCard();
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

    public int GetSuperStarValue()
    {
        return SuperStar.SuperstarValue;
    }

    public void SetDefaultValuesInCards()
    {
        foreach (Card card in _allCardsList) card.SetDefaultValues();
    }

    public List<(int, Card)> GetPlayableCardsFromPlayer(PlayManager playManager)
    {
        return Hand.GetTuplesWithPositionInHandAndPlayableCards(playManager, Fortitude);
    }

    public List<(int, Card)> GetReversalTuplesFromHand(PlayManager playManager)
    {
        return Hand.GetTuplesWithPositionInHandAndReversalCards(this, playManager);
    }

    public List<Card> GetReversalsFromArsenal(PlayManager playManager)
    {
        List<Card> validReversals = new();
        foreach (Card card in Arsenal.CardList)
            if (ReversalsChecker.IsCorrectReversalCard(playManager, card))
            {
                validReversals.Add(card);
                card.SetDefaultValues();
            }
        return validReversals;
    }

    public void ReceiveDamage(int damage)
    {
        CardMobilizer.MoveCardsFromArsenalToRingSideByDamageAmount(this, damage);
        DamagesReceived.Add(damage);
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

    private void AddCardToRingArea(Card card)
    {
        RingArea.AddCard(card);
    }

    private void AddCardToArsenalAtTheBeginning(Card card)
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

    public bool HasZeroCardsInArsenal()
    {
        return Arsenal.CardListSize == 0;
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
        if (SuperStar.HasInitialAbility) SuperStar.UseInitialAbility(this);
    }

    private void InitializeArsenal(List<Card> deck)
    {
        foreach (Card card in deck) Arsenal.AddCard(card);
    }

    public void RemoveLastCardFromArsenal()
    {
        Arsenal.RemoveLastCard();
    }

    public List<Card> GetLastCardsFromArsenalReversed(int? numberOfCards)
    {
        return Arsenal.GetLastCardsReversed(numberOfCards);
    }

    private void AddViewToSuperStar()
    {
        SuperStar.AddView(_view);
    }

    public List<Card> GetAllReversalCards()
    {
        return _allCardsList.Where(card => card.IsTypeReversal).ToList();
    }
}