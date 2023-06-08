using RawDeal.Cards.CardEffects;
using RawDeal.Cards.CardPreConditions;
using RawDeal.GameLogic;
using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards;

public class CardDamager
{
    private protected readonly Play _currentPlay;
    private readonly Game _game;
    private protected readonly View _view;
    private int _actualDamage;
    private List<Card> _actualDamagedCards;
    private Card _attackingCard;
    private bool _cardWasReversedByDeck;
    private bool _cardWasReversedInLastCardOfDeck;
    private protected Player _currentPlayer;
    private protected Player _notCurrentPlayer;
    private protected bool _opponentRanOutOfCards;
    private int _pretendedDamage;

    public CardDamager(Game game, View view)
    {
        _game = game;
        _view = view;
        _currentPlay = _game.CurrentPlay;
    }

    public void ApplyCardDamage()
    {
        SetPlayers(_currentPlay);
        SetAttackingCardAndPretendedDamage();
        if (_pretendedDamage == 0) return;
        HandleActualDamage(_notCurrentPlayer);
        HandleReversalsByDeck(_currentPlayer, _notCurrentPlayer);
        _opponentRanOutOfCards =
            GameEndChecker.PlayerRanOutOfCardsDuringDamage(_notCurrentPlayer, _pretendedDamage);
        _notCurrentPlayer.ReceiveDamage(_actualDamage);
        FinishCardDamage(_currentPlayer);
    }
    
    private void HandleActualDamage(Player damagedPlayer)
    {
        _actualDamage = 0;
        List<Card> cardsToBeDamaged = damagedPlayer.GetCardsFromArsenal(_pretendedDamage);
        _view.SayThatSuperstarWillTakeSomeDamage(damagedPlayer.GetSuperStarName(),
            _pretendedDamage);
        RunDamageLoop(cardsToBeDamaged);
        ViewManager.ShowDamagedCards(_view, _actualDamagedCards, _pretendedDamage);
    }

    private void HandleReversalsByDeck(Player attackingPlayer, Player damagedPlayer)
    {
        if (!_cardWasReversedByDeck) return;
        _view.SayThatCardWasReversedByDeck(damagedPlayer.GetSuperStarName());
        HandleStunValue(attackingPlayer, _attackingCard);
    }

    private void RunDamageLoop(List<Card> cardsToBeDamaged)
    {
        _actualDamagedCards = new List<Card>();
        for (int index = cardsToBeDamaged.Count - 1; index >= 0; index--)
        {
            _actualDamage++;
            Card damagedCard = cardsToBeDamaged[index];
            _actualDamagedCards.Add(damagedCard);
            CheckAndApplyReversalByDeck(damagedCard, index);
            if (_cardWasReversedByDeck) break;
        }
    }

    private void CheckAndApplyReversalByDeck(Card possibleReversal, int index)
    {
        List<Card> possibleReversals = _notCurrentPlayer.GetReversalsFromArsenal(_game.PlayManager);
        possibleReversal.PlayedFrom = "Arsenal";
        if (!possibleReversals.Contains(possibleReversal) ||
            !ReversalsChecker.IsCorrectReversalCard(_game.PlayManager, possibleReversal)) return;
        _cardWasReversedByDeck = true;
        _currentPlayer.HasEndsHisTurn = true;
        _currentPlay.SetReversalCardTuple((index, possibleReversal));
        CardPlayer.HandleEffects(_game, possibleReversal);
        if (_actualDamage == _pretendedDamage) _cardWasReversedInLastCardOfDeck = true;
    }

    private void HandleStunValue(Player attackingPlayer, Card attackingCard)
    {
        if (attackingCard.GetStunValue() <= 0 || _cardWasReversedInLastCardOfDeck) return;
        int numberOfCardsToDraw = _view.AskHowManyCardsToDrawBecauseOfStunValue(
            attackingPlayer.GetSuperStarName(), attackingCard.GetStunValue());
        if (numberOfCardsToDraw == 0) return;
        CardMobilizer.DrawStunValueCards(attackingPlayer, numberOfCardsToDraw);
        _view.SayThatPlayerDrawCards(attackingPlayer.GetSuperStarName(), numberOfCardsToDraw);
    }

    private void FinishCardDamage(Player attackingPlayer)
    {
        EndGameIfPlayerRanOutOfCardsAndNotReverse(attackingPlayer);
        _game.MakePlayManagerRemoveEffectsOnCards();
    }
    
    protected void EndGameIfPlayerRanOutOfCardsAndNotReverse(Player attackingPlayer)
    {
        if (_opponentRanOutOfCards && !_cardWasReversedByDeck) _game.EndGame(attackingPlayer);
    }

    protected void SetPlayers(Play currentPlay)
    {
        _currentPlayer = currentPlay.CurrentPlayer;
        _notCurrentPlayer = currentPlay.NotCurrentPlayer;
    }
    
    private void SetAttackingCardAndPretendedDamage()
    {
        _attackingCard = _currentPlay.GetLastCard();
        _pretendedDamage = _notCurrentPlayer.CalculateDamage(_attackingCard);
    }

}