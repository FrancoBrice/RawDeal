using RawDeal.Cards;
using RawDeal.Tools;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.GameLogic;

public class UserAsker
{
    public List<string> TypesOfPlayableCards;
    public List<(int, Card)> ListOfTuplesOfPlayableCards;
    private PlayableCardsFormatter _playableCardsFormatter;
    private View _view;

    public UserAsker(View view)
    {
        _playableCardsFormatter = new PlayableCardsFormatter();
        _view = view;
    }

    public NextPlay GetNextPlay(Player player)
    {
        bool canUserUseHisAbility = player.CanUseHisAbility();
        if (canUserUseHisAbility && !player.IsAbilityAutomatic())
        {
            return _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible();
        }
        return _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
    }

    public int SelectACard(PlayManager playManager)
    {
        Player currentPlayer = playManager.CurrentPlay.CurrentPlayer;
        List<(int, Card)> playableCards = currentPlayer.GetPlayableCardsFromPlayer(playManager);
        List<string> playableCardsFormatted = _playableCardsFormatter.GetPlayableCards(playableCards, currentPlayer.Fortitude);
        TypesOfPlayableCards = _playableCardsFormatter.TypesOfPlayableCards;
        ListOfTuplesOfPlayableCards = _playableCardsFormatter.ListOfTuplesOfPlayableCards;
        return _view.AskUserToSelectAPlay(playableCardsFormatted);
    }
    
    
}
