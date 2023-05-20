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
    private CardCollection _playedCards; 
    public (int,Card) AttackingCardTuple ;
    public Card AttackingCard; 
    public (int,Card) ReversalCardTuple;
    public Card ReversalCard; 
    private TupleManager _tupleManager;
    public bool IsAPendingEffect;
    public Effect PendingEffect;
    private View _view;
    
    public Play(Dictionary<string, Player> playersDictionary, View view)
    {
        CurrentPlayer = playersDictionary["CurrentPlayer"];
        NotCurrentPlayer = playersDictionary["NotCurrentPlayer"];
        Players = new List<Player>();
        Players?.AddRange(new[] { CurrentPlayer, NotCurrentPlayer });
        _playedCards = new CardCollection();
        _tupleManager = new TupleManager();
        IsAPendingEffect = false;
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
        AddCardToPlayedCardsWithPendingEffectsApplied(ReversalCard);
    }

    public void AddDamagedCard(Card damagedCard)
    {
        damagedCard.PlayedType = "Damaged";
        AddCardToPlayedCardsWithPendingEffectsApplied(damagedCard);
    }

    private void AddCardToPlayedCardsWithPendingEffectsApplied(Card card)
    {
        _playedCards.AddCard(card);
        switch (IsAPendingEffect)
        {
            case true:
                ApplyPendingEffects();
                break;
        }
    }
    
    public void SetDefaultValuesOnCards()
    {
        foreach (Player player in Players)
        {
            player.SetDefaultValuesInCards();
        }
    }

    public void ApplyPendingEffects()
    {
        PendingEffect.ApplyEffect(currentPlay: this);
        IsAPendingEffect = false;
    }

    public Card GetLastCard()
    {
        return _playedCards.GetLastCard();
    }

    public void EndPlay()
    {
        foreach (Card card in _playedCards.CardList)
        {
            card.SetDefaultValues();
        }
        SwapCurrentAndNotCurrentPlayer();
    }

    public void SwapCurrentAndNotCurrentPlayer()
    {
        (CurrentPlayer, NotCurrentPlayer) = (NotCurrentPlayer, CurrentPlayer);
    }

    public void SetPendingEffect(Effect pendingEffect)
    {
        PendingEffect = pendingEffect;
        IsAPendingEffect = true;
    }
}