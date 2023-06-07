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
    private Player _currentPlayer;
    private Player _notCurrentPlayer;
    private readonly Play _currentPlay;
    private bool _cardWasReversedByDeck;
    private bool _cardWasReversedInLastCardOfDeck;
    private int _pretendedDamage;
    private int _actualDamage;
    private bool _opponentRanOutOfCards;
    private List<Card> _actualDamagedCards;
    private Card _attackingCard;
    private readonly View _view;
    private readonly Game _game;

    public CardDamager(Game game, View view)
    {
        _game = game;
        _view = view;
        _currentPlay = _game.CurrentPlay;
    }

    public void ApplyCardDamage()
    {
        SetPlayers(_currentPlay);
        _attackingCard = _currentPlay.GetLastCard();
        _pretendedDamage = _notCurrentPlayer.CalculateDamage(_attackingCard);
        if (_pretendedDamage == 0) return;
        DealActualDamage(_notCurrentPlayer);
        HandleReversalsByDeck(_currentPlayer, _notCurrentPlayer);
        _opponentRanOutOfCards =
            GameEndChecker.PlayerRanOutOfCardsDuringDamage(_notCurrentPlayer, _pretendedDamage);
        _notCurrentPlayer.ReceiveDamage(_actualDamage);
        FinishCardDamage(_currentPlayer);
    }

    private void DealActualDamage(Player damagedPlayer)
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
         if (_cardWasReversedByDeck)
         {
             _view.SayThatCardWasReversedByDeck(damagedPlayer.GetSuperStarName());
             HandleStunValue(attackingPlayer, _attackingCard, _cardWasReversedInLastCardOfDeck);
         }
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

     private void CheckAndApplyReversalByDeck(Card damagedCard, int index)
     {
         List<Card> possibleReversals = _notCurrentPlayer.GetReversalsFromArsenal(_game.PlayManager);
         damagedCard.PlayedFrom = "Arsenal";
         if (!possibleReversals.Contains(damagedCard) ||
             !ReversalsChecker.IsCorrectReversalCard(_game.PlayManager, damagedCard)) return;
         _cardWasReversedByDeck = true;
         _currentPlayer.HasEndsHisTurn = true;
         _currentPlay.SetReversalCardTuple((index, damagedCard));
         List<Effect> effectsAssigned = 
             EffectAssigner.AssignEffect(_game, _currentPlay.ReversalCard);
         EffectsApplier.ApplyAssignedEffects(_game, effectsAssigned);
         if (_actualDamage == _pretendedDamage) _cardWasReversedInLastCardOfDeck = true;
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
     
     public void BeginCollateralDamage(int damageAmount)
     {
         if (damageAmount == 0) return;
         SetPlayers(_currentPlay);
         _view.SayThatSuperstarWillTakeSomeDamage(_currentPlayer.GetSuperStarName(), damageAmount);
         List<Card> cardsToBeDamaged = _currentPlayer.GetCardsFromArsenal(damageAmount);
         ViewManager.ShowDamagedCards(_view, cardsToBeDamaged, damageAmount);
         AddCardsToCardToBeDamaged(cardsToBeDamaged);
         ApplyCollateralDamage(damageAmount, cardsToBeDamaged);
     }

     private void ApplyCollateralDamage(int damageAmount, List<Card> cardsToBeDamaged)
     {
         _currentPlayer.ReceiveDamage(cardsToBeDamaged.Count - 1);
         _opponentRanOutOfCards =
             GameEndChecker.PlayerRanOutOfCardsDuringDamage(_currentPlayer, damageAmount);
         if (_opponentRanOutOfCards)
             _view.SayThatPlayerLostDueToSelfDamage(_currentPlayer.GetSuperStarName());
         EndGameIfPlayerRanOutOfCardsAndNotReverse(_currentPlay.NotCurrentPlayer);
     }

     private static void AddCardsToCardToBeDamaged(List<Card> cardsToBeDamaged)
     {
         for (int index = cardsToBeDamaged.Count - 1; index >= 0; index--)
         {
             Card damagedCard = cardsToBeDamaged[index];
             cardsToBeDamaged.Add(damagedCard);
         }
     }

     private void EndGameIfPlayerRanOutOfCardsAndNotReverse(Player attackingPlayer)
     {
         if (_opponentRanOutOfCards && !_cardWasReversedByDeck) _game.EndGame(attackingPlayer);
     }
     
     private void SetPlayers(Play currentPlay)
     {
         _currentPlayer = currentPlay.CurrentPlayer;
         _notCurrentPlayer = currentPlay.NotCurrentPlayer;
     }
}