using RawDeal.GameLogic;
using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects;

public abstract class Effect
{
    private protected readonly View _view;
    private Play _currentPlay;
    private protected Card _attackingCard;
    private protected IndexedCard _attackingIndexedCard;
    private protected Player _currentPlayer;
    private protected Player _notCurrentPlayer;
    private protected Card _reversalCard;
    private protected IndexedCard _reversalIndexedCard;
    public bool IsImportable => CheckIfIsImportable();

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

    protected virtual bool CheckIfIsImportable()
    {
        return false;
    }
    private void SetPlayers()
    {
        _currentPlayer = _currentPlay.CurrentPlayer;
        _notCurrentPlayer = _currentPlay.NotCurrentPlayer;
    }

    private void SetCards()
    {
        _attackingCard = _currentPlay.AttackingCard;
        _attackingIndexedCard = _currentPlay.AttackingIndexedCard;
        _reversalCard = _currentPlay.ReversalCard;
        _reversalIndexedCard = _currentPlay.ReversalIndexedCard;
    }
}