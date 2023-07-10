using RawDeal.Cards.CardPreConditions;
using RawDeal.GameLogic;
using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards;

public class ReversalsByDeckController : CardDamageController
{
    private bool _cardWasReversedInLastCardOfDeck;

    public ReversalsByDeckController(Game game, PlayManager playManager, View view) : base(game, view)
    {
        _cardWasReversedInLastCardOfDeck = false;
        _playManager = playManager;
        SetPlayers(_currentPlay);
        SetAttackingCardAndPretendedDamage();
    }
    
    public void HandleReversalsByDeck()
    {
        _view.SayThatCardWasReversedByDeck(_notCurrentPlayer.GetSuperStarName());
        if (!GameEndChecker.CheckForGameOver(_game))
        {
            HandleStunValue(_currentPlayer, _attackingCard);
        }
    }
    
    private void HandleStunValue(Player attackingPlayer, Card attackingCard)
    {
        if (!ConditionsOfStunValueAreCorrect(attackingCard, attackingPlayer)) return;
        int numberOfCardsToDraw = _view.AskHowManyCardsToDrawBecauseOfStunValue(
            attackingPlayer.GetSuperStarName(), attackingCard.GetStunValue());
        if (numberOfCardsToDraw == 0) return;
        CardMobilizer.DrawStunValueCards(attackingPlayer, numberOfCardsToDraw);
        _view.SayThatPlayerDrawCards(attackingPlayer.GetSuperStarName(), numberOfCardsToDraw);
    }
    
    public void ApplyReversalByDeckIfPossible(Card possibleReversal, int index)
    {
        if (!IsPossibleApplyReversalByDeck(possibleReversal)) return;
        _cardWasReversedByDeck = true;
        _currentPlayer.HasEndsHisTurn = true;
        _currentPlay.SetReversalIndexedCard(new IndexedCard(index, possibleReversal));
        CardPlayer.HandleEffects(_view, _game, possibleReversal);
        if (_actualDamage == _pretendedDamage) _cardWasReversedInLastCardOfDeck = true;
    }

    
    private bool ConditionsOfStunValueAreCorrect(Card attackingCard, Player attackingPlayer)
    {
        return attackingCard.GetStunValue() > 0 && !_cardWasReversedInLastCardOfDeck && 
               !attackingPlayer.HasZeroCardsInArsenal();
    }
    
    public bool IsPossibleApplyReversalByDeck(Card possibleReversal)
    {
        possibleReversal.PlayedFrom = "Arsenal";
        return ReversalsChecker.IsCorrectReversalCard(_playManager, possibleReversal);
    }

    public void SetActualDamage(int actualDamage)
    {
        _actualDamage = actualDamage;
    }
}