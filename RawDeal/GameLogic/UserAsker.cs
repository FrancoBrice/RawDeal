using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.GameLogic;

public class UserAsker
{
    private readonly PlayableCardsFormatter _playableCardsFormatter;
    private readonly View _view;
    public List<IndexedCard> ListOfIndexedCardsOfPlayableCards;
    public List<string> TypesOfPlayableCards;

    public UserAsker(View view)
    {
        _playableCardsFormatter = new PlayableCardsFormatter();
        _view = view;
    }

    public static NextPlay GetNextPlay(View view, Player player)
    {
        bool canUserUseHisAbility = player.CanUseHisAbility();
        if (canUserUseHisAbility && !player.IsAbilityAutomatic())
            return view.AskUserWhatToDoWhenUsingHisAbilityIsPossible();
        return view.AskUserWhatToDoWhenHeCannotUseHisAbility();
    }

    public int SelectACard(PlayManager playManager)
    {
        Player currentPlayer = playManager.GetCurrentPlay().CurrentPlayer;
        List<IndexedCard> playableCards = currentPlayer.GetPlayableCardsFromPlayer(playManager);
        List<string> playableCardsFormatted =
            _playableCardsFormatter.GetPlayableCards(playManager, playableCards, 
                currentPlayer.Fortitude);
        TypesOfPlayableCards = _playableCardsFormatter.TypesOfPlayableCards;
        ListOfIndexedCardsOfPlayableCards = _playableCardsFormatter.ListOfIndexedCardsOfPlayableCards;
        return _view.AskUserToSelectAPlay(playableCardsFormatted);
    }
}