using RawDeal.CardCollections;
using RawDeal.Cards;
using RawDeal.Cards.CardEffects;
using RawDeal.GameLogic.Players;

namespace RawDeal.GameLogic.Plays;

public class PlayManager
{
    private int _nextPlayId;
    private readonly List<Play> _plays;
    private readonly CardCollection _playedCards;
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
        AddIdToNewPlay(play);
        _plays.Add(play);
        ImportPendingEffectsIfPossible();
        play.CardAddedToPlayedCards += HandleCardAddedToPlayedCards;
        if (play.PlayedCardsCount <= 0) return;
        foreach (Card card in play.PlayedCards.CardList) _playedCards.Add(card);
    }

    private void AddIdToNewPlay(Play play)
    {
        play.Id = _nextPlayId;
        _nextPlayId++;
    }

    public void ImportPendingEffectsIfPossible()
    {
        if (_plays.Count < 2) return;
        List<Effect> pendingEffectsCopy = new List<Effect>(PreviousPlay.PendingEffects);
        if (pendingEffectsCopy.Count < 1) return;
        for (int index = pendingEffectsCopy.Count - 1; index >= 0; index--)
        {
            Effect pendingEffect = pendingEffectsCopy[index];
            if (pendingEffect.IsImportable)
            {
                CurrentPlay.AddPendingEffect(pendingEffect);
                pendingEffect.ApplyEffect(PreviousPlay);
            }
        }
    }

    private void HandleCardAddedToPlayedCards(object sender, Card card)
    {
        _playedCards.Add(card);
    }
    
    public void RemoveEffectsOnCards()
    {
        foreach (Player player in CurrentPlay.Players) player.SetDefaultValuesInCards();
    }

    public Card GetPenultimateCardPlayed()
    {
        return _playedCards.GetPenultimateCard;
    }

    public int GetNumberOfPlayedCards()
    {
        return _playedCards.Count;
    }

    public Play GetCurrentPlay()
    {
        return CurrentPlay;
    }

    public Play GetPreviousPlay()
    {
        return PreviousPlay;
    }
}