using RawDeal.Cards;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.GameLogic;

public class Turn
{
    private View _view;
    private ViewManager _viewManager;
    private Play _currentPlay;
    private Player _currentPlayer;
    private Player _notCurrentPlayer;
    private Game _game;
    

    public Turn(Play currentPlay, View view)
    {
        _view = view;
        _viewManager = new ViewManager(_view);
        _currentPlay = currentPlay;
        SetPlayers();
    }
    
    public void PlayTurn(Game game)
    {
        _game = game;
        _view.SayThatATurnBegins(_currentPlayer.SuperStar.Name);
        ResetPlayerStatusInTurn();
        RunDrawSegment();
        RunTurnLoop();

    }
    
    private void ResetPlayerStatusInTurn()
    {
        _currentPlayer.HasUsedHisAbilityInTheTurn = false;
        _currentPlayer.HasEndsHisTurn = false;
    }
    
    private void RunDrawSegment()
    {
        _currentPlayer.MoveCardFromArsenalToHand();
    }
    
    private void RunTurnLoop()
    {
        while (!_currentPlayer.HasEndsHisTurn && !_game.GameIsOver)
        {
            ExecuteAutomaticAbilities();
            _viewManager.ShowPlayersInfo(_currentPlay);
            UserAsker userAsker = new UserAsker(_view);
            NextPlay nextPlay = userAsker.GetNextPlay(_currentPlayer);
            ExecuteNextPlay(nextPlay);
            UpdatePlayersFortitude();
            GameEndChecker.CheckForGameOver(_game);

        }
    }
    
    private void ExecuteAutomaticAbilities()
    {
        if (_currentPlayer.CanUseHisAbility() && _currentPlayer.IsAbilityAutomatic())
        {
            _currentPlayer.UseSuperStarAbility(_notCurrentPlayer);
        }
        if (_currentPlayer.IsAbilityAutomatic()) _currentPlayer.HasUsedHisAbilityInTheTurn = true;
        
    }
    
    private void ExecuteNextPlay(NextPlay nextPlay)
    {
        switch (nextPlay)
        {
            case NextPlay.ShowCards:
                _viewManager.ShowCardsBasedOnSelection(_currentPlay);
                break;
            case NextPlay.PlayCard:
                _game.MakePlayManagerApplyPendingEffects();
                CardPlayer cardPlayer = new CardPlayer(_game, _view);
                cardPlayer.PlayCard();
                break;
            case NextPlay.UseAbility:
                _currentPlayer.UseSuperStarAbility(_notCurrentPlayer);
                break;
            case NextPlay.EndTurn:
                _currentPlayer.HasEndsHisTurn = true;
                break;
            case NextPlay.GiveUp:
                _game.EndGame(winnerPlayer: _notCurrentPlayer);
                break;
        }
    }
    
    private void UpdatePlayersFortitude()
    {
        _currentPlayer.UpdateFortitude();
        _notCurrentPlayer.UpdateFortitude();
        
    }
    
    private void SetPlayers()
    {
        _currentPlayer = _currentPlay.CurrentPlayer;
        _notCurrentPlayer = _currentPlay.NotCurrentPlayer;
    }
    
}