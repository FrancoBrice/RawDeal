using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects;

public abstract class Effect
{
    private protected readonly View _view;
    private Play _currentPlay;
    private protected Card AttackingCard;
    private protected (int, Card) AttackingCardTuple;
    private protected Player CurrentPlayer;
    private protected Player NotCurrentPlayer;
    private protected Card ReversalCard;
    private protected (int, Card) ReversalCardTuple;

    protected Effect(View view)
    {
        _view = view;
    }

    public void ApplyEffect(Play currentPlay)
    {
        _currentPlay = currentPlay;
        SetPlayers();
        SetCards();
        ApplyCustomEffect(currentPlay);
    }

    protected abstract void ApplyCustomEffect(Play currentPlay);

    private void SetPlayers()
    {
        CurrentPlayer = _currentPlay.CurrentPlayer;
        NotCurrentPlayer = _currentPlay.NotCurrentPlayer;
    }

    private void SetCards()
    {
        AttackingCard = _currentPlay.AttackingCard;
        AttackingCardTuple = _currentPlay.AttackingCardTuple;
        ReversalCard = _currentPlay.ReversalCard;
        ReversalCardTuple = _currentPlay.ReversalCardTuple;
    }
}