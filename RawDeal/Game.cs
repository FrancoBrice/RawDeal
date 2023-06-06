using RawDeal.Cards;
using RawDeal.GameLogic;
using RawDeal.JsonReader;
using RawDeal.SuperStars;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal;

public class Game 
{
    public List<Card> AllCardsList { get; }
    public List<SuperStar> AllSuperStarList { get; }
    public List<Player> PlayersList;
    public bool GameIsOver;
    private int _indexCurrentPlayer;
    private int _indexNotCurrentPlayer;
    public Player CurrentPlayer => PlayersList[_indexCurrentPlayer];
    public Player NotCurrentPlayer => PlayersList[_indexNotCurrentPlayer];
    public List<DeckValidator> SelectedDecks;
    public Play CurrentPlay;
    public PlayManager PlayManager;
    public readonly View ViewObject;
    private readonly string _deckFolder;
    
    public Game(View viewObject, string deckFolder)
    {
        PlayersList = new List<Player>();
        ViewObject = viewObject;
        AllCardsList = CardsJsonReader.GenerateAllCardsListFromCardsFromJson();
        SetViewObjectInCards(ViewObject);
        AllSuperStarList = SuperstarsJsonReader.GenerateAllSuperStarsListFromJson();
        _deckFolder = deckFolder;
        GameIsOver = false;
        _indexCurrentPlayer = 0;
        _indexNotCurrentPlayer = 1;
        SelectedDecks = new List<DeckValidator>();
        PlayManager = new PlayManager(ViewObject);
    }

    public void Play()
    {
        DeckSelector deckSelector= new DeckSelector(this, ViewObject);
        deckSelector.AskUsersToSelectDecks(_deckFolder);
        if (deckSelector.AreDecksValid())
        {
            PlayersCreator playersCreator = new PlayersCreator(this, ViewObject);
            playersCreator.CreatePlayers(SelectedDecks);
            OrderPlayersBySuperStarValue();
            ApplyInitialAbilities();
            RunGameLoop();
        }
    }
    
    private void RunGameLoop()
    {
        while (!GameIsOver)
        {
            if (NotCurrentPlayer.HasZeroCardsInArsenal()) EndGame(winnerPlayer: CurrentPlayer);
            CurrentPlay = new Play(GetDictionaryOfCurrentAndNotCurrentPlayer(), ViewObject);
            PlayManager.AddPlay(CurrentPlay);
            if (!GameIsOver)
            {
                Turn currentTurn = new Turn(CurrentPlay, ViewObject);
                currentTurn.PlayTurn(game: this);
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
        foreach (Player player in PlayersList)
        {
            player.ExecuteInitialAbility();
        }
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
        if (PlayersList[1].SuperStar.SuperstarValue > PlayersList[0].SuperStar.SuperstarValue)
        {
            ExchangePlayersPositions(PlayersList);
        }
    }

    private static void ExchangePlayersPositions<TPlayer>(List<TPlayer> playersList)
    {
        (playersList[0], playersList[1]) = (playersList[1], playersList[0]);
    }

    private void SetViewObjectInCards(View view)
    {
        foreach (Card card in AllCardsList)
        {
            card.SetViewObject(view);
        }
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
}