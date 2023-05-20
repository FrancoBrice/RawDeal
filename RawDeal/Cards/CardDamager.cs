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
    private CardMobilizer _cardMobilizer;
    private Game _game;

    public CardDamager(Game game, View view)
    {
        _game = game;
        _view = view;
        _viewManager = new ViewManager(_view);
        _cardMobilizer = new CardMobilizer(_view);
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
        _opponentRanOutOfCards = GameEndChecker.OpponentRanOutOfCardsDuringDamage(damagedPlayer, _pretendedDamage);
        damagedPlayer.ReceiveDamageWithoutView(_actualDamage);
        FinishCardDamage(attackingPlayer);
    }
     
     private void DealActualDamage(Player damagedPlayer)
     {
         _actualDamage = 0;
         List<Card> cardsToBeDamaged = damagedPlayer.GetCardsFromArsenal(_pretendedDamage);
         _view.SayThatOpponentWillTakeSomeDamage(damagedPlayer.GetSuperStarName(), _pretendedDamage);
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
             CheckAndApplyReversalByDeck(attackingCard, damagedCard);
             if (_cardWasReversedByDeck) break;
         }
     }

     private void CheckAndApplyReversalByDeck(Card attackingCard, Card damagedCard)
     {
         List<Card> possibleReversals = _notCurrentPlayer.GetReversalsFromArsenal(attackingCard);
         if (possibleReversals.Contains(damagedCard) && _notCurrentPlayer.IsCorrectReversalCard(attackingCard, damagedCard))
         {
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
             _cardMobilizer.DrawStunValueCards(attackingPlayer, numberOfCardsToDraw);
             _view.SayThatPlayerDrawCards(attackingPlayer.GetSuperStarName(), numberOfCardsToDraw);
         }
     }
     
     private void FinishCardDamage(Player attackingPlayer)
     {
         EndGameIfOpponentRanOutOfCardsAndNotReverse(attackingPlayer);
         _game.MakePlayManagerRemoveEffectsOnCards();
     }

     private void EndGameIfOpponentRanOutOfCardsAndNotReverse(Player attackingPlayer)
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