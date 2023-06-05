using RawDeal.CardCollections;
using RawDeal.Cards;
using RawDeal.GameLogic;
using RawDeal.SuperStars;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal;

public class Player
{
    public SuperStar SuperStar { get; }
    private Arsenal Arsenal { get; }
    public Ringside Ringside  { get; }
    private Hand Hand { get; }
    private RingArea RingArea { get; }
    public List<int> DamagesReceived;

    private List<Card> _allCardsList;
    public int Fortitude;
    private View _view;
    public bool HasUsedHisAbilityInTheTurn;
    public bool HasEndsHisTurn;
    private int _damageReducedByShield;
    private CardMobilizer _cardMobilizer;

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
        _cardMobilizer = new CardMobilizer(_view);
        DamagesReceived = new List<int>();
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

    public void SetDefaultValuesInCards()
    {
        foreach (Card card in _allCardsList)
        {
            card.SetDefaultValues();
        }
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
        {
            if (IsCorrectReversalCard(playManager, card, playedFrom: "Arsenal"))
            {
                validReversals.Add(card);
                card.SetDefaultValues();
            }
        }
    
        return validReversals;
    }

    public bool IsCorrectReversalCard(PlayManager playManager, Card reversalCard, string playedFrom)
    {
        Play currentPlay = playManager.CurrentPlay;
        Card attackingCard = currentPlay.AttackingCard;
        if (reversalCard.GetCurrentFortitude(reversalCard.PlayedType) > Fortitude)
        {
            return false;
        }
        if (reversalCard.Subtypes.Contains("ReversalStrike"))
        {
            if (attackingCard.PlayedType.Contains("Maneuver") && attackingCard.Subtypes.Contains("Strike"))
            {
                return true;
            }
        }
        if (reversalCard.Subtypes.Contains("ReversalGrapple"))
        {
            if (attackingCard.PlayedType.Contains("Maneuver") && attackingCard.Subtypes.Contains("Grapple"))
            {
                return true;
            }
        } 
        if (reversalCard.Subtypes.Contains("ReversalSubmission"))
        {
            if (attackingCard.PlayedType.Contains("Maneuver") && attackingCard.Subtypes.Contains("Submission"))
            {
                return true;
            }
        }
        if (reversalCard.Subtypes.Contains("ReversalAction"))
        {
            if (attackingCard.PlayedType.Contains("Action"))
            {
                return true;
            }
        }
        
        if (reversalCard.Subtypes.Contains("ReversalGrappleSpecial"))
        {
            if (attackingCard.PlayedType.Contains("Maneuver") && attackingCard.Subtypes.Contains("Grapple"))
            {
                const int maximumDamageThatCanReverse = 7;
                if (attackingCard.GetCurrentDamage(attackingCard.PlayedType) <= maximumDamageThatCanReverse)
                {
                    return true;
                }
            }
        }
        
        if (reversalCard.Subtypes.Contains("ReversalStrikeSpecial"))
        {
            if (attackingCard.PlayedType.Contains("Maneuver") && attackingCard.Subtypes.Contains("Strike"))
            {
                const int maximumDamageThatCanReverse = 7;
                if (attackingCard.GetCurrentDamage(attackingCard.PlayedType) <= maximumDamageThatCanReverse)
                {
                    return true;
                }
            }
        }
        
        if (reversalCard.Subtypes.Contains("ReversalSpecial"))
        {
            return IsValidTheConditionOfReversalSpecial(playManager, reversalCard, playedFrom);
        }

        return false;

    }

    private bool IsValidTheConditionOfReversalSpecial(PlayManager playManager, Card reversalCard, string playedFrom)
    {
        Play currentPlay = playManager.CurrentPlay;
        Card attackingCard = currentPlay.AttackingCard;
        switch (reversalCard.Title)
        {
            case "Elbow to the Face":
                if (attackingCard.PlayedType.Contains("Maneuver") && attackingCard.GetCurrentDamage(attackingCard.PlayedType) <= 7)
                {
                    return true;
                }
                break;
            case "Manager Interferes":
                if (attackingCard.PlayedType.Contains("Maneuver"))
                {
                    return true;
                }
                break;
            case "Chyna Interferes":
                if (attackingCard.PlayedType.Contains("Maneuver"))
                {
                    return true;
                }
                break;
            case "Clean Break" when attackingCard.Title == "Jockeying for Position":
                return true;
            case "Jockeying for Position" when attackingCard.Title == "Jockeying for Position":
                return true;
            case "Irish Whip" when attackingCard.Title == "Irish Whip":
                return true;
            case "Shoulder Block":
                if (playManager.PlayedCards.CardListSize >= 2 && attackingCard.PlayedType == "Maneuver")
                {
                    if (playManager.PlayedCards.GetPenultimateCard.Title == "Irish Whip")
                    {
                        return true;
                    }
                }
                break;
            case "Spear":
                if (playManager.PlayedCards.CardListSize >= 2 && attackingCard.PlayedType == "Maneuver")
                {
                    if (playManager.PlayedCards.GetPenultimateCard.Title == "Irish Whip")
                    {
                        return true;
                    }
                }
                break;
            case "Facebuster":
                if (playManager.PlayedCards.CardListSize >= 2 && attackingCard.PlayedType == "Maneuver" && playedFrom == "Hand")
                {
                    if (playManager.PlayedCards.GetPenultimateCard.Title == "Irish Whip")
                    {
                        return true;
                    }
                }
                break;
            case "Lou Thesz Press":
                if (playManager.PlayedCards.CardListSize >= 2 && attackingCard.PlayedType == "Maneuver" && playedFrom == "Hand")
                {
                    if (playManager.PlayedCards.GetPenultimateCard.Title == "Irish Whip")
                    {
                        return true;
                    }
                }
                break;
            case "Cross Body Block":
                if (playManager.PlayedCards.CardListSize >= 2 && attackingCard.PlayedType == "Maneuver")
                {
                    if (playManager.PlayedCards.GetPenultimateCard.Title == "Irish Whip")
                    {
                        return true;
                    }
                }
                break;
            case "Belly to Belly Suplex" when attackingCard.Title == "Belly to Belly Suplex":
                return true;
            case "Vertical Suplex" when attackingCard.Title == "Vertical Suplex":
                return true;
            case "Belly to Back Suplex" when attackingCard.Title == "Belly to Back Suplex":
                return true;
            case "Ensugiri" when attackingCard.Title == "Kick":
                return true;
            case "Drop Kick" when attackingCard.Title == "Drop Kick":
                return true;
            case "Double Arm DDT" when attackingCard.Title == "Back Body Drop":
                return true;
        }

        return false;
    }
    
    public void ReceiveDamageWithView(int? damage)
    {
        _cardMobilizer.MoveCardsFromArsenalToRingSideByDamageAmount(this, damage);
        _view.SayThatSuperstarWillTakeSomeDamage(GetSuperStarName(), (int)damage);
    }
    
    public void ReceiveDamageWithoutView(int damage)
    {
        _cardMobilizer.MoveCardsFromArsenalToRingSideByDamageAmount(this, damage);
    }

    public int CalculateDamage(Card card)
    {
        int actualDamage = Math.Max((int)(card.GetCurrentDamage(card.PlayedType) - _damageReducedByShield), 0);
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
        if (SuperStar.HasInitialAbility)
        {
            SuperStar.UseInitialAbility(this);
        }
    }

    public string GetTypeOfPlayedCard(int indexOfCardSelected)
    {
        return Hand.GetTypeOfPlayedCard(indexOfCardSelected);
    }

    private void InitializeArsenal(List<Card> deck)
    {
        foreach (Card card in deck)
        {
            Arsenal.AddCard(card);
        }
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

    public List<Card> GetReversalCards()
    {
        List<Card> reversalCards = new List<Card>(); 
        foreach (Card card in _allCardsList)
        {
            if (card.IsTypeReversal)
            {
                reversalCards.Add(card);
            }
        }

        return reversalCards;
    }
}