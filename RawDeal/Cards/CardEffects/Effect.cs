using RawDeal.Cards.CardEffects.ReversalsEffects;
using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects;

public abstract class Effect
{
    protected Play CurrentPlay;
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
        CurrentPlay = currentPlay;
        SetPlayers();
        SetCards();
        ApplyCustomEffect(currentPlay);
    }

    protected abstract void ApplyCustomEffect(Play currentPlay);

    protected bool CanReverseByDamage(int maximumDamageThatCanReverse, int? actualDamage)
    {
        return actualDamage <= maximumDamageThatCanReverse;
    }

    private void SetPlayers()
    {
        CurrentPlayer = CurrentPlay.CurrentPlayer;
        NotCurrentPlayer = CurrentPlay.NotCurrentPlayer;
    }

    private void SetCards()
    {
        AttackingCard = CurrentPlay.AttackingCard;
        AttackingCardTuple = CurrentPlay.AttackingCardTuple;
        ReversalCard = CurrentPlay.ReversalCard;
        ReversalCardTuple = CurrentPlay.ReversalCardTuple;
    }
}