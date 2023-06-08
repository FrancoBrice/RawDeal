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
    private readonly string _deckFolder;
    public readonly List<Player> PlayersList;
    public readonly PlayManager PlayManager;
    public readonly List<DeckValidator> SelectedDecks;
    public readonly View ViewObject;
    private int _indexCurrentPlayer;
    private int _indexNotCurrentPlayer;
    public Play CurrentPlay;
    public bool GameIsOver;

    public Game(View viewObject, string deckFolder)
    {
        PlayersList = new List<Player>();
        ViewObject = viewObject;
        AllCardsList = CardsJsonReader.GenerateAllCardsListFromCardsFromJson();
        AllSuperStarList = SuperstarsJsonReader.GenerateAllSuperStarsListFromJson();
        _deckFolder = deckFolder;
        GameIsOver = false;
        _indexCurrentPlayer = 0;
        _indexNotCurrentPlayer = 1;
        SelectedDecks = new List<DeckValidator>();
        PlayManager = new PlayManager(ViewObject);
    }

    public List<Card> AllCardsList { get; }
    public List<SuperStar> AllSuperStarList { get; }
    public Player CurrentPlayer => PlayersList[_indexCurrentPlayer];
    public Player NotCurrentPlayer => PlayersList[_indexNotCurrentPlayer];

    public void Play()
    {
        DeckSelector deckSelector = new(this, ViewObject);
        deckSelector.AskUsersToSelectDecks(_deckFolder);
        if (!deckSelector.AreDecksValid()) return;
        PlayersCreator playersCreator = new(this, ViewObject);
        playersCreator.CreatePlayers(SelectedDecks);
        OrderPlayersBySuperStarValue();
        ApplyInitialAbilities();
        RunGameLoop();
    }

    private void RunGameLoop()
    {
        while (!GameIsOver)
        {
            if (NotCurrentPlayer.HasZeroCardsInArsenal()) EndGame(CurrentPlayer);
            CurrentPlay = new Play(GetDictionaryOfCurrentAndNotCurrentPlayer(), ViewObject);
            PlayManager.AddPlay(CurrentPlay);
            if (!GameIsOver)
            {
                Turn currentTurn = new(CurrentPlay, ViewObject);
                currentTurn.PlayTurn(this);
            }

            UpdatePlayersIndex();
        }
    }

    public void EndGame(Player winnerPlayer)
    {
        GameIsOver = true;
        ViewObject.CongratulateWinner(winnerPlayer.GetSuperStarName());
    }

    private void ApplyInitialAbilities()
    {
        foreach (Player player in PlayersList) player.ExecuteInitialAbility();
    }

    public void MakePlayManagerApplyPendingEffects()
    {
        PlayManager.ApplyPendingEffectsIfPossible();
    }

    public void MakePlayManagerRemoveEffectsOnCards()
    {
        PlayManager.RemoveEffectsOnCards();
    }

    private void UpdatePlayersIndex()
    {
        _indexCurrentPlayer = (_indexCurrentPlayer + 1) % PlayersList.Count;
        _indexNotCurrentPlayer = (_indexNotCurrentPlayer + 1) % PlayersList.Count;
    }

    private void OrderPlayersBySuperStarValue()
    {
        if (PlayersList[1].GetSuperStarValue() > PlayersList[0].GetSuperStarValue())
            ExchangePlayersPositions(PlayersList);
    }

    private static void ExchangePlayersPositions<TPlayer>(List<TPlayer> playersList)
    {
        (playersList[0], playersList[1]) = (playersList[1], playersList[0]);
    }

    public Dictionary<string, Player> GetDictionaryOfCurrentAndNotCurrentPlayer()
    {
        Dictionary<string, Player> players = new()
        {
            { "CurrentPlayer", CurrentPlayer },
            { "NotCurrentPlayer", NotCurrentPlayer }
        };
        return players;
    }
}