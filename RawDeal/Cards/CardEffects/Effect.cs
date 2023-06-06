using RawDeal.Cards.CardEffects.ReversalsEffects;
using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects;

public abstract class Effect
{
    private Play _currentPlay;
    protected Player CurrentPlayer;
    protected Player NotCurrentPlayer;
    protected Card AttackingCard;
    protected Card ReversalCard;
    protected readonly View _view;
    protected CardMobilizer _cardMobilizer;
    protected (int, Card) AttackingCardTuple;
    protected (int, Card) ReversalCardTuple;

    protected Effect(View view)
    {
        _view = view;
        _cardMobilizer = new CardMobilizer(_view);
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