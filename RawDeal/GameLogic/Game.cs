using RawDeal.Cards;
using RawDeal.Cards.CardEffects;
using RawDeal.Cards.CardEffects.ActionEffects;
using RawDeal.Cards.CardEffects.ReversalsEffects;
using RawDeal.JsonReader;
using RawDeal.SuperStars;
using RawDeal.Tools;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.GameLogic;

public class Game 
{
    private readonly View _view;
    private readonly string _deckFolder;
    public List<Card> AllCardsList { get; set; }
    public List<SuperStar> AllSuperStarList { get; set; }
    private List<Player> _playersList = new();
    public bool GameIsOver;
    private int _indexCurrentPlayer;
    private int _indexNotCurrentPlayer;
    public List<DeckValidator> SelectedDecks;
    private Player CurrentPlayer => _playersList[_indexCurrentPlayer];
    private Player NotCurrentPlayer => _playersList[_indexNotCurrentPlayer];
    private Play _currentPlay;
    private PlayManager _playManager;
    private TupleManager _tupleManager;
    private CardMobilizer _cardMobilizer;
    private GameFormatter _gameFormatter;
    private UserAsker _userAsker;
    private ViewManager _viewManager;


    public Game(View view, string deckFolder)
    {
        _view = view;
        _viewManager = new ViewManager(_view);
        AllCardsList = CardsJsonReader.GenerateAllCardsListFromCardsFromJson();
        SetViewObjectInCards(_view);
        AllSuperStarList = SuperstarsJsonReader.GenerateAllSuperStarsListFromJson();
        _deckFolder = deckFolder;
        GameIsOver = false;
        _indexCurrentPlayer = 0;
        _indexNotCurrentPlayer = 1;
        SelectedDecks = new List<DeckValidator>();
        _tupleManager = new TupleManager();
        _cardMobilizer = new CardMobilizer(_view);
        _playManager = new PlayManager(_view);
        _gameFormatter = new GameFormatter();
        _userAsker = new UserAsker(_view);
    }

    public void Play()
    {
        DeckSelector deckSelector= new DeckSelector(this, _view);
        deckSelector.AskUsersToSelectDecks(_deckFolder);
        if (deckSelector.AreDecksValid())
        {
            CreatePlayers(SelectedDecks);
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
            _currentPlay = new Play(GetDictionaryOfCurrentAndNotCurrentPlayer(), _view);
            _playManager.AddPlay(_currentPlay);
            if (!GameIsOver)
            {
                Turn currentTurn = new Turn(_currentPlay, _view);
                currentTurn.PlayTurn(game: this);
            }
            UpdatePlayersIndex();
        }
    }

    public void EndGame(Player winnerPlayer)
    {
        GameIsOver = true;
        _view.CongratulateWinner(winnerPlayer.GetSuperStarName());
    }
    
    public void PlayCard() 
    {
        EffectAsigner effectAssigner = new EffectAsigner(GetDictionaryOfCurrentAndNotCurrentPlayer(), _view);
        int selectedCardIndex = AskUserToSelectCard(CurrentPlayer);
        if (selectedCardIndex == -1) return;
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
                PlayManeuver(_currentPlay);
                break;
            case "Action":
                Effect actionEffect = effectAssigner.AssignActionEffect(attackingCard);
                actionEffect.ApplyEffect(_currentPlay);
                break;
        }
    }

    private void HandleReversals(List<(int, Card)> validReversals, Card attackingCard) 
    {
        List<string> validReversalsInString = GetFormattedReversalCards(validReversals);
        int selectedReversalIndex = _view.AskUserToSelectAReversal(NotCurrentPlayer.GetSuperStarName(), validReversalsInString);
        if (selectedReversalIndex == -1) return;
        attackingCard.PlayedType = "Reversed";
        (int, Card) tupleWithIndexInHandAndReverseCard = validReversals[selectedReversalIndex];
        _currentPlay.SetReversalCardTuple(tupleWithIndexInHandAndReverseCard);
        Card selectedReversalCard = _tupleManager.ExtractCard(tupleWithIndexInHandAndReverseCard);
        selectedReversalCard.PlayedType = "Reversal";
        selectedReversalCard.SetReversalTypeAndSubtype();
        HandleReversalEffects();
        _currentPlay.SwapCurrentAndNotCurrentPlayer();
        ApplyCardDamage(_currentPlay);
    }

    private void HandleReversalEffects()
    {
        EffectAsigner effectAssigner = new EffectAsigner(GetDictionaryOfCurrentAndNotCurrentPlayer(), _view);
        Effect assignedEffect = effectAssigner.AssignReversalEffect(_currentPlay);
        assignedEffect.ApplyEffect(_currentPlay);
    }
    
    private void PlayManeuver(Play currentPlay) 
    {
        _cardMobilizer.MoveCardFromHandToRingArea(currentPlay.CurrentPlayer, currentPlay.AttackingCardTuple);
        _view.SayThatPlayerSuccessfullyPlayedACard();
        if (NotCurrentPlayer.CalculateDamage(currentPlay.AttackingCard) > 0) 
        {
            ApplyCardDamage(_currentPlay);
        }
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

    private int AskUserToSelectCard(Player player)
    {
        List<(int, Card)> playableCards = player.GetPlayableCardsFromPlayer();
        List<string> playableCardsFormatted = GetFormattedPlayableCards(playableCards);
        return _view.AskUserToSelectAPlay(playableCardsFormatted);
    }

    public void CheckForGameOver()
    {
        if (NotCurrentPlayer.HasZeroCardsInArsenal() && CurrentPlayer.HasEndsHisTurn )
        {
            EndGame(winnerPlayer: CurrentPlayer);
        }
    }

    public void MakePlayManagerApplyPendingEffects()
    {
        _playManager.ApplyPendingEffectsIfPossible();

    }
    

    private void ApplyCardDamage(Play currentPlay)
    {
        Player attackingPlayer = currentPlay.CurrentPlayer;
        Player damagedPlayer = currentPlay.NotCurrentPlayer;
        Card attackingCard = currentPlay.GetLastCard();
        int pretendedDamage = damagedPlayer.CalculateDamage(attackingCard);
        if (pretendedDamage == 0) return;
        int actualDamage = 0;
        List<Card> cardsToBeDamaged = damagedPlayer.GetCardsFromArsenal(pretendedDamage);
        List<Card> actualDamagedCards = new List<Card>();
        bool cardWasReversedByDeck = false;
        bool cardWasReversedInLastCardOfDeck = false;
        for (int index = cardsToBeDamaged.Count - 1; index >= 0; index--)
        {
            actualDamage++;
            Card damagedCard = cardsToBeDamaged[index];
            actualDamagedCards.Add(damagedCard);
            List<Card> possibleReversals = damagedPlayer.GetReversalsFromArsenal(attackingCard);
            if (possibleReversals.Contains(damagedCard) && damagedPlayer.IsCorrectReversalCard(attackingCard, damagedCard))
            {
                cardWasReversedByDeck = true;
                attackingPlayer.HasEndsHisTurn = true;
                if (actualDamage == pretendedDamage) cardWasReversedInLastCardOfDeck = true;
                break;
            }
        }
        _view.SayThatOpponentWillTakeSomeDamage(damagedPlayer.GetSuperStarName(), pretendedDamage);
        ViewManager viewManager = new ViewManager(_view);
        viewManager.ShowDamagedCards(actualDamagedCards, pretendedDamage);
        if (cardWasReversedByDeck)
        {
            _view.SayThatCardWasReversedByDeck(damagedPlayer.GetSuperStarName());
            HandleStunValue(attackingPlayer, attackingCard, cardWasReversedInLastCardOfDeck);
        }
        bool opponentHasRunOutOfCards = OpponentLostDuringDamage(damagedPlayer, pretendedDamage);
        damagedPlayer.ReceiveDamageWithoutView(actualDamage);
        if (opponentHasRunOutOfCards && !cardWasReversedByDeck)
        {
            EndGame(attackingPlayer);
        }
        _playManager.RemoveEffectsOnCards();
        attackingCard.SetDefaultValues();
    }

    private void HandleStunValue(Player attackingPlayer, Card attackingCard, bool cardWasReversedInLastCardOfDeck)
    {
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

    private bool OpponentLostDuringDamage(Player opponentPlayer, int damage)
    {
        int maximumDamage = opponentPlayer.GetArsenalSize();
        return damage > maximumDamage;
    }

    private void SayPlayerIsTryingToPlayCard(Card selectedCard)
    {
        string superStarName = CurrentPlayer.GetSuperStarName();
        string cardInPlayFormat = selectedCard.GetCardInPlayFormat(selectedCard.PlayedType);
        _view.SayThatPlayerIsTryingToPlayThisCard(superStarName, cardInPlayFormat);
    }

    private List<string> _typesOfPlayableCards;
    private List<(int, Card)> _listOfTuplesOfPlayableCards;
    
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
                GetFormattedCardIfIsHybrid(currentCard, formattedPlayableCards, tupleIndexInHandAndCard);
            }
            else
            {
                GetFormattedPlayableCard(currentCard, formattedPlayableCards, tupleIndexInHandAndCard);
            }
        }
        return formattedPlayableCards;
    }

    private void GetFormattedCardIfIsHybrid(Card currentCard, List<string> formattedPlayableCards,
        (int, Card) tupleIndexInHandAndCard)
    {
        for (int j = 0; j < currentCard.AmountOfTypes; j++)
        {
            currentCard.PlayedType = currentCard.Types[j];
            if (currentCard.CurrentPlayedTypeIsPlayable())
            {
                formattedPlayableCards.Add(GetCardInPlayFormat(tupleIndexInHandAndCard, currentCard.PlayedType));
                _typesOfPlayableCards.Add(currentCard.PlayedType);
                _listOfTuplesOfPlayableCards.Add(tupleIndexInHandAndCard);
            }
        }
    }
    
    private void GetFormattedPlayableCard(Card currentCard, List<string> formattedPlayableCards, (int, Card) tupleIndexInHandAndCard)
    {
        currentCard.PlayedType = currentCard.Types[0];
        formattedPlayableCards.Add(GetCardInPlayFormat(tupleIndexInHandAndCard, currentCard.PlayedType));
        _typesOfPlayableCards.Add(currentCard.PlayedType);
        _listOfTuplesOfPlayableCards.Add(tupleIndexInHandAndCard);
    }

    private List<string> GetFormattedReversalCards(List<(int, Card)> reversalCardsTuples)
    {
        List<string> formattedReversalCards = new List<string>();
        foreach (var tupleIndexInHandAndCard in reversalCardsTuples)
        {
            formattedReversalCards.Add(GetCardInPlayFormat(tupleIndexInHandAndCard, "Reversal"));
        }
        
        return formattedReversalCards;
    }
    
    private string GetCardInPlayFormat((int, Card) tuple, string type)
    {
        Card card = _tupleManager.ExtractCard(tuple);
        return card.GetCardInPlayFormat(type);
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