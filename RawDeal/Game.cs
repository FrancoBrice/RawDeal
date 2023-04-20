using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RawDealView;

namespace RawDeal;

public class Game
{
    private View _view;
    private string _deckFolder;
    private List<Card> AllCardsList { get; set; }
    private List<SuperStar> AllSuperStarList { get; set; }
    private List<string> SuperStarLogosList = new List<string>();
    private List<Player> PlayersList = new List<Player>();
    private bool _gameIsOver;


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
        GenerateSuperStarLogosList();
    }


    private bool CheckIfThereIsInvalidDecks()
    {
        while (PlayersList.Count() < 2)
        {
            string pathDeckSelected = _view.AskUserToSelectDeck(_deckFolder);
            List<Card> deckCardList = new List<Card>();
            List<SuperStar> deckSuperStarList = new List<SuperStar>();
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
    
    private void PlayTurn(Player currentPlayer, Player notCurrentPlayer)
    {

        _view.SayThatATurnBegins(currentPlayer.SuperStar.Name);

        currentPlayer.MoveCardFromArsenalToHand();
        currentPlayer.HasUsedHisAbilityInTheTurn = false;

        
        bool playerEndedTurn = false;
        while (!playerEndedTurn && !_gameIsOver)
        {
            bool canUserUseHisAbility = CheckIfPlayerCanUseHisAbility(currentPlayer);
            bool abilityIsAutomatic = currentPlayer.CheckIfAbilityIsAutomatic();
            NextPlay nextPlay;
            
            if (canUserUseHisAbility && abilityIsAutomatic)
            {
                currentPlayer.UseSuperStarAbility(notCurrentPlayer);
                currentPlayer.HasUsedHisAbilityInTheTurn = true;
            }
            ShowPlayersInfo(currentPlayer, notCurrentPlayer);
            
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
                ShowCardsBasedOnSelection(currentPlayer);
            }
            
            else if (nextPlay == NextPlay.PlayCard)
            {
                List<(int, Card)> playableCardsFromPlayer = currentPlayer.GetPlayableCardsFromPlayer();
                List<string> availablePlays = new List<string>();
                
                
                foreach (var tuple in playableCardsFromPlayer)
                {
                    Card card = tuple.Item2;
                    string formattedPlay = card.GetCardInFormattedPlay();
                    availablePlays.Add(formattedPlay);
                }
                int cardIndex = _view.AskUserToSelectAPlay(availablePlays);
                
                if (cardIndex != -1)
                {
                    Card selectedCard = playableCardsFromPlayer[cardIndex].Item2;
                    string formattedPlay = selectedCard.GetCardInFormattedPlay();
                    
                    _view.SayThatPlayerIsTryingToPlayThisCard(currentPlayer.GetSuperStarName(), formattedPlay);
                    currentPlayer.MoveCardFromHandToRingAreaByIndex(playableCardsFromPlayer[cardIndex].Item1);
                    _view.SayThatPlayerSuccessfullyPlayedACard();
                    int actualDamage = notCurrentPlayer.CalculateDamage(selectedCard.GetDamage());
                    List<Card> notCurrentPlayerDamagedCards = notCurrentPlayer.GetCardsFromArsenal(actualDamage);
                    notCurrentPlayer.ReceiveDamage(actualDamage);

                    int indexShowedCard = 1;
                    for (cardIndex = notCurrentPlayerDamagedCards.Count - 1; cardIndex >= 0; cardIndex -- )
                    {
                        _view.ShowCardOverturnByTakingDamage(notCurrentPlayerDamagedCards[cardIndex].GetCardFormattedInfo(), indexShowedCard , actualDamage);
                        indexShowedCard++;
                    }



                }
                
            }
            
            else if (nextPlay == NextPlay.UseAbility)
            {
                currentPlayer.UseSuperStarAbility(notCurrentPlayer);
                currentPlayer.HasUsedHisAbilityInTheTurn = true;
            }
            
            else if (nextPlay == NextPlay.EndTurn)
            {
                playerEndedTurn = true;
            }
            else if (nextPlay == NextPlay.GiveUp)
            {
                EndGame(notCurrentPlayer);
            }
            currentPlayer.UpdateFortitude();
            
            bool notCurrentPlayerHasLose = notCurrentPlayer.CheckIfPlayerLose();
            if (notCurrentPlayerHasLose)
            {
                EndGame(currentPlayer);
            }

        }
        
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
            int indexCurrentPlayer = 0;
            int indexNotCurrentPlayer = 1;

            while (!_gameIsOver)
            {
                Player currentPlayer = PlayersList[indexCurrentPlayer];
                Player notCurrentPlayer = PlayersList[indexNotCurrentPlayer];

                bool notCurrentPlayerHasLose =  notCurrentPlayer.CheckIfPlayerLose();
                if (notCurrentPlayerHasLose)
                {
                    EndGame(currentPlayer);
                }

                if (!_gameIsOver)
                {
                    PlayTurn(currentPlayer, notCurrentPlayer);
                }
                

                indexCurrentPlayer = (indexCurrentPlayer + 1) % PlayersList.Count;
                indexNotCurrentPlayer = (indexNotCurrentPlayer + 1) % PlayersList.Count;
            }
        }

    }

    private List<Player> OrderPlayersBySuperStarValue(List<Player> playerList)
    {
        if (playerList[1].SuperStar.SuperstarValue > playerList[0].SuperStar.SuperstarValue)
        {
            SwapPlayers(playerList);
        }

        return playerList;
    }
    

    
    private void ShowCardsBasedOnSelection(Player player)
    {

        CardSet setOfCardsSelected = _view.AskUserWhatSetOfCardsHeWantsToSee();
        Player opponent = PlayersList.FirstOrDefault(p => p != player);

        List<string> cardTitles = new List<string>();

        if (setOfCardsSelected == CardSet.RingArea)
        {
            cardTitles = player.GetCardTitlesFromRingArea();
        }
        else if (setOfCardsSelected == CardSet.Hand)
        {
            cardTitles = player.GetFormattedCardsFromHand();
        }
        else if (setOfCardsSelected == CardSet.RingsidePile)
        {
            cardTitles = player.GetCardTilesFromRingside();
        }
        else if (setOfCardsSelected == CardSet.OpponentsRingArea)
        {
            cardTitles = opponent?.GetCardTitlesFromRingArea();
        }
        else if (setOfCardsSelected == CardSet.OpponentsRingsidePile)
        {
            cardTitles = opponent?.GetCardTilesFromRingside();
        }

        if (cardTitles != null) _view.ShowCards(cardTitles);
    }

    private void ShowPlayersInfo(Player currentPlayer, Player notCurrentPlayer)
    {
        List<Player> playersList = new List<Player>() { currentPlayer, notCurrentPlayer };
        List<PlayerInfo> playerInfoList = new List<PlayerInfo>();
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