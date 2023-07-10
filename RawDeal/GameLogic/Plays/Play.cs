using RawDeal.CardCollections;
using RawDeal.Cards;
using RawDeal.Cards.CardEffects;
using RawDeal.GameLogic.Players;

namespace RawDeal.GameLogic.Plays;

public class Play
{
    public int Id;
    public Player CurrentPlayer;
    public Player NotCurrentPlayer;
    public readonly List<Effect> PendingEffects;
    public readonly CardCollection PlayedCards;
    public readonly List<Player> Players;
    public Card AttackingCard;
    public IndexedCard AttackingIndexedCard;
    public Card ReversalCard;
    public IndexedCard ReversalIndexedCard;
    public int PlayedCardsCount => PlayedCards.CardList.Count;
    private int PendingEffectsCount => PendingEffects.Count;
    public event EventHandler<Card> CardAddedToPlayedCards;

    public Play(PlayersPackage playersPackage)
    {
        CurrentPlayer = playersPackage.CurrentPlayer;
        NotCurrentPlayer = playersPackage.NotCurrentPlayer;
        Players = new List<Player>();
        Players?.AddRange(new[] { CurrentPlayer, NotCurrentPlayer });
        PlayedCards = new CardCollection();
        PendingEffects = new List<Effect>();
    }

    public void SetAttackingIndexedCard(IndexedCard attackingIndexedCard)
    {
        AttackingIndexedCard = attackingIndexedCard;
        AttackingCard = AttackingIndexedCard.Card;
        AddCardToPlayedCardsWithPendingEffectsApplied(AttackingCard);
        CurrentPlayer.PlayIdAndPlayedCards.Add(new IndexedCard(Id, AttackingCard));
    }

    public void SetReversalIndexedCard(IndexedCard reversalIndexedCard)
    {
        ReversalIndexedCard = reversalIndexedCard;
        ReversalCard = ReversalIndexedCard.Card;
        ReversalCard.PlayedType = "Reversal";
        AddCardToPlayedCardsWithPendingEffectsApplied(ReversalCard);
        NotCurrentPlayer.PlayIdAndPlayedCards.Add(new IndexedCard(Id, ReversalCard));
    }

    private void AddCardToPlayedCardsWithPendingEffectsApplied(Card card)
    {
        PlayedCards.Add(card);
        if (PendingEffectsCount > 0) ApplyPendingEffects();
        CardAddedToPlayedCards?.Invoke(this, card);
    }

    private void ApplyPendingEffects()
    {
        int pendingEffectsCount = PendingEffectsCount;
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
    }

    public void RemoveAPendingEffect(Effect pendingEffectToRemove)
    {
        PendingEffects.Remove(pendingEffectToRemove);
    }
}