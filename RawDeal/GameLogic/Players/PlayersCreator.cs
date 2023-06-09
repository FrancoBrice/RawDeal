using RawDeal.Cards;
using RawDeal.GameLogic.DecksLogic;
using RawDeal.SuperStars;
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
        Player player = new Player(superstar, cardsList, _view);
        return player;
    }
}