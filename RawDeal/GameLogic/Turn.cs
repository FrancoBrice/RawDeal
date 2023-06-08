using RawDeal.Cards;
using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.GameLogic;

public class Turn
{
    private readonly Play _currentPlay;
    private Player _currentPlayer;
    private Game _game;
    private Player _notCurrentPlayer;
    private readonly View _view;


    public Turn(Play currentPlay, View view)
    {
        _view = view;
        _currentPlay = currentPlay;
        SetPlayers();
    }

    public void PlayTurn(Game game)
    {
        _game = game;
        _view.SayThatATurnBegins(_currentPlayer.GetSuperStarName());
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
            ViewManager.ShowPlayersInfo(_view, _currentPlay);
            NextPlay nextPlay = UserAsker.GetNextPlay(_view, _currentPlayer);
            ExecuteNextPlay(nextPlay);
            UpdatePlayersFortitude();
            GameEndChecker.CheckForGameOver(_game);
        }
    }

    private void ExecuteAutomaticAbilities()
    {
        if (_currentPlayer.CanUseHisAbility() && _currentPlayer.IsAbilityAutomatic())
            _currentPlayer.UseSuperStarAbility(_notCurrentPlayer);
        if (_currentPlayer.IsAbilityAutomatic()) _currentPlayer.HasUsedHisAbilityInTheTurn = true;
    }

    private void ExecuteNextPlay(NextPlay nextPlay)
    {
        switch (nextPlay)
        {
            case NextPlay.ShowCards:
                ViewManager.ShowCardsBasedOnSelection(_view, _currentPlay);
                break;
            case NextPlay.PlayCard:
                _game.MakePlayManagerApplyPendingEffects();
                CardPlayer cardPlayer = new(_game, _view);
                cardPlayer.PlayCard(_game.PlayManager);
                break;
            case NextPlay.UseAbility:
                _currentPlayer.UseSuperStarAbility(_notCurrentPlayer);
                break;
            case NextPlay.EndTurn:
                _currentPlayer.HasEndsHisTurn = true;
                break;
            case NextPlay.GiveUp:
                _game.EndGame(_notCurrentPlayer);
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