using RawDealView;
using RawDealView.Options;

namespace RawDeal.GameLogic;

public class Turn
{
    private View _view;
    private ViewManager _viewManager;
    private Play CurrentPlay;
    private Player CurrentPlayer;
    private Player NotCurrentPlayer;
    private Game _game;
    

    public Turn(Play currentPlay, View view)
    {
        _view = view;
        _viewManager = new ViewManager(_view);
        CurrentPlay = currentPlay;
        SetPlayers();
    }
    
    public void PlayTurn(Game game)
    {
        _game = game;
        _view.SayThatATurnBegins(CurrentPlayer.SuperStar.Name);
        ResetPlayerStatusInTurn();
        ExecuteDrawSegment();
        ExecuteTurnLoop();

    }
    
    private void ResetPlayerStatusInTurn()
    {
        CurrentPlayer.HasUsedHisAbilityInTheTurn = false;
        CurrentPlayer.HasEndsHisTurn = false;
    }
    
    private void ExecuteDrawSegment()
    {
        CurrentPlayer.MoveCardFromArsenalToHand();
    }
    
    private void ExecuteTurnLoop()
    {
        while (!CurrentPlayer.HasEndsHisTurn && !_game.GameIsOver)
        {
            ExecuteAutomaticAbilities();
            _viewManager.ShowPlayersInfo(CurrentPlay);
            UserAsker userAsker = new UserAsker(_view);
            NextPlay nextPlay = userAsker.AskUserNextPlay(CurrentPlayer);
            ExecuteNextPlay(nextPlay);
            UpdatePlayersFortitude();
            _game.CheckForGameOver();

        }
    }
    
    private void ExecuteAutomaticAbilities()
    {
        if (CurrentPlayer.CanUseHisAbility() && CurrentPlayer.IsAbilityAutomatic())
        {
            CurrentPlayer.UseSuperStarAbility(NotCurrentPlayer);
        }
        if (CurrentPlayer.IsAbilityAutomatic()) CurrentPlayer.HasUsedHisAbilityInTheTurn = true;
        
    }
    
    private void ExecuteNextPlay(NextPlay nextPlay)
    {
        switch (nextPlay)
        {
            case NextPlay.ShowCards:
                _viewManager.ShowCardsBasedOnSelection(CurrentPlay);
                break;
            case NextPlay.PlayCard:
                _game.MakePlayManagerApplyPendingEffects();
                _game.PlayCard();
                break;
            case NextPlay.UseAbility:
                CurrentPlayer.UseSuperStarAbility(NotCurrentPlayer);
                break;
            case NextPlay.EndTurn:
                CurrentPlayer.HasEndsHisTurn = true;
                break;
            case NextPlay.GiveUp:
                _game.EndGame(winnerPlayer: NotCurrentPlayer);
                break;
        }
    }
    
    private void UpdatePlayersFortitude()
    {
        CurrentPlayer.UpdateFortitude();
        NotCurrentPlayer.UpdateFortitude();
        
    }
    
    private void SetPlayers()
    {
        CurrentPlayer = CurrentPlay.CurrentPlayer;
        NotCurrentPlayer = CurrentPlay.NotCurrentPlayer;
    }
    
}