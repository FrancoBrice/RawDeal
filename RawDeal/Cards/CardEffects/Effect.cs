using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects;

public abstract class Effect
{
    private protected readonly View _view;
    private Play _currentPlay;
    private protected Card _attackingCard;
    private protected (int, Card) _attackingCardTuple;
    private protected Player _currentPlayer;
    private protected Player _notCurrentPlayer;
    private protected Card _reversalCard;
    private protected (int, Card) _reversalCardTuple;

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
        _currentPlayer = _currentPlay.CurrentPlayer;
        _notCurrentPlayer = _currentPlay.NotCurrentPlayer;
    }

    private void SetCards()
    {
        _attackingCard = _currentPlay.AttackingCard;
        _attackingCardTuple = _currentPlay.AttackingCardTuple;
        _reversalCard = _currentPlay.ReversalCard;
        _reversalCardTuple = _currentPlay.ReversalCardTuple;
    }
}