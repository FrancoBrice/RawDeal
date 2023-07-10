using RawDeal.CardCollections;
using RawDeal.Cards.CardPreConditions;
using RawDeal.GameLogic;
using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards;

public class CardDamageController
{
    private protected readonly Play _currentPlay;
    private protected readonly Game _game;
    private protected readonly View _view;
    private protected int _actualDamage;
    private CardCollection _actualDamagedCards;
    private protected Card _attackingCard;
    private protected bool _cardWasReversedByDeck;
    private protected Player _currentPlayer;
    private protected Player _notCurrentPlayer;
    private protected bool _opponentRanOutOfCards;
    private protected int _pretendedDamage;
    private protected PlayManager _playManager;
    private ReversalsByDeckController _reversalsByDeckController;

    public CardDamageController(Game game, View view)
    {
        _game = game;
        _view = view;
        _currentPlay = _game.GetCurrentPlay();
    }

    public void BeginApplicationOfCardDamage(PlayManager playManager)
    {
        _playManager = playManager;
        _reversalsByDeckController = new ReversalsByDeckController(_game, _playManager, _view);
        SetPlayers(_currentPlay);
        SetAttackingCardAndPretendedDamage();
        if (_pretendedDamage == 0) return;
        ApplyCardDamage();
    }

    private void ApplyCardDamage()
    {
        HandleActualDamage(_notCurrentPlayer);
        if (_cardWasReversedByDeck)
        {
            _reversalsByDeckController.HandleReversalsByDeck();
        }
        _opponentRanOutOfCards =
            GameEndChecker.PlayerRanOutOfCardsDuringDamage(_notCurrentPlayer, _pretendedDamage);
        PlayerReceiveDamage(_notCurrentPlayer, _actualDamage);
        FinishCardDamage(_currentPlayer);
    }

    private void HandleActualDamage(Player damagedPlayer)
    {
        _actualDamage = 0;
        CardCollection cardsToBeDamaged = damagedPlayer.GetCardsFromArsenal(_pretendedDamage);
        _view.SayThatSuperstarWillTakeSomeDamage(damagedPlayer.GetSuperStarName(),
            _pretendedDamage);
        RunDamageLoop(cardsToBeDamaged);
        ViewManager.ShowDamagedCards(_view, _actualDamagedCards, _pretendedDamage);
    }
    
    private void RunDamageLoop(CardCollection cardsToBeDamaged)
    {
        _actualDamagedCards = new CardCollection();
        for (int index = cardsToBeDamaged.Count - 1; index >= 0; index--)
        {
            _actualDamage++;
            Card damagedCard = cardsToBeDamaged.CardList[index];
            _actualDamagedCards.Add(damagedCard);
            if (_reversalsByDeckController.IsPossibleApplyReversalByDeck(damagedCard))
            {
                _reversalsByDeckController.SetActualDamage(_actualDamage);
                _reversalsByDeckController.ApplyReversalByDeckIfPossible(damagedCard, index);
                _cardWasReversedByDeck = true;
                break;
            }
        }
    }
    
    public static void PlayerReceiveDamage(Player player, int damage)
    {
        CardMobilizer.MoveFromArsenalToRingsideByDamageAmount(player, damage);
        player.DamagesReceived.Add(damage);
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
    
    protected void SetAttackingCardAndPretendedDamage()
    {
        if (_currentPlay.PlayedCardsCount == 0) return;
        _attackingCard = _currentPlay.GetLastCard();
        _pretendedDamage = _notCurrentPlayer.CalculateDamage(_attackingCard);
    }

}