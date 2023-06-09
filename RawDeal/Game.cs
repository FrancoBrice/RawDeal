using RawDeal.Cards;
using RawDeal.GameLogic;
using RawDeal.GameLogic.DecksLogic;
using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDeal.JsonReaders;
using RawDeal.SuperStars;
using RawDealView;

namespace RawDeal;

public class Game
{
    private readonly PlayManager _playManager;
    private readonly List<DeckValidator> _selectedDecks;
    private Play _currentPlay;
    private readonly View _view;
    private readonly string _deckFolder;
    private readonly List<Player> _playersList;
    private int _indexCurrentPlayer;
    private int _indexNotCurrentPlayer;
    private bool _gameIsOver;

    public Game(View view, string deckFolder)
    {
        _playersList = new List<Player>();
        _view = view;
        AllCardsList = CardsJsonReader.GenerateAllCardsListFromCardsFromJson();
        AllSuperStarList = SuperstarsJsonReader.GenerateAllSuperStarsListFromJson();
        _deckFolder = deckFolder;
        _gameIsOver = false;
        _indexCurrentPlayer = 0;
        _indexNotCurrentPlayer = 1;
        _selectedDecks = new List<DeckValidator>();
        _playManager = new PlayManager();
    }

    public List<Card> AllCardsList { get; }
    public List<SuperStar> AllSuperStarList { get; }
    public Player CurrentPlayer => _playersList[_indexCurrentPlayer];
    public Player NotCurrentPlayer => _playersList[_indexNotCurrentPlayer];

    public void Play()
    {
        DeckSelector deckSelector = new DeckSelector(this, _view);
        deckSelector.AskUsersToSelectDecks(_deckFolder);
        if (!deckSelector.AreDecksValid()) return;
        PlayersCreator playersCreator = new PlayersCreator(this, _view);
        playersCreator.CreatePlayers(_selectedDecks);
        OrderPlayersBySuperStarValue();
        ApplyInitialAbilities();
        RunGameLoop();
    }

    private void RunGameLoop()
    {
        while (!_gameIsOver)
        {
            if (NotCurrentPlayer.HasZeroCardsInArsenal()) EndGame(CurrentPlayer);
            _currentPlay = new Play(GetDictionaryOfCurrentAndNotCurrentPlayer());
            _playManager.AddPlay(_currentPlay);
            if (!_gameIsOver)
            {
                Turn currentTurn = new Turn(_playManager, _view);
                currentTurn.PlayTurn(this);
            }

            UpdatePlayersIndex();
        }
    }

    public void EndGame(Player winnerPlayer)
    {
        _gameIsOver = true;
        _view.CongratulateWinner(winnerPlayer.GetSuperStarName());
    }

    private void ApplyInitialAbilities()
    {
        foreach (Player player in _playersList) player.ExecuteInitialAbility();
    }

    public void MakePlayManagerApplyPendingEffects()
    {
        _playManager.ApplyPendingEffectsIfPossible();
    }

    public void MakePlayManagerRemoveEffectsOnCards()
    {
        _playManager.RemoveEffectsOnCards();
    }

    private void UpdatePlayersIndex()
    {
        _indexCurrentPlayer = (_indexCurrentPlayer + 1) % _playersList.Count;
        _indexNotCurrentPlayer = (_indexNotCurrentPlayer + 1) % _playersList.Count;
    }

    private void OrderPlayersBySuperStarValue()
    {
        if (_playersList[1].GetSuperStarValue() > _playersList[0].GetSuperStarValue())
            ExchangePlayersPositions(_playersList);
    }

    private static void ExchangePlayersPositions<TPlayer>(List<TPlayer> playersList)
    {
        (playersList[0], playersList[1]) = (playersList[1], playersList[0]);
    }

    public Dictionary<string, Player> GetDictionaryOfCurrentAndNotCurrentPlayer()
    {
        Dictionary<string, Player> players = new Dictionary<string, Player>
        {
            { "CurrentPlayer", CurrentPlayer },
            { "NotCurrentPlayer", NotCurrentPlayer }
        };
        return players;
    }

    public void AddPlayerToPlayersList(Player player)
    {
        _playersList.Add(player);
    }

    public Play GetCurrentPlay()
    {
        return _currentPlay;
    }

    public void AddDeckValidator(DeckValidator deck)
    {
        _selectedDecks.Add(deck);
    }

    public int GetSelectedDecksSize()
    {
        return _selectedDecks.Count;
    }

    public bool IsGameOver()
    {
        return _gameIsOver;
    }
}