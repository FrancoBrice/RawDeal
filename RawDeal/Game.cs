using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RawDealView;

namespace RawDeal;

public class Game
{
    private readonly View _view;
    private readonly string _deckFolder;
    private List<Card> AllCardsList { get; set; }
    private List<SuperStar> AllSuperStarList { get; set; }
    private List<string> SuperStarLogosList = new();
    private List<Player> PlayersList = new();
    private bool _gameIsOver;
    private int _indexCurrentPlayer;
    private int _indexNotCurrentPlayer;
    private Player _currentPlayer => PlayersList[_indexCurrentPlayer];
    private Player _notCurrentPlayer => PlayersList[_indexNotCurrentPlayer];


    private void GenerateSuperStarLogosList(){
        foreach (SuperStar superstar in AllSuperStarList)
        {
            SuperStarLogosList.Add(superstar.Logo);
        }
    }
    
    public Game(View view, string deckFolder)
    {
        JsonReader jsonReader = new JsonReader();
        AllCardsList = jsonReader.GenerateAllCardsListFromCardsFromJson();
        AllSuperStarList = jsonReader.GenerateAllSuperStarsListFromJson();
        _view = view;
        _deckFolder = deckFolder;
        _gameIsOver = false;
        _indexCurrentPlayer = 0;
        _indexNotCurrentPlayer = 1;
        GenerateSuperStarLogosList();
    }


    private bool CheckIfThereIsInvalidDecks()
    {
        while (PlayersList.Count() < 2)
        {
            string pathDeckSelected = _view.AskUserToSelectDeck(_deckFolder);
            List<Card> deckCardList = new();
            List<SuperStar> deckSuperStarList = new();
            foreach (string cardString in File.ReadAllLines(pathDeckSelected))
            {
                if (cardString.Contains("(Superstar Card)"))
                {
                    string cardName = cardString.Replace(" (Superstar Card)", "");
                    SuperStar superStar = AllSuperStarList.FirstOrDefault(s => s.Name == cardName);
                    if (superStar != null)
                    {
                        deckSuperStarList.Add(superStar);
                    }
                }
                else
                {
                    Card card = AllCardsList.FirstOrDefault(c => c.Title == cardString);
                    if (card != null)
                    {
                        deckCardList.Add(card);
                    }
                }
            }

            // Validar mazo
            Deck deck = new Deck(deckSuperStarList, deckCardList, SuperStarLogosList);
            bool validDeck = deck.IsValidDeck();
            // Si es válido se crea un Player con las Card y una SuperStar
            if (validDeck)
            {
                SuperStar superstar = deckSuperStarList.First();
                Player player = new Player(deckCardList, superstar, _view);
                PlayersList.Add(player);
            }
            else
            {
                _view.SayThatDeckIsInvalid();
                return true;
            }
        }
        return false;
    }
    
    private void PlayTurn()
    {

        _view.SayThatATurnBegins(_currentPlayer.SuperStar.Name);

        _currentPlayer.MoveCardFromArsenalToHand();
        _currentPlayer.HasUsedHisAbilityInTheTurn = false;

        bool playerEndedTurn = false;
        while (!playerEndedTurn && !_gameIsOver)
        {
            bool canUserUseHisAbility = CheckIfPlayerCanUseHisAbility(_currentPlayer);
            bool abilityIsAutomatic = _currentPlayer.CheckIfAbilityIsAutomatic();
            NextPlay nextPlay;
            
            if (canUserUseHisAbility && abilityIsAutomatic)
            {
                _currentPlayer.UseSuperStarAbility(_notCurrentPlayer);
                _currentPlayer.HasUsedHisAbilityInTheTurn = true;
            }
            ShowPlayersInfo();
            
            if (canUserUseHisAbility && !abilityIsAutomatic)
            {
                nextPlay = _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible();
            }
            else
            {
                nextPlay = _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
                
            }

            if (nextPlay == NextPlay.ShowCards)
            {
                ShowCardsBasedOnSelection(_currentPlayer);
            }
            
            else if (nextPlay == NextPlay.PlayCard)
            {
                PlayCard();
            }

            else if (nextPlay == NextPlay.UseAbility)
            {
                _currentPlayer.UseSuperStarAbility(_notCurrentPlayer);
                _currentPlayer.HasUsedHisAbilityInTheTurn = true;
            }
            
            else if (nextPlay == NextPlay.EndTurn)
            {
                playerEndedTurn = true;
            }
            else if (nextPlay == NextPlay.GiveUp)
            {
                EndGame(_notCurrentPlayer);
            }
            _currentPlayer.UpdateFortitude();
            
            bool notCurrentPlayerHasLose = _notCurrentPlayer.CheckIfPlayerLose();
            if (notCurrentPlayerHasLose)
            {
                EndGame(_currentPlayer);
            }
        }
    }

    private void PlayCard()
    {
        List<(int, Card)> playableCardsFromPlayer = _currentPlayer.GetPlayableCardsFromPlayer();
        List<string> availablePlays = new();
                
        foreach (var tupleIndexInHandCard in playableCardsFromPlayer)
        {
            Card card = ExtractCardFromTuple(tupleIndexInHandCard);
            string cardInPlayFormat = card.GetCardInPlayFormat();
            availablePlays.Add(cardInPlayFormat);
        }
        int cardIndex = _view.AskUserToSelectAPlay(availablePlays);

        if (cardIndex != -1)
        {
            Card selectedCard = playableCardsFromPlayer[cardIndex].Item2;
            string cardInPlayFormat = selectedCard.GetCardInPlayFormat();

            _view.SayThatPlayerIsTryingToPlayThisCard(_currentPlayer.GetSuperStarName(), cardInPlayFormat);
            _currentPlayer.MoveCardFromHandToRingAreaByIndex(playableCardsFromPlayer[cardIndex].Item1);
            _view.SayThatPlayerSuccessfullyPlayedACard();
            int actualDamage = _notCurrentPlayer.CalculateDamage(selectedCard.GetDamage());
            List<Card> notCurrentPlayerDamagedCards = _notCurrentPlayer.GetCardsFromArsenal(actualDamage);
            _notCurrentPlayer.ReceiveDamage(actualDamage);

            int indexShowedCard = 1;
            for (cardIndex = notCurrentPlayerDamagedCards.Count - 1; cardIndex >= 0; cardIndex--)
            {
                _view.ShowCardOverturnByTakingDamage(notCurrentPlayerDamagedCards[cardIndex].GetCardFormattedInfo(),
                    indexShowedCard, actualDamage);
                indexShowedCard++;
            }
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
            PlayersList = OrderPlayersBySuperStarValue(PlayersList);
            AplyInitialAbilities();

            while (!_gameIsOver)
            {
                bool notCurrentPlayerHasLose =  _notCurrentPlayer.CheckIfPlayerLose();
                if (notCurrentPlayerHasLose)
                {
                    EndGame(_currentPlayer);
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
        _indexCurrentPlayer = (_indexCurrentPlayer + 1) % PlayersList.Count;
        _indexNotCurrentPlayer = (_indexNotCurrentPlayer + 1) % PlayersList.Count;
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
        List<string> cardstringsList = GenerateCardsStringsList(player, setOfCardsSelected);

        if (cardstringsList != null) _view.ShowCards(cardstringsList);
    }
    

    private List<string> GenerateCardsStringsList(Player player, CardSet setOfCardsSelected)
    {
        List<string> cardstringsList = new();
        Player opponent = PlayersList.FirstOrDefault(p => p != player);

        if (setOfCardsSelected == CardSet.RingArea)
        {
            cardstringsList = player.GetCardsFromRingAreaInStringFormat();
        }
        else if (setOfCardsSelected == CardSet.Hand)
        {
            cardstringsList = player.GetCardsFromHandInStringFormat();
        }
        else if (setOfCardsSelected == CardSet.RingsidePile)
        {
            cardstringsList = player.GetCardsFromRingsideInStringFormat();
        }
        else if (setOfCardsSelected == CardSet.OpponentsRingArea)
        {
            cardstringsList = opponent.GetCardsFromRingAreaInStringFormat();
        }
        else if (setOfCardsSelected == CardSet.OpponentsRingsidePile)
        {
            cardstringsList = opponent.GetCardsFromRingsideInStringFormat();
        }
        return cardstringsList;
    }

    private void ShowPlayersInfo()
    {
        List<Player> playersList = new() { _currentPlayer, _notCurrentPlayer };
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

    private void AplyInitialAbilities()
    {
        foreach (Player player in PlayersList)
        {
            player.ExecuteInitialAbility();
        }
    }

    private void EndGame(Player winnerPlayer)
    {
        _gameIsOver = true;
        _view.CongratulateWinner(winnerPlayer.GetSuperStarName());
    }
    
    private static void SwapPlayers<TPlayer>(IList<TPlayer> list)
    {
        (list[0], list[1]) = (list[1], list[0]);
    }

}