using RawDealView;

namespace RawDeal;

public class Game
{
    private readonly View _view;
    private readonly string _deckFolder;
    private List<Card> AllCardsList { get; set; }
    private List<SuperStar> AllSuperStarList { get; set; }
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
        List<Card> cards = GetCardsFromDeck(path);
        List<SuperStar> superStars = GetSuperStarsListFromDeck(path);
        return new DeckValidator(superStars, cards);
    }

    private List<Card> GetCardsFromDeck(string path)
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

    private List<SuperStar> GetSuperStarsListFromDeck(string path)
    {
        var superstarStrings = File.ReadAllLines(path).Where(line => line.Contains("(Superstar Card)"));
        var superStarsList = new List<SuperStar>();

        foreach (var superstarString in superstarStrings)
        {
            string cardName = superstarString.Replace(" (Superstar Card)", "");
            var superstar = AllSuperStarList.FirstOrDefault(s => s.Name == cardName);
            if (superstar == null)
            {
                return null;
            }
            superStarsList.Add(superstar);
        }

        return superStarsList;
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
        Player player = new Player(superstar, cardsList, _view);
        return player;
    }
    
    private void PlayTurn()
    {
        _view.SayThatATurnBegins(CurrentPlayer.SuperStar.Name);
        ResetPlayerStatusInTurn();
        ExecuteDrawSegment();
        ExecuteTurnLoop();
    }

    private void ExecuteTurnLoop()
    {
        while (!_playerEndsHisTurn && !_gameIsOver)
        {
            ExecuteAutomaticAbilities();
            ShowPlayersInfo();
            NextPlay nextPlay = AskUserNextPlay();
            ExecuteNextPlay(nextPlay);
            CurrentPlayer.UpdateFortitude();
            CheckForGameOver();
        }
    }

    private NextPlay AskUserNextPlay()
    {
        bool canUserUseHisAbility = CheckIfPlayerCanUseHisAbility(CurrentPlayer);
        if (canUserUseHisAbility && !CurrentPlayer.CheckIfAbilityIsAutomatic())
        {
            return _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible();
        }
        return _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
    }

    private void CheckForGameOver()
    {
        if (NotCurrentPlayer.PlayerHasLost())
        {
            EndGame(CurrentPlayer);
        }
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
        List<(int, Card)> tuplesPlayableCardsFromPlayer = CurrentPlayer.GetPlayableCardsFromPlayer();
        List<string> playableCardsFormatted = GeneratePlayableCardsFormatted(tuplesPlayableCardsFromPlayer);
        int userInput = _view.AskUserToSelectAPlay(playableCardsFormatted);
        if (userInput != -1)
        {
            int cardIndexSelected = userInput;
            Card selectedCard = tuplesPlayableCardsFromPlayer[cardIndexSelected].Item2;
            int actualDamage = NotCurrentPlayer.CalculateDamage(selectedCard.GetDamage());
            List<Card> notCurrentPlayerDamagedCards = NotCurrentPlayer.GetCardsFromArsenal(actualDamage);
            int positionInHand = tuplesPlayableCardsFromPlayer[cardIndexSelected].Item1;
            PerformCardAction(selectedCard, positionInHand);
            NotCurrentPlayer.ReceiveDamage(actualDamage);
            ShowDamagedCards(notCurrentPlayerDamagedCards, actualDamage);
        }
    }

    
    private List<string> GeneratePlayableCardsFormatted(List<(int, Card)> playableCardsFromPlayer)
    {
        return playableCardsFromPlayer.Select(tuple => ExtractCardFromTuple(tuple).GetCardInPlayFormat()).ToList();
    }


    private void PerformCardAction(Card selectedCard, int cardIndexInHand)
    {
        string cardInPlayFormat = selectedCard.GetCardInPlayFormat();
        _view.SayThatPlayerIsTryingToPlayThisCard(CurrentPlayer.GetSuperStarName(), cardInPlayFormat);
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }

    private void ShowDamagedCards(List<Card> damagedCards, int actualDamage)
    {
        int indexShowedCard = 1;
        for (int indexInDamagedCards = damagedCards.Count - 1; indexInDamagedCards >= 0; indexInDamagedCards--)
        {
            string cardFormattedInfo = damagedCards[indexInDamagedCards].GetCardFormattedInfo();
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
            AplyInitialAbilities();
            RunGameLoop();
        }
    }

    private void RunGameLoop()
    {
        while (!_gameIsOver)
        {
            if (NotCurrentPlayer.PlayerHasLost()) EndGame(CurrentPlayer);
            if (!_gameIsOver) PlayTurn();
            UpdatePlayersIndex();
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
        List<string> cardstringsList = GetCardsInStringFormatFromSelectedSet(setOfCardsSelected);
        _view.ShowCards(cardstringsList);
    }
    

    private List<string> GetCardsInStringFormatFromSelectedSet(CardSet setOfCardsSelected)
    {
        List<string> cardStrings = new List<string>();
        
        switch (setOfCardsSelected)
        {
            case CardSet.RingArea:
                cardStrings = CurrentPlayer.GetCardsInStringFormatFromRingArea();
                break;
            case CardSet.Hand:
                cardStrings = CurrentPlayer.GetCardsInStringFormatFromHand();
                break;
            case CardSet.RingsidePile:
                cardStrings = CurrentPlayer.GetCardsInStringFormatFromRingside();
                break;
            case CardSet.OpponentsRingArea:
                cardStrings = NotCurrentPlayer.GetCardsInStringFormatFromRingArea();
                break;
            case CardSet.OpponentsRingsidePile:
                cardStrings = NotCurrentPlayer.GetCardsInStringFormatFromRingside();
                break;
        }
        
        return cardStrings;
    }
    
    private void ShowPlayersInfo()
    {
        _view.ShowGameInfo(GeneratePlayerInfo(CurrentPlayer), GeneratePlayerInfo(NotCurrentPlayer));
    }

    private PlayerInfo GeneratePlayerInfo(Player player)
    {
        return new PlayerInfo(player.GetSuperStarName(), player.Fortitude, player.GetHandSize(),
                player.GetArsenalSize());
    }

    private void AplyInitialAbilities()
    {
        foreach (Player player in _playersList)
        {
            player.ExecuteInitialAbility();
        }
    }
    
    private static void SwapPlayers<TPlayer>(List<TPlayer> playersList)
    {
        (playersList[0], playersList[1]) = (playersList[1], playersList[0]);
    }

}