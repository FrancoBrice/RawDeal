using System.Net.Mail;
using RawDeal.CardCollections;
using RawDeal.Cards;
using RawDeal.Cards.CardEffects;
using RawDeal.Cards.CardEffects.ActionEffects;
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
    public Effect PendingEffect;
    private View _view;
    
    public Play(Dictionary<string, Player> playersDictionary, View view)
    {
        CurrentPlayer = playersDictionary["CurrentPlayer"];
        NotCurrentPlayer = playersDictionary["NotCurrentPlayer"];
        Players = new List<Player>();
        Players?.AddRange(new[] { CurrentPlayer, NotCurrentPlayer });
        PlayedCards = new CardCollection();
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

    public void AddCardToPlayedCardsWithPendingEffectsApplied(Card card)
    {
        PlayedCards.AddCard(card);
        Console.WriteLine($"playId: {Id} card added {card.Title} pendingeffect = {IsAPendingEffect}");
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

    public void NextCardDamageBonusByTypeAndSubtype(Card card, string playedType,  string subtype, int damageBonus)
    {
        NextCardDamageBonusByTypeAndSubtype effect = new NextCardDamageBonusByTypeAndSubtype(_view);
        effect.SetPlayedTypeThatAppliesBonus(playedType);
        effect.SetSubtypeThatAppliesBonus(subtype);
        effect.SetDamageBonus(damageBonus);
        SetPendingEffect(effect);
    }

    public void NextReversalRequiresMoreFortitudeBySubtype(Card card, string playedType,  string subtype, int extraFortitude)
    {
        NextReversalHasMoreFortitudeBySubtype effect = new NextReversalHasMoreFortitudeBySubtype(_view);
        effect.SetPlayedTypeThatAppliesExtraFortitude(playedType);
        effect.SetSubtypeThatAppliesExtraFortitude(subtype);
        effect.SetExtraFortitude(extraFortitude);
        SetPendingEffect(effect);
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
        (CurrentPlayer, NotCurrentPlayer) = (NotCurrentPlayer, CurrentPlayer);
    }

    public void SetPendingEffect(Effect pendingEffect)
    {
        PendingEffect = pendingEffect;
        IsAPendingEffect = true;
    }
}