using RawDeal.Cards;
using RawDeal.Tools;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.GameLogic;

public class UserAsker
{
    private GameFormatter _gameFormatter;
    private View _view;

    public UserAsker(View view)
    {
        _gameFormatter = new GameFormatter();
        _view = view;
    }

    public NextPlay AskUserNextPlay(Player player)
    {
        bool canUserUseHisAbility = player.CanUseHisAbility();
        if (canUserUseHisAbility && !player.IsAbilityAutomatic())
        {
            return _view.AskUserWhatToDoWhenUsingHisAbilityIsPossible();
        }
        return _view.AskUserWhatToDoWhenHeCannotUseHisAbility();
    }

    public int AskUserToSelectCard(Player player)
    {
        List<(int, Card)> playableCards = player.GetPlayableCardsFromPlayer();
        List<string> playableCardsFormatted = _gameFormatter.GetFormattedPlayableCards(playableCards);
        return _view.AskUserToSelectAPlay(playableCardsFormatted);
    }
}
