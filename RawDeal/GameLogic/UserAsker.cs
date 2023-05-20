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

    public int SelectACard(Player player)
    {
        List<(int, Card)> playableCards = player.GetPlayableCardsFromPlayer();
        List<string> playableCardsFormatted = _playableCardsFormatter.GetPlayableCards(playableCards);
        TypesOfPlayableCards = _playableCardsFormatter.TypesOfPlayableCards;
        ListOfTuplesOfPlayableCards = _playableCardsFormatter.ListOfTuplesOfPlayableCards;
        return _view.AskUserToSelectAPlay(playableCardsFormatted);
    }
    
    
}
