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
    private List<DeckValidator> _selectedDecks;
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
        _selectedDecks = new List<DeckValidator>();
    }

    public void Play()
    {
        AskUsersToSelectDecks();
        if (AreDecksValid())
        {
            CreatePlayers(_selectedDecks);
            OrderPlayersBySuperStarValue();
            AplyInitialAbilities();
            RunGameLoop();
        }
    }
    
    private void RunGameLoop()
    {
        while (!_gameIsOver)
        {
            if (NotCurrentPlayer.PlayerHasLost()) EndGame(winnerPlayer: CurrentPlayer);
            if (!_gameIsOver) PlayTurn();
            UpdatePlayersIndex();
        }
    }
    
    private void EndGame(Player winnerPlayer)
    {
        _gameIsOver = true;
        _view.CongratulateWinner(winnerPlayer.GetSuperStarName());
    }

    private void AskUsersToSelectDecks()
    {
        for (int i = 0; i < 2; i++)
        {
            string deckPath = _view.AskUserToSelectDeck(_deckFolder);
            DeckValidator deck = GetDeckFromPath(deckPath);
            if (IsInvalidDeck(deck)) return;
            _selectedDecks.Add(deck);
        }
    }
    
    private bool IsInvalidDeck(DeckValidator deck)
    {
        if (!deck.IsValidDeck())
        {
            _view.SayThatDeckIsInvalid();
            return true;
        }
        return false;
    }

    private bool AreDecksValid()
    {
        if (_selectedDecks.Count() == 2) return true;
        return false;
    }

    private void CreatePlayers(List<DeckValidator> selectedDecks)
    {
        foreach (DeckValidator deck in selectedDecks)
        {
            Player player = CreatePlayerFromDeck(deck);
            _playersList.Add(player);    
        }
        
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
            cardsList.Add(card);
        }
        return cardsList;
    }

    private List<SuperStar> GetSuperStarsListFromDeck(string path)
    {
        var superStarStringsList = File.ReadAllLines(path).Where(line => line.Contains("(Superstar Card)"));
        List<SuperStar> superStarsList = new List<SuperStar>();

        foreach (var superstarString in superStarStringsList)
        {
            string cardName = superstarString.Replace(" (Superstar Card)", "");
            var superstar = AllSuperStarList.FirstOrDefault(superstar => superstar.Name == cardName);
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
        bool canUserUseHisAbility = CanUseHisAbility(CurrentPlayer);
        if (canUserUseHisAbility && !CurrentPlayer.IsAbilityAutomatic())
        {
            return _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible();
        }
        return _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
    }

    private void CheckForGameOver()
    {
        if (NotCurrentPlayer.PlayerHasLost())
        {
            EndGame(winnerPlayer: CurrentPlayer);
        }
    }

    private void ResetPlayerStatusInTurn()
    {
        CurrentPlayer.HasUsedHisAbilityInTheTurn = false;
        _playerEndsHisTurn = false;
    }

    private void ExecuteAutomaticAbilities()
    {
        if (CurrentPlayer.CanUseAbility() && CurrentPlayer.IsAbilityAutomatic())
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
                ShowCardsBasedOnSelection();
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
                EndGame(winnerPlayer: NotCurrentPlayer);
                break;
        }
    }
    
    private void PlayCard()
    {
        List<(int, Card)> playableCards = CurrentPlayer.GetPlayableCardsFromPlayer();
        int selectedCardIndex = AskUserToSelectCard(playableCards);
        if (selectedCardIndex == -1) return;
        Card selectedCard = ExtractCardFromTuple(playableCards[selectedCardIndex]);
        int indexInHand = ExtractCardIndexInHandFromTuple(playableCards[selectedCardIndex]);
        int actualDamage = CalculateActualDamage(selectedCard);
        List<Card> damagedCards = NotCurrentPlayer.GetCardsFromArsenal(actualDamage);
        SayPlayerIsTryingToPlayCard(selectedCard);
        MoveCardFromHandToRingArea(indexInHand, selectedCard);
        SayPlayerSuccessfullyPlayedCard();
        NotCurrentPlayer.ReceiveDamage(actualDamage);
        ShowDamagedCards(damagedCards, actualDamage);
    }

    private int AskUserToSelectCard(List<(int, Card)> playableCards)
    {
        List<string> playableCardsFormatted = GetFormattedPlayableCards(playableCards);
        return _view.AskUserToSelectAPlay(playableCardsFormatted);
    }

    private int CalculateActualDamage(Card selectedCard)
    {
        return NotCurrentPlayer.CalculateDamage(selectedCard.GetDamage());
    }

    private void ShowDamagedCards(List<Card> damagedCards, int actualDamage)
    {
        int indexShowedCard = 1;
        damagedCards.Reverse();
        foreach (Card damagedCard in damagedCards)
        {
            ShowCardOverturnByTakingDamage(damagedCard, indexShowedCard, actualDamage);
            indexShowedCard++;
        }
    }

    private void SayPlayerIsTryingToPlayCard(Card selectedCard)
    {
        string superStarName = CurrentPlayer.GetSuperStarName();
        string cardInPlayFormat = selectedCard.GetCardInPlayFormat();
        _view.SayThatPlayerIsTryingToPlayThisCard(superStarName, cardInPlayFormat);
    }

    private void MoveCardFromHandToRingArea(int cardIndexInHand, Card selectedCard)
    {
        CurrentPlayer.MoveCardFromHandToRingAreaByIndex(cardIndexInHand);
    }

    private void SayPlayerSuccessfullyPlayedCard()
    {
        _view.SayThatPlayerSuccessfullyPlayedACard();
    }

    private List<string> GetFormattedPlayableCards(List<(int, Card)> playableCards)
    {
        return playableCards.Select(GetCardInPlayFormat).ToList();
    }

    private string GetCardInPlayFormat((int, Card) tuple)
    {
        Card card = ExtractCardFromTuple(tuple);
        return card.GetCardInPlayFormat();
    }

    private Card ExtractCardFromTuple((int, Card) tuple)
    {
        return tuple.Item2;
    }
    private int ExtractCardIndexInHandFromTuple((int, Card) tuple)
    {
        return tuple.Item1;
    }

    private void ShowCardOverturnByTakingDamage(Card damagedCard, int indexShowedCard, int actualDamage)
    {
        string cardFormattedInfo = damagedCard.GetCardFormattedInfo();
        _view.ShowCardOverturnByTakingDamage(cardFormattedInfo, indexShowedCard, actualDamage);
}

    private bool CanUseHisAbility(Player currentPlayer)
    {
        return currentPlayer.CanUseAbility();
    }

    private void UpdatePlayersIndex()
    {
        _indexCurrentPlayer = (_indexCurrentPlayer + 1) % _playersList.Count;
        _indexNotCurrentPlayer = (_indexNotCurrentPlayer + 1) % _playersList.Count;
    }

    private void OrderPlayersBySuperStarValue()
    {
        if (_playersList[1].SuperStar.SuperstarValue > _playersList[0].SuperStar.SuperstarValue)
        {
            ExchangePlayersPositions(_playersList);
        }
    }
    
    private void ShowCardsBasedOnSelection()
    {

        CardSet setOfCardsSelected = _view.AskUserWhatSetOfCardsHeWantsToSee();
        List<string> cardstringsList = GetCardsAsStringListFromSelectedSet(setOfCardsSelected);
        _view.ShowCards(cardstringsList);
    }
    

    private List<string> GetCardsAsStringListFromSelectedSet(CardSet setOfCardsSelected)
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
    
    private static void ExchangePlayersPositions<TPlayer>(List<TPlayer> playersList)
    {
        (playersList[0], playersList[1]) = (playersList[1], playersList[0]);
    }

}