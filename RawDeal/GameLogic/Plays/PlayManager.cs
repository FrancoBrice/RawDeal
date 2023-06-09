using RawDeal.CardCollections;
using RawDeal.Cards;
using RawDeal.Cards.CardEffects;
using RawDeal.GameLogic.Players;
using RawDealView;

namespace RawDeal.GameLogic.Plays;

public class PlayManager
{
    private int _nextPlayId;
    private readonly List<Play> _plays;
    private CardCollection _playedCards;
    private Play CurrentPlay => (_plays.Count > 0 ? _plays[^1] : null)!;
    private Play PreviousPlay => (_plays.Count > 1 ? _plays[^2] : null)!;
    
    public PlayManager()
    {
        _plays = new List<Play>();
        _nextPlayId = 1;
        _playedCards = new CardCollection();
    }

    public void AddPlay(Play play)
    {
        play.Id = _nextPlayId;
        _nextPlayId++;
        _plays.Add(play);
        ApplyPendingEffectsIfPossible();
        play.CardAddedToPlayedCards += HandleCardAddedToPlayedCards;
        if (play.PlayedCards.CardListSize <= 0) return;
        foreach (Card card in play.PlayedCards.CardList) _playedCards.AddCard(card);
    }

    public void ApplyPendingEffectsIfPossible()
    {
        if (_plays.Count < 2) return;
        if (PreviousPlay.PendingEffects.Count < 1) return;
        if (PreviousPlay.ReversalCard == null) return;
        Effect pendingEffect = PreviousPlay.PendingEffects[^1];
        CurrentPlay.AddPendingEffect(pendingEffect);
        pendingEffect.ApplyEffect(PreviousPlay);
    }

    private void HandleCardAddedToPlayedCards(object sender, Card card)
    {
        _playedCards.AddCard(card);
    }
    
    public void RemoveEffectsOnCards()
    {
        foreach (Player player in CurrentPlay.Players) player.SetDefaultValuesInCards();
    }

    public Card GetPenultimateCardPlayed()
    {
        return _playedCards.GetPenultimateCard;
    }

    public int NumberOfPlayedCards()
    {
        return _playedCards.CardListSize;
    }

    public Play GetCurrentPlay()
    {
        return CurrentPlay;
    }
}