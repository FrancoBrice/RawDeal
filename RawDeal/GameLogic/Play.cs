using System.Net.Mail;
using RawDeal.CardCollections;
using RawDeal.Cards;
using RawDeal.Cards.CardEffects;
using RawDeal.Cards.CardEffects.ActionEffects;

namespace RawDeal.GameLogic;

public class Play
{
    public Player CurrentPlayer;
    public Player NotCurrentPlayer;
    public CardCollection PlayedCards; 
    public (int,Card) AttackingCardTuple ;
    public Card AttackingCard; 
    public (int,Card) ReversalCardTuple;
    public Card ReversalCard; 
    private TupleManager _tupleManager;
    public bool IsAPendingEffect;
    public NextCardEffect NextCardEffect { get; set; }
    
    public Play(Dictionary<string, Player> playersDictionary)
    {
        CurrentPlayer = playersDictionary["CurrentPlayer"];
        NotCurrentPlayer = playersDictionary["NotCurrentPlayer"];
        PlayedCards = new CardCollection();
        _tupleManager = new TupleManager();
        IsAPendingEffect = false;
    }

    public void SetAttackingCardTuple((int, Card) attackingCardTuple)
    {
        AttackingCardTuple = attackingCardTuple;
        AttackingCard = _tupleManager.ExtractCard(AttackingCardTuple);
        AddCardToPlayedCards(AttackingCard);
    }
    public void SetReversalCardTuple((int, Card) reversalCardTuple)
    {
        ReversalCardTuple = reversalCardTuple;
        ReversalCard = _tupleManager.ExtractCard(ReversalCardTuple);
        AddCardToPlayedCards(ReversalCard);
    }

    public void AddCardToPlayedCards(Card card)
    {
        if (IsAPendingEffect) ApplyPendingEffects();
        PlayedCards.AddCard(card);
    }

    public void ApplyPendingEffects()
    {
        Card lastCard = GetLastCard();
    }

    public void NextCardDamageBonusByTypeAndSubtype(Card card, string playedType,  string subtype, int damageBonus)
    {
        if (card.PlayedType == playedType && card.Subtypes.Contains(subtype))
        {
            card.SetCurrentDamage(card.GetCurrentDamage() + damageBonus);
        }
    }

    public void NextReversalHasMoreFortitudeBySubtype(string subtype, int extraFortitude)
    {
        
    }

    public Card GetLastCard()
    {
        return PlayedCards.GetLastCard();
    }

    public void EndPlay()
    {
        foreach (Card card in PlayedCards.CardList)
        {
            card.SetDefaultValues();
        }
    }
}