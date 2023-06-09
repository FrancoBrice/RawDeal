using RawDeal.CardCollections;
using RawDeal.Cards;
using RawDeal.Cards.CardEffects;
using RawDeal.GameLogic.Players;
using RawDeal.Tools;

namespace RawDeal.GameLogic.Plays;

public class Play
{
    public Card AttackingCard;
    public (int, Card) AttackingCardTuple;
    public Player CurrentPlayer;
    public int Id;
    public bool IsAPendingEffect;
    public Player NotCurrentPlayer;
    public readonly List<Effect> PendingEffects;
    public readonly CardCollection PlayedCards;
    public readonly List<Player> Players;
    public Card ReversalCard;
    public (int, Card) ReversalCardTuple;
    public int PlayedCardsCount => PlayedCards.CardList.Count;
    public event EventHandler<Card> CardAddedToPlayedCards;

    public Play(Dictionary<string, Player> playersDictionary)
    {
        CurrentPlayer = playersDictionary["CurrentPlayer"];
        NotCurrentPlayer = playersDictionary["NotCurrentPlayer"];
        Players = new List<Player>();
        Players?.AddRange(new[] { CurrentPlayer, NotCurrentPlayer });
        PlayedCards = new CardCollection();
        IsAPendingEffect = false;
        PendingEffects = new List<Effect>();
    }

    public void SetAttackingCardTuple((int, Card) attackingCardTuple)
    {
        AttackingCardTuple = attackingCardTuple;
        AttackingCard = TupleManager.ExtractCard(AttackingCardTuple);
        AddCardToPlayedCardsWithPendingEffectsApplied(AttackingCard);
        CurrentPlayer.TuplesWithPlayIdAndPlayedCards.Add((Id, AttackingCard));
    }

    public void SetReversalCardTuple((int, Card) reversalCardTuple)
    {
        ReversalCardTuple = reversalCardTuple;
        ReversalCard = TupleManager.ExtractCard(ReversalCardTuple);
        ReversalCard.PlayedType = "Reversal";
        AddCardToPlayedCardsWithPendingEffectsApplied(ReversalCard);
        NotCurrentPlayer.TuplesWithPlayIdAndPlayedCards.Add((Id, ReversalCard));
    }

    private void AddCardToPlayedCardsWithPendingEffectsApplied(Card card)
    {
        PlayedCards.AddCard(card);
        if (IsAPendingEffect) ApplyPendingEffects();
        CardAddedToPlayedCards?.Invoke(this, card);
    }

    private void ApplyPendingEffects()
    {
        int pendingEffectsCount = PendingEffects.Count;
        for (int i = 0; i < pendingEffectsCount; i++)
        {
            Effect effect = PendingEffects[i];
            effect.ApplyEffect(this);
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
        foreach (Card card in PlayedCards.CardList) card.SetDefaultValues();
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

    public void RemoveAPendingEffect(Effect pendingEffectToRemove)
    {
        PendingEffects.Remove(pendingEffectToRemove);
    }
}