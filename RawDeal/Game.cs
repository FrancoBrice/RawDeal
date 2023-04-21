using RawDealView;

namespace RawDeal;

public class Game
{
    private readonly View _view;
    private readonly string _deckFolder;
    private List<Card> AllCardsList { get; }
    private List<SuperStar> AllSuperStarList { get; }
    private List<Player> _playersList = new();
    private bool _gameIsOver;
    private int _indexCurrentPlayer;
    private int _indexNotCurrentPlayer;
    private bool _playerEndsHisTurn;
    private Player CurrentPlayer => _playersList[_indexCurrentPlayer];
    private Player NotCurrentPlayer => _playersList[_indexNotCurrentPlayer];

    public Game(View view, string deckFolder)
    {
        AllCardsList = JsonReader.GenerateAllCardsListFromCardsFromJson();
        AllSuperStarList = JsonReader.GenerateAllSuperStarsListFromJson();
        _view = view;
        _deckFolder = deckFolder;
        _gameIsOver = false;
        _indexCurrentPlayer = 0;
        _indexNotCurrentPlayer = 1;
        _playerEndsHisTurn = false;
    }

    private bool CheckIfThereIsInvalidDecks()
    {
        while (_playersList.Count < 2)
        {
            string deckPath = _view.AskUserToSelectDeck(_deckFolder);
            DeckValidator deck = GetDeckFromPath(deckPath);
        
            if (deck == null)
            {
                _view.SayThatDeckIsInvalid();
                return true;
            }
        
            Player player = CreatePlayerFromDeck(deck);
            if (player == null)
            {
                return true;
            }
            _playersList.Add(player);
        }
        return false;
    }
    
    private void EndGame(Player winnerPlayer)
    {
        _gameIsOver = true;
        _view.CongratulateWinner(winnerPlayer.GetSuperStarName());
    }

    private DeckValidator GetDeckFromPath(string path)
    {
        List<Card> cards = GetCardsFromPath(path);
        foreach (var VARIABLE in cards)
        {
            Console.WriteLine($"chequeando deckfrompath id {VARIABLE.Id}");
        }
        List<SuperStar> superstars = GetSuperStarsFromPath(path);
        return new DeckValidator(superstars, cards);
    }

    private List<Card> GetCardsFromPath(string path)
    {
        var cardStrings = File.ReadAllLines(path).Where(line => !line.Contains("(Superstar Card)"));
        List<Card> cardsList = new List<Card>();

        foreach (var cardString in cardStrings)
        {
            var card = AllCardsList.FirstOrDefault(card => card.Title == cardString);
            if (card == null)
            {
                return null;
            }
            cardsList.Add(card);
        }
        return cardsList;
    }

    private List<SuperStar> GetSuperStarsFromPath(string path)
    {
        var superstarStrings = File.ReadAllLines(path).Where(line => line.Contains("(Superstar Card)"));
        var superstars = new List<SuperStar>();
        foreach (var superstarString in superstarStrings)
        {
            string cardName = superstarString.Replace(" (Superstar Card)", "");
            var superstar = AllSuperStarList.FirstOrDefault(s => s.Name == cardName);
            if (superstar == null)
            {
                return null;
            }
            superstars.Add(superstar);
        }

        return superstars;
    }

    private Player CreatePlayerFromDeck(DeckValidator deckValidator)
    {
        if (!deckValidator.IsValidDeck())
        {
            _view.SayThatDeckIsInvalid();
            return null;
        }
        SuperStar superstar = deckValidator.SuperStarsList.First();
        List<Card> cardsList = deckValidator.CardList;
        foreach (var VARIABLE in cardsList)
        {
            Console.WriteLine($"mazo que crea jugador: {VARIABLE.Title} id: {VARIABLE.Id}");
        }
        Player player = new Player(superstar, cardsList, _view);
        return player;
    }
    
    private void PlayTurn()
    {
        _view.SayThatATurnBegins(CurrentPlayer.SuperStar.Name);
        ExecuteDrawSegment();
        ResetPlayerStatusInTurn();
        while (!_playerEndsHisTurn && !_gameIsOver)
        {
            ExecuteAutomaticAbilities();
            ShowPlayersInfo();
            if (CheckIfPlayerCanUseHisAbility(CurrentPlayer) && !CurrentPlayer.CheckIfAbilityIsAutomatic())
            {
                HandlePlayerInputWhenAbilityItsAvailable();
            }
            else
            {
                HandlePlayerInputWhenAbilityItsNotAvailable();
            }
        
            CurrentPlayer.UpdateFortitude();
            if (NotCurrentPlayer.PlayerHasLost())
            {
                EndGame(CurrentPlayer);
            }
        }
    }

    private void HandlePlayerInputWhenAbilityItsAvailable()
    {
        NextPlay playerInput = _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible();
        ExecuteNextPlay(playerInput);
    }
    private void HandlePlayerInputWhenAbilityItsNotAvailable()
    {
        NextPlay playerInput = _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
        ExecuteNextPlay(playerInput);
    }
    private void ResetPlayerStatusInTurn()
    {
        CurrentPlayer.HasUsedHisAbilityInTheTurn = false;
        _playerEndsHisTurn = false;
    }

    private void ExecuteAutomaticAbilities()
    {
        if (CurrentPlayer.CheckIfCanUseAbility() && CurrentPlayer.CheckIfAbilityIsAutomatic())
        {
            CurrentPlayer.UseSuperStarAbility(NotCurrentPlayer);
        }
    }

    private void ExecuteDrawSegment()
    {
        CurrentPlayer.MoveCardFromArsenalToHand();
    }

    private void ExecuteNextPlay(NextPlay nextPlay)
    {
        switch (nextPlay)
        {
            case NextPlay.ShowCards:
                ShowCardsBasedOnSelection(CurrentPlayer);
                break;
            case NextPlay.PlayCard:
                PlayCard();
                break;
            case NextPlay.UseAbility:
                CurrentPlayer.UseSuperStarAbility(NotCurrentPlayer);
                break;
            case NextPlay.EndTurn:
                _playerEndsHisTurn = true;
                break;
            case NextPlay.GiveUp:
                EndGame(NotCurrentPlayer);
                break;
        }
    }


    private void PlayCard()
    {
        List<Card> playableCardsFromPlayer = CurrentPlayer.GetPlayableCardsFromPlayer();
        List<string> playableCardsFormatted = GeneratePlayableCardsFormatted(playableCardsFromPlayer);
        int userInput = _view.AskUserToSelectAPlay(playableCardsFormatted);
        if (userInput != -1)
        {
            Card selectedCard = playableCardsFromPlayer[userInput];
            PlaySelectedCard(selectedCard);
        }
    }

    
    private List<string> GeneratePlayableCardsFormatted(List<Card> playableCardsFromPlayer)
    {
        return playableCardsFromPlayer.Select(card => card.GetCardInPlayFormat()).ToList();
    }


    private void PlaySelectedCard(Card selectedCard)
    {
        string cardInPlayFormat = selectedCard.GetCardInPlayFormat();
        _view.SayThatPlayerIsTryingToPlayThisCard(CurrentPlayer.GetSuperStarName(), cardInPlayFormat);
        CurrentPlayer.MoveCardFromHandToRingAreaById(selectedCard.Id);
        _view.SayThatPlayerSuccessfullyPlayedACard();
        int actualDamage = NotCurrentPlayer.CalculateDamage(selectedCard.GetDamage());
        List<Card> notCurrentPlayerDamagedCards = NotCurrentPlayer.GetCardsFromArsenal(actualDamage);
        NotCurrentPlayer.ReceiveDamage(actualDamage);
        ShowDamagedCards(notCurrentPlayerDamagedCards, actualDamage);
    }

    private void ShowDamagedCards(List<Card> damagedCards, int actualDamage)
    {
        int indexShowedCard = 1;
        for (int i = damagedCards.Count - 1; i >= 0; i--)
        {
            string cardFormattedInfo = damagedCards[i].GetCardFormattedInfo();
            _view.ShowCardOverturnByTakingDamage(cardFormattedInfo, indexShowedCard, actualDamage);
            indexShowedCard++;
        }
    }
    
    private Card ExtractCardFromTuple((int, Card) tupleIndexCard)
    {
        return tupleIndexCard.Item2;
    }

    private bool CheckIfPlayerCanUseHisAbility(Player currentPlayer)
    {
        return currentPlayer.CheckIfCanUseAbility();
    }

    public void Play()
    {
        bool thereIsInvalidDeck = CheckIfThereIsInvalidDecks();
        if (!thereIsInvalidDeck)
        {
            _playersList = OrderPlayersBySuperStarValue(_playersList);
            AplyInitialSuperStarAbilities();
            while (!_gameIsOver)
            {
                bool notCurrentPlayerHasLost =  NotCurrentPlayer.PlayerHasLost();
                if (notCurrentPlayerHasLost)
                {
                    EndGame(CurrentPlayer);
                }
                if (!_gameIsOver)
                {
                    PlayTurn();
                }
                UpdatePlayersIndex();
            }
        }
    }

    private void UpdatePlayersIndex()
    {
        _indexCurrentPlayer = (_indexCurrentPlayer + 1) % _playersList.Count;
        _indexNotCurrentPlayer = (_indexNotCurrentPlayer + 1) % _playersList.Count;
    }

    private List<Player> OrderPlayersBySuperStarValue(List<Player> playersList)
    {
        if (playersList[1].SuperStar.SuperstarValue > playersList[0].SuperStar.SuperstarValue)
        {
            SwapPlayers(playersList);
        }
        return playersList;
    }
    
    private void ShowCardsBasedOnSelection(Player player)
    {
        CardSet setOfCardsSelected = _view.AskUserWhatSetOfCardsHeWantsToSee();
        List<string> cardstringsList = GenerateCardsStringsList(setOfCardsSelected);
        _view.ShowCards(cardstringsList);
    }
    
    private List<string> GenerateCardsStringsList(CardSet setOfCardsSelected)
    {
        List<string> cardstringsList = new List<string>();
        switch (setOfCardsSelected)
        {
            case CardSet.RingArea:
                cardstringsList = CurrentPlayer.GetCardsFromRingAreaInStringFormat();
                break;
            case CardSet.Hand:
                cardstringsList = CurrentPlayer.GetCardsFromHandInStringFormat();
                break;
            case CardSet.RingsidePile:
                cardstringsList = CurrentPlayer.GetCardsFromRingsideInStringFormat();
                break;
            case CardSet.OpponentsRingArea:
                cardstringsList = NotCurrentPlayer.GetCardsFromRingAreaInStringFormat();
                break;
            case CardSet.OpponentsRingsidePile:
                cardstringsList = NotCurrentPlayer.GetCardsFromRingsideInStringFormat();
                break;
        }
        return cardstringsList;
    }


    private void ShowPlayersInfo()
    {
        List<Player> playersList = new() { CurrentPlayer, NotCurrentPlayer };
        List<PlayerInfo> playerInfoList = new();
        foreach (Player player in playersList)
        {
            PlayerInfo playerInfo = GeneratePlayerInfo(player);
                playerInfoList.Add(playerInfo);
        }
        _view.ShowGameInfo(playerInfoList[0], playerInfoList[1]);
    }

    private PlayerInfo GeneratePlayerInfo(Player player)
    {
        return new PlayerInfo(player.GetSuperStarName(), player.Fortitude, player.GetHandSize(),
                player.GetArsenalSize());
    }

    private void AplyInitialSuperStarAbilities()
    {
        foreach (Player player in _playersList)
        {
            player.ExecuteInitialAbility();
        }
    }

    private static void SwapPlayers<TPlayer>(IList<TPlayer> list)
    {
        (list[0], list[1]) = (list[1], list[0]);
    }

}