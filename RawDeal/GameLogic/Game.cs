using RawDeal.Cards;
using RawDeal.Cards.CardEffects.ActionEffects;
using RawDeal.Cards.CardEffects.ReversalsEffects;
using RawDeal.JsonReader;
using RawDeal.SuperStars;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.GameLogic;

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
    private List<DeckValidator> _selectedDecks;
    private Player CurrentPlayer => _playersList[_indexCurrentPlayer];
    private Player NotCurrentPlayer => _playersList[_indexNotCurrentPlayer];
    private Play _currentPlay;
    private TupleManager _tupleManager;
    private CardMobilizer _cardMobilizer;


    public Game(View view, string deckFolder)
    {
        _view = view;
        AllCardsList = CardsJsonReader.GenerateAllCardsListFromCardsFromJson();
        SetViewObjectInCards(_view);
        AllSuperStarList = SuperstarsJsonReader.GenerateAllSuperStarsListFromJson();
        _deckFolder = deckFolder;
        _gameIsOver = false;
        _indexCurrentPlayer = 0;
        _indexNotCurrentPlayer = 1;
        _selectedDecks = new List<DeckValidator>();
        _tupleManager = new TupleManager();
        _cardMobilizer = new CardMobilizer();
    }

    public void Play()
    {
        AskUsersToSelectDecks();
        if (AreDecksValid())
        {
            CreatePlayers(_selectedDecks);
            OrderPlayersBySuperStarValue();
            ApplyInitialAbilities();
            RunGameLoop();
        }
    }
    
    private void RunGameLoop()
    {
        while (!_gameIsOver)
        {
            if (NotCurrentPlayer.HasCeroCardsInArsenal()) EndGame(winnerPlayer: CurrentPlayer);
            if (!_gameIsOver) PlayTurn();
            UpdatePlayersIndex();
        }
    }
    
    private void EndGame(Player winnerPlayer)
    {
        _gameIsOver = true;
        _view.CongratulateWinner(winnerPlayer.GetSuperStarName());
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
        while (!CurrentPlayer.EndsHisTurn && !_gameIsOver)
        {
            ExecuteAutomaticAbilities();
            ShowPlayersInfo();
            NextPlay nextPlay = AskUserNextPlay();
            ExecuteNextPlay(nextPlay);
            UpdatePlayersFortitude();
            CheckForGameOver();

        }
    }

    private void UpdatePlayersFortitude()
    {
        foreach (Player player in _playersList)
        {
            player.UpdateFortitude();
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
                _currentPlay = new Play(GetDictionaryOfCurrentAndNotCurrentPlayer());
                PlayCard();
                break;
            case NextPlay.UseAbility:
                CurrentPlayer.UseSuperStarAbility(NotCurrentPlayer);
                break;
            case NextPlay.EndTurn:
                CurrentPlayer.EndsHisTurn = true;
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
        if (selectedCardIndex == -1) {
            return;
        }

        (int, Card) tupleWithIndexInHandAndAttackingCard = _listOfTuplesOfPlayableCards[selectedCardIndex];
        Card attackingCard = _tupleManager.ExtractCard(tupleWithIndexInHandAndAttackingCard);
        attackingCard.PlayedType = _typesOfPlayableCards[selectedCardIndex];
        _currentPlay.SetAttackingCardTuple(tupleWithIndexInHandAndAttackingCard);
        SayPlayerIsTryingToPlayCard(attackingCard);

        List<(int, Card)> validReversals = NotCurrentPlayer.GetReversalTuplesFromHand(attackingCard);
        if (validReversals.Count > 0) 
        {
            HandleReversals(validReversals, attackingCard);
        }
        
        switch (attackingCard.PlayedType)
        {
            case "Maneuver":
                PlayManeuver(attackingCard, tupleWithIndexInHandAndAttackingCard);
                break;
            case "Action":
                if (attackingCard.Title == "Jockeying for Position")
                {
                    JockeyingForPositionActionEffect actionEffect = new JockeyingForPositionActionEffect(_view);
                    actionEffect.ApplyEffect(_currentPlay);
                } 
                else
                {
                    SimpleAction simpleAction = new SimpleAction(_view);
                    simpleAction.ApplyEffect(_currentPlay);
                }
                break;
        }
    }

    private void HandleReversals(List<(int, Card)> validReversals, Card attackingCard) 
    {
        List<string> validReversalsInString = GetFormattedReversalCards(validReversals);
        int selectedReversalIndex = _view.AskUserToSelectAReversal(NotCurrentPlayer.GetSuperStarName(), validReversalsInString);
        if (selectedReversalIndex == -1) {
            return;
        }

        attackingCard.PlayedType = "Reversed";
        (int, Card) tupleWithIndexInHandAndReverseCard = validReversals[selectedReversalIndex];
        _currentPlay.SetReversalCardTuple(tupleWithIndexInHandAndReverseCard);
        Card selectedReversalCard = _tupleManager.ExtractCard(tupleWithIndexInHandAndReverseCard);
        selectedReversalCard.SetReversalTypeAndSubtype();
        
        HandleReversalEffects(selectedReversalCard);
    }

    private void HandleReversalEffects(Card selectedReversalCard)
    {
        switch (selectedReversalCard.ReversalType)
        {
            case "ReversalStrike":
            case "ReversalGrapple":
            case "ReversalSubmission":
            case "ReversalAction":
                ReversalSimple reversalEffect = new ReversalSimple(_view);
                reversalEffect.ApplyEffect(_currentPlay);
                break;
            case "ReversalGrappleSpecial":
            case "ReversalStrikeSpecial":
                ReversalWithMaximumDamage reversalSpecial = new ReversalWithMaximumDamage(_view);
                const int maximumDamageReversalSpecial = 7;
                reversalSpecial.SetMaximumDamageThatCanReverse(maximumDamageReversalSpecial);
                reversalSpecial.ApplyEffect(_currentPlay);
                break;
            case "ReversalSpecial":
                HandleSpecialReversal(selectedReversalCard);
                break;
        }

        if (selectedReversalCard.ReversalType != "ReversalSpecial")
        {
            ApplyCardDamage(NotCurrentPlayer, CurrentPlayer, selectedReversalCard);
        }
    }
    private void HandleSpecialReversal(Card selectedReversalCard) 
    {
        switch (selectedReversalCard.Title) 
        {
            case "Elbow to the Face":
                ReversalWithMaximumDamage reversalElbowToTheFace = new ReversalWithMaximumDamage(_view);
                const int maximumDamageReversalElbowToTheFace = 7;
                reversalElbowToTheFace.SetMaximumDamageThatCanReverse(maximumDamageReversalElbowToTheFace);
                reversalElbowToTheFace.ApplyEffect(_currentPlay);
                ApplyCardDamage(NotCurrentPlayer, CurrentPlayer, selectedReversalCard);
                break;
            case "Manager Interferes":
                ReversalSimple managerInterferes = new ReversalSimple(_view);
                managerInterferes.ApplyEffect(_currentPlay);
                const int numberOfCardsToDrawByManagerInterferes = 1;
                _cardMobilizer.MoveCardsFromArsenalToHand(NotCurrentPlayer, numberOfCardsToDrawByManagerInterferes);
                ApplyCardDamage(NotCurrentPlayer, CurrentPlayer, selectedReversalCard);
                break;
            case "Chyna Interferes":
                ReversalSimple chynaInterferes = new ReversalSimple(_view);
                chynaInterferes.ApplyEffect(_currentPlay);
                const int numberOfCardsToDrawByChynaInterferes = 2;
                _cardMobilizer.MoveCardsFromArsenalToHand(NotCurrentPlayer, numberOfCardsToDrawByChynaInterferes);
                ApplyCardDamage(NotCurrentPlayer, CurrentPlayer, selectedReversalCard);
                break;
            case "Clean Break":
                ReversalByTitle cleanBreak = new ReversalByTitle(_view);
                cleanBreak.SetCardTitleThatCanReverse("Jockeying for Position");
                cleanBreak.ApplyEffect(_currentPlay);
                const int numberOfCardsToDrawByCleanBreak = 1;
                _cardMobilizer.MoveCardsFromArsenalToHand(NotCurrentPlayer, numberOfCardsToDrawByCleanBreak);
                ApplyCardDamage(NotCurrentPlayer, CurrentPlayer, selectedReversalCard);
                break;
            case "Jockeying for Position":
                JockeyingForPositionReversalEffect jockeyingForPosition = new JockeyingForPositionReversalEffect(_view);
                jockeyingForPosition.ApplyEffect(_currentPlay);
                break;
        }
    }

    private void PlayManeuver(Card attackingCard, (int, Card) tupleWithIndexInHandAndAttackingCard) 
    {
        _cardMobilizer.MoveCardFromHandToRingArea(CurrentPlayer, tupleWithIndexInHandAndAttackingCard);
        _view.SayThatPlayerSuccessfullyPlayedACard();
        if (NotCurrentPlayer.CalculateDamage(attackingCard.GetCurrentDamage()) > 0) 
        {
            ApplyCardDamage(CurrentPlayer, NotCurrentPlayer, attackingCard);
        }
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
        if (deck.IsValidDeck()) return false;
        _view.SayThatDeckIsInvalid();
        return true;
    }

    private bool AreDecksValid()
    {
        const int correctNumberOfDecks = 2;
        return _selectedDecks.Count == correctNumberOfDecks;
    }

    private void CreatePlayers(List<DeckValidator> selectedDecks)
    {
        foreach (DeckValidator deck in selectedDecks)
        {
            Player player = CreatePlayerFromDeck(deck);
            _playersList.Add(player);    
        }
        
    }
    private Player CreatePlayerFromDeck(DeckValidator deckValidator)
    {
        SuperStar superstar = deckValidator.SuperStarsList.First();
        List<Card> cardsList = deckValidator.CardList;
        Player player = new Player(superstar, cardsList, _view);
        return player;
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
    
    
    private NextPlay AskUserNextPlay()
    {
        bool canUserUseHisAbility = CanUseHisAbility(CurrentPlayer);
        if (canUserUseHisAbility && !CurrentPlayer.IsAbilityAutomatic())
        {
            return _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible();
        }
        return _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
    }
    
    private int AskUserToSelectCard(List<(int, Card)> playableCards)
    {
        List<string> playableCardsFormatted = GetFormattedPlayableCards(playableCards);
        return _view.AskUserToSelectAPlay(playableCardsFormatted);
    }

    private void CheckForGameOver()
    {
        if (NotCurrentPlayer.HasCeroCardsInArsenal() && CurrentPlayer.EndsHisTurn)
        {
            EndGame(winnerPlayer: CurrentPlayer);
        }
    }

    private void ResetPlayerStatusInTurn()
    {
        CurrentPlayer.HasUsedHisAbilityInTheTurn = false;
        CurrentPlayer.EndsHisTurn = false;
    }

    private void ExecuteAutomaticAbilities()
    {
        if (CurrentPlayer.CanUseAbility() && CurrentPlayer.IsAbilityAutomatic())
        {
            CurrentPlayer.UseSuperStarAbility(NotCurrentPlayer);
        }
        if (CurrentPlayer.IsAbilityAutomatic()) CurrentPlayer.HasUsedHisAbilityInTheTurn = true;
        
    }

    private void ApplyCardDamage(Player attackingPlayer, Player damagedPlayer, Card attackingCard)
    {
        int pretendedDamage = CalculateActualDamage(attackingCard);
        if (pretendedDamage == 0) return;
        int actualDamage = 0;
        List<Card> cardsToBeDamaged = damagedPlayer.GetCardsFromArsenal(pretendedDamage);
        cardsToBeDamaged.Reverse();
        List<Card> actualDamagedCards = new List<Card>();
        bool cardWasReversedByDeck = false;
        bool cardWasReversedInLastCardOfDeck = false;
        for (int index = 0; index < cardsToBeDamaged.Count; index++)
        {
            actualDamage++;
            Card damagedCard = cardsToBeDamaged[index];
            actualDamagedCards.Add(damagedCard);
            List<Card> possibleReversals = damagedPlayer.GetReversalFromArsenal(attackingCard);
            if (possibleReversals.Contains(damagedCard))
            {
                cardWasReversedByDeck = true;
                attackingPlayer.EndsHisTurn = true;
                if (index == cardsToBeDamaged.Count - 1) cardWasReversedInLastCardOfDeck = true;
                break;
            }
        }
        _view.SayThatOpponentWillTakeSomeDamage(damagedPlayer.GetSuperStarName(), pretendedDamage);
        DamagedCardsDisplayer damagedCardsDisplayer = new DamagedCardsDisplayer(_view);
        damagedCardsDisplayer.ShowDamagedCards(actualDamagedCards, pretendedDamage);
        
        bool opponentHasRunOutOfCards = false;
        if (cardWasReversedByDeck)
        {
            _view.SayThatCardWasReversedByDeck(damagedPlayer.GetSuperStarName());
            if (attackingCard.GetStunValue() > 0 && !cardWasReversedInLastCardOfDeck)
            {
                int numberOfCardsToDraw = _view.AskHowManyCardsToDrawBecauseOfStunValue(attackingPlayer.GetSuperStarName(),
                    attackingCard.GetStunValue());
                for (int i = 0; i < numberOfCardsToDraw; i++)
                {
                    attackingPlayer.MoveCardFromArsenalToHand();
                }

                if (numberOfCardsToDraw > 0)
                {
                    _view.SayThatPlayerDrawCards(attackingPlayer.GetSuperStarName(), numberOfCardsToDraw);
                    
                }
            }
        }
        else
        {
            opponentHasRunOutOfCards = OpponentLostDuringDamage(pretendedDamage);
        }
        damagedPlayer.ReceiveDamageWithoutView(actualDamage);

        if (opponentHasRunOutOfCards)
        {
            EndGame(attackingPlayer);
        }
    }

    private bool OpponentLostDuringDamage(int actualDamage)
    {
        int maximumDamage = NotCurrentPlayer.GetArsenalSize();
        return actualDamage > maximumDamage;
    }

    private int CalculateActualDamage(Card selectedCard)
    {
        return NotCurrentPlayer.CalculateDamage(selectedCard.GetCurrentDamage());
    }
    

    private void SayPlayerIsTryingToPlayCard(Card selectedCard)
    {
        string superStarName = CurrentPlayer.GetSuperStarName();
        string cardInPlayFormat = selectedCard.GetCardInPlayFormat(selectedCard.PlayedType);
        _view.SayThatPlayerIsTryingToPlayThisCard(superStarName, cardInPlayFormat);
    }

    private List<string> _typesOfPlayableCards = new();
    private List<(int, Card)> _listOfTuplesOfPlayableCards = new();
    
    private List<string> GetFormattedPlayableCards(List<(int, Card)> playableCardsTuples)
    {
        _typesOfPlayableCards = new List<string>();
        _listOfTuplesOfPlayableCards = new List<(int, Card)>();
        List<string> formattedPlayableCards = new List<string>();
        foreach (var tupleIndexInHandAndCard in playableCardsTuples)
        {
            Card currentCard = _tupleManager.ExtractCard(tupleIndexInHandAndCard);
            if (currentCard.IsHybrid)
            {
                for (int j = 0; j < currentCard.AmountOfTypes; j++)
                {
                    currentCard.PlayedType = currentCard.Types[j];
                    if (currentCard.CurrentPlayedTypeIsPlayable())
                    {
                        formattedPlayableCards.Add(GetCardInPlayFormat(tupleIndexInHandAndCard));
                        _typesOfPlayableCards.Add(currentCard.PlayedType);
                        _listOfTuplesOfPlayableCards.Add(tupleIndexInHandAndCard);
                    }
                }
            }
            else
            {
                currentCard.PlayedType = currentCard.Types[0];
                formattedPlayableCards.Add(GetCardInPlayFormat(tupleIndexInHandAndCard));
                _typesOfPlayableCards.Add(currentCard.PlayedType);
                _listOfTuplesOfPlayableCards.Add(tupleIndexInHandAndCard);
            }
        }
        
        return formattedPlayableCards;
    }
    
    private List<string> GetFormattedReversalCards(List<(int, Card)> reversalCardsTuples)
    {
        List<string> formattedReversalCards = new List<string>();
        foreach (var tupleIndexInHandAndCard in reversalCardsTuples)
        {
            Card currentCard = _tupleManager.ExtractCard(tupleIndexInHandAndCard);
            currentCard.PlayedType = "Reversal";
            formattedReversalCards.Add(GetCardInPlayFormat(tupleIndexInHandAndCard));
            
        }
        
        return formattedReversalCards;
    }
    
    private string GetCardInPlayFormat((int, Card) tuple)
    {
        Card card = _tupleManager.ExtractCard(tuple);
        return card.GetCardInPlayFormat(card.PlayedType);
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

    private void ApplyInitialAbilities()
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

    private void SetViewObjectInCards(View view)
    {
        foreach (Card card in AllCardsList)
        {
            card.SetViewObject(view);
        }
    }

    private Dictionary<string, Player> GetDictionaryOfCurrentAndNotCurrentPlayer()
    {
        Dictionary<string, Player> players = new Dictionary<string, Player>
        {
            { "CurrentPlayer", CurrentPlayer },
            { "NotCurrentPlayer", NotCurrentPlayer }
        };

        return players;
    }
    
}