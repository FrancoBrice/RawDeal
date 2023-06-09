using RawDeal.CardCollections;
using RawDeal.Cards;
using RawDeal.Cards.CardPreConditions;
using RawDeal.GameLogic.Plays;
using RawDeal.SuperStars;

namespace RawDeal.GameLogic.Players;

public class Player
{
    public List<(int, Card)> TuplesWithPlayIdAndPlayedCards;
    public int Fortitude;
    public bool HasEndsHisTurn;
    public bool HasUsedHisAbilityInTheTurn;
    public List<int> DamagesReceived;
    public int DamageReducedByShield;
    public List<Card> AllCardsList;
    public SuperStar SuperStar { get; init; }
    public Arsenal Arsenal { get; init; }
    public Ringside Ringside { get; init; }
    public Hand Hand { get; init; }
    public RingArea RingArea { get; init; }

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
        foreach (Card card in AllCardsList) card.SetDefaultValues();
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
        int actualDamage = Math.Max((int)(card.GetCurrentDamage() - DamageReducedByShield), 0);
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
        DamageReducedByShield = amountOfDamageShield;
    }

    public void ExecuteInitialAbility()
    {
        if (SuperStar.HasInitialAbility()) SuperStar.UseInitialAbility(this);
    }

    public List<Card> GetAllReversalCards()
    {
        return AllCardsList.Where(card => card.IsTypeReversal).ToList();
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