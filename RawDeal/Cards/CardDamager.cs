using RawDeal.Cards.CardEffects;
using RawDeal.Cards.CardPreConditions;
using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards;

public class CardDamager
{
    private Player _currentPlayer;
    private Player _notCurrentPlayer;
    private Play _currentPlay;
    private bool _cardWasReversedByDeck;
    private bool _cardWasReversedInLastCardOfDeck;
    private int _pretendedDamage;
    private int _actualDamage;
    private bool _opponentRanOutOfCards;
    private List<Card> _actualDamagedCards;
    private Card _attackingCard;
    private readonly View _view;
    private readonly ViewManager _viewManager;
    private Game _game;

    public CardDamager(Game game, View view)
    {
        _game = game;
        _view = view;
        _viewManager = new ViewManager(_view);
        _currentPlay = _game.CurrentPlay;
    }

    public void ApplyCardDamage()
    {
        SetPlayers(_currentPlay);
        Player attackingPlayer = _currentPlay.CurrentPlayer;
        Player damagedPlayer = _currentPlay.NotCurrentPlayer;
        _attackingCard = _currentPlay.GetLastCard();
        _pretendedDamage = damagedPlayer.CalculateDamage(_attackingCard);
        if (_pretendedDamage == 0) return;
        DealActualDamage(damagedPlayer);
        HandleReversalsByDeck(attackingPlayer, damagedPlayer);
        _opponentRanOutOfCards = GameEndChecker.PlayerRanOutOfCardsDuringDamage(damagedPlayer, _pretendedDamage);
        damagedPlayer.ReceiveDamage(_actualDamage);
        damagedPlayer.DamagesReceived.Add(_actualDamage);
        FinishCardDamage(attackingPlayer);
    }
    
    public void ApplyCollateralCardDamage(int damageAmount)
    {
        _cardWasReversedByDeck = false;
        SetPlayers(_currentPlay);
        _view.SayThatSuperstarWillTakeSomeDamage(_currentPlayer.GetSuperStarName(), damageToBeReceived: damageAmount);
        Player damagedPlayer = _currentPlay.CurrentPlayer;
        if (damageAmount == 0) return;
        _pretendedDamage = damageAmount;
        List<Card> cardsToBeDamaged = damagedPlayer.GetCardsFromArsenal(damageAmount);
        _viewManager.ShowDamagedCards(cardsToBeDamaged, _pretendedDamage);
        _actualDamage = 0;
        for (int index = cardsToBeDamaged.Count - 1; index >= 0; index--)
        {
            _actualDamage++;
            Card damagedCard = cardsToBeDamaged[index];
            cardsToBeDamaged.Add(damagedCard);
        }
        _opponentRanOutOfCards = GameEndChecker.PlayerRanOutOfCardsDuringDamage(damagedPlayer, _pretendedDamage);
        if (_opponentRanOutOfCards)
        {
            _view.SayThatPlayerLostDueToSelfDamage(damagedPlayer.GetSuperStarName());
        }
        damagedPlayer.ReceiveDamage(_actualDamage);
        EndGameIfPlayerRanOutOfCardsAndNotReverse(_currentPlay.NotCurrentPlayer);
    }
     
     private void DealActualDamage(Player damagedPlayer)
     {
         _actualDamage = 0;
         List<Card> cardsToBeDamaged = damagedPlayer.GetCardsFromArsenal(_pretendedDamage);
         _view.SayThatSuperstarWillTakeSomeDamage(damagedPlayer.GetSuperStarName(), _pretendedDamage);
         RunDamageLoop(cardsToBeDamaged, _attackingCard);
         _viewManager.ShowDamagedCards(_actualDamagedCards, _pretendedDamage);
     }
     
     private void HandleReversalsByDeck(Player attackingPlayer, Player damagedPlayer)
     {
         if (_cardWasReversedByDeck)
         {
             _view.SayThatCardWasReversedByDeck(damagedPlayer.GetSuperStarName());
             HandleStunValue(attackingPlayer, _attackingCard, _cardWasReversedInLastCardOfDeck);
         }
     }
     
     private void RunDamageLoop(List<Card> cardsToBeDamaged, Card attackingCard)
     {
         _actualDamagedCards = new List<Card>();
         for (int index = cardsToBeDamaged.Count - 1; index >= 0; index--)
         {
             _actualDamage++;
             Card damagedCard = cardsToBeDamaged[index];
             _actualDamagedCards.Add(damagedCard);
             CheckAndApplyReversalByDeck(attackingCard, damagedCard, index);
             if (_cardWasReversedByDeck) break;
         }
     }

     private void CheckAndApplyReversalByDeck(Card attackingCard, Card damagedCard, int index)
     {
         List<Card> possibleReversals = _notCurrentPlayer.GetReversalsFromArsenal(_game.PlayManager);
         if (possibleReversals.Contains(damagedCard) && ReversalsChecker.IsCorrectReversalCard(_game.PlayManager, damagedCard, "Arsenal") && attackingCard.CanBeReversed)
         {
             damagedCard.PlayedFrom = "Deck";
             _currentPlay.SetReversalCardTuple((index, damagedCard));
             List<Effect> effectsAssigned = EffectAssigner.AssignReversalEffect(_game);
             EffectsApplier.ApplyAssignedEffects(_game, effectsAssigned);
             _cardWasReversedByDeck = true;
             _currentPlayer.HasEndsHisTurn = true;
             if (_actualDamage == _pretendedDamage) _cardWasReversedInLastCardOfDeck = true;
         }
     }
     
     private void HandleStunValue(Player attackingPlayer, Card attackingCard, bool cardWasReversedInLastCardOfDeck)
     {
         if (attackingCard.GetStunValue() > 0 && !cardWasReversedInLastCardOfDeck)
         {
             int numberOfCardsToDraw = _view.AskHowManyCardsToDrawBecauseOfStunValue(attackingPlayer.GetSuperStarName(),
                 attackingCard.GetStunValue());
             if (numberOfCardsToDraw == 0) return;
             CardMobilizer.DrawStunValueCards(attackingPlayer, numberOfCardsToDraw);
             _view.SayThatPlayerDrawCards(attackingPlayer.GetSuperStarName(), numberOfCardsToDraw);
         }
     }
     
     private void FinishCardDamage(Player attackingPlayer)
     {
         EndGameIfPlayerRanOutOfCardsAndNotReverse(attackingPlayer);
         _game.MakePlayManagerRemoveEffectsOnCards();
     }

     private void EndGameIfPlayerRanOutOfCardsAndNotReverse(Player attackingPlayer)
     {
         if (_opponentRanOutOfCards && !_cardWasReversedByDeck)
         {
             _game.EndGame(attackingPlayer);
         }
     }
     
     private void SetPlayers(Play currentPlay)
     {
         _currentPlayer = currentPlay.CurrentPlayer;
         _notCurrentPlayer = currentPlay.NotCurrentPlayer;
     }
}