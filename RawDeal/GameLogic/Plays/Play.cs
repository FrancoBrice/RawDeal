using System.Net.Mail;
using RawDeal.CardCollections;
using RawDeal.Cards;
using RawDeal.Cards.CardEffects;
using RawDeal.Cards.CardEffects.ActionEffects;
using RawDeal.Cards.CardEffects.GeneralEffects;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.GameLogic;

public class Play
{
    public int Id;
    public Player CurrentPlayer;
    public Player NotCurrentPlayer;
    public List<Player> Players;
    public CardCollection PlayedCards; 
    public (int,Card) AttackingCardTuple ;
    public Card AttackingCard; 
    public (int,Card) ReversalCardTuple;
    public Card ReversalCard; 
    private TupleManager _tupleManager;
    public bool IsAPendingEffect;
    public List<Effect> PendingEffects;
    private View _view;
    public int PlayedCardsCount => PlayedCards.CardList.Count;
    public event EventHandler<Card> CardAddedToPlayedCards;

    public Play(Dictionary<string, Player> playersDictionary, View view)
    {
        CurrentPlayer = playersDictionary["CurrentPlayer"];
        NotCurrentPlayer = playersDictionary["NotCurrentPlayer"];
        Players = new List<Player>();
        Players?.AddRange(new[] { CurrentPlayer, NotCurrentPlayer });
        PlayedCards = new CardCollection();
        _tupleManager = new TupleManager();
        IsAPendingEffect = false;
        PendingEffects = new List<Effect>();
        _view = view;
    }

    public void SetAttackingCardTuple((int, Card) attackingCardTuple)
    {
        AttackingCardTuple = attackingCardTuple;
        AttackingCard = _tupleManager.ExtractCard(AttackingCardTuple);
        AddCardToPlayedCardsWithPendingEffectsApplied(AttackingCard);
    }
    public void SetReversalCardTuple((int, Card) reversalCardTuple)
    {
        ReversalCardTuple = reversalCardTuple;
        ReversalCard = _tupleManager.ExtractCard(ReversalCardTuple);
        ReversalCard.PlayedType = "Reversal";
        AddCardToPlayedCardsWithPendingEffectsApplied(ReversalCard);
    }

    private void AddCardToPlayedCardsWithPendingEffectsApplied(Card card)
    {
        PlayedCards.AddCard(card);
        if (IsAPendingEffect) ApplyPendingEffects();
        CardAddedToPlayedCards?.Invoke(this, card);

    }

    public void ApplyPendingEffects()
    {
        int pendingEffectsCount = PendingEffects.Count;
        Console.WriteLine($"En play cantidad de efectos pendientes: {pendingEffectsCount}");
        if (pendingEffectsCount > 0) Console.WriteLine(PendingEffects[^1]);
        Console.WriteLine(AttackingCard.Title);
        Console.WriteLine(AttackingCard.GetCurrentDamage(AttackingCard.PlayedType));
        for (int i = 0; i < pendingEffectsCount; i++)
        {
            Effect effect = PendingEffects[i];
            effect.ApplyEffect(currentPlay: this);
            i--;
            pendingEffectsCount--;
        }
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
        SwapCurrentAndNotCurrentPlayer();
    }

    public void SwapCurrentAndNotCurrentPlayer()
    {
        (CurrentPlayer, NotCurrentPlayer) = (NotCurrentPlayer, CurrentPlayer);
    }

    public void AddPendingEffect(Effect pendingEffect)
    {
        PendingEffects.Add(pendingEffect);
        IsAPendingEffect = true;
    }
    
    
}