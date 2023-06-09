using RawDeal.CardCollections;
using RawDeal.Cards;
using RawDeal.GameLogic.DecksLogic;
using RawDeal.SuperStars;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.GameLogic.Players;

public class PlayersCreator
{
    private static Game _game;
    private static View _view;

    public PlayersCreator(Game game, View view)
    {
        _game = game;
        _view = view;
    }

    public void CreatePlayers(List<DeckValidator> selectedDecks)
    {
        foreach (DeckValidator deck in selectedDecks)
        {
            Player player = CreatePlayerFromDeck(deck);
            _game.AddPlayerToPlayersList(player);
        }
    }

    private Player CreatePlayerFromDeck(DeckValidator deckValidator)
    {
        SuperStar superstar = deckValidator.SuperStarsList.First();
        List<Card> cardsList = deckValidator.CardList;
        return CreatePlayer(superstar, cardsList);
    }

    private static Player CreatePlayer(SuperStar superstar, List<Card> cardList)
    {
        Player player = CreateDefaultPlayer(superstar, cardList);
        InitializeArsenal(cardList, player);
        CardMobilizer.DistributeOpeningHand(player, superstar.HandSize);
        AddViewToSuperStar(superstar);
        return player;
    }

    private static Player CreateDefaultPlayer(SuperStar superstar, List<Card> cardList)
    {
        return new Player
        {
            SuperStar = superstar,
            Fortitude = 0,
            Ringside = new Ringside(),
            RingArea = new RingArea(),
            Arsenal = new Arsenal(),
            Hand = new Hand(),
            AllCardsList = cardList,
            DamagesReceived = new List<int>(),
            TuplesWithPlayIdAndPlayedCards = new List<(int, Card)>(),
            HasUsedHisAbilityInTheTurn = false,
            HasEndsHisTurn = false,
            DamageReducedByShield = 0
        };
    }
    
    private static void InitializeArsenal(List<Card> deck, Player player)
    {
        foreach (Card card in deck) player.Arsenal.AddCard(card);
    }
    
    private static void AddViewToSuperStar(SuperStar superstar)
    {
        superstar.AddView(_view);
    }

}