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
    private View _view;
    public CardCollection PlayedCards;

    public PlayManager(View view)
    {
        _view = view;
        _plays = new List<Play>();
        _nextPlayId = 1;
        PlayedCards = new CardCollection();
    }

    public Play CurrentPlay => (_plays.Count > 0 ? _plays[^1] : null)!;
    private Play PreviousPlay => (_plays.Count > 1 ? _plays[^2] : null)!;

    public void AddPlay(Play play)
    {
        play.Id = _nextPlayId;
        _nextPlayId++;
        _plays.Add(play);
        ApplyPendingEffectsIfPossible();
        play.CardAddedToPlayedCards += HandleCardAddedToPlayedCards;
        if (play.PlayedCards.CardListSize > 0)
            foreach (Card card in play.PlayedCards.CardList)
                PlayedCards.AddCard(card);
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

    public void HandleCardAddedToPlayedCards(object sender, Card card)
    {
        PlayedCards.AddCard(card);
    }


    public void RemoveEffectsOnCards()
    {
        foreach (Player player in CurrentPlay.Players) player.SetDefaultValuesInCards();
    }

    public Card GetLastCard()
    {
        return PlayedCards.GetLastCard();
    }
}