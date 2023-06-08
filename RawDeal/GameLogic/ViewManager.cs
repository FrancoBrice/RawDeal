using RawDeal.Cards;
using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.GameLogic;

public static class ViewManager
{
    private static View _view;

    public static void ShowDamagedCards(View view, List<Card> damagedCards, int actualDamage)
    {
        _view = view;
        int indexShowedCard = 1;
        foreach (Card damagedCard in damagedCards)
        {
            ShowCardOverturnByTakingDamage(damagedCard, indexShowedCard, actualDamage);
            indexShowedCard++;
        }
    }

    public static void ShowPlayersInfo(View view, Play currentPlay)
    {
        view.ShowGameInfo(GeneratePlayerInfo(currentPlay.CurrentPlayer),
            GeneratePlayerInfo(currentPlay.NotCurrentPlayer));
    }

    public static void ShowCardsBasedOnSelection(View view, Play currentPlay)
    {
        CardSet setOfCardsSelected = view.AskUserWhatSetOfCardsHeWantsToSee();
        List<string> cardStringsList =
            GetCardsAsStringListFromSelectedSet(currentPlay, setOfCardsSelected);
        view.ShowCards(cardStringsList);
    }

    private static List<string> GetCardsAsStringListFromSelectedSet(Play currentPlay,
        CardSet setOfCardsSelected)
    {
        List<string> cardStrings = new();

        switch (setOfCardsSelected)
        {
            case CardSet.RingArea:
                cardStrings = currentPlay.CurrentPlayer.GetCardsInStringFormatFromRingArea();
                break;
            case CardSet.Hand:
                cardStrings = currentPlay.CurrentPlayer.GetCardsInStringFormatFromHand();
                break;
            case CardSet.RingsidePile:
                cardStrings = currentPlay.CurrentPlayer.GetCardsInStringFormatFromRingside();
                break;
            case CardSet.OpponentsRingArea:
                cardStrings = currentPlay.NotCurrentPlayer.GetCardsInStringFormatFromRingArea();
                break;
            case CardSet.OpponentsRingsidePile:
                cardStrings = currentPlay.NotCurrentPlayer.GetCardsInStringFormatFromRingside();
                break;
        }

        return cardStrings;
    }

    private static PlayerInfo GeneratePlayerInfo(Player player)
    {
        return new PlayerInfo(player.GetSuperStarName(), player.Fortitude, player.GetHandSize(),
            player.GetArsenalSize());
    }

    private static void ShowCardOverturnByTakingDamage(Card damagedCard, int indexShowedCard,
        int actualDamage)
    {
        string cardFormattedInfo = damagedCard.GetCardFormattedInfo();
        _view.ShowCardOverturnByTakingDamage(cardFormattedInfo, indexShowedCard, actualDamage);
    }

    public static void SayPlayerIsTryingToPlayCard(View view, Play currentPlay)
    {
        Card selectedCard = currentPlay.GetLastCard();
        string superStarName = currentPlay.CurrentPlayer.GetSuperStarName();
        string cardInPlayFormat = selectedCard.GetCardInPlayFormat(selectedCard.PlayedType);
        view.SayThatPlayerIsTryingToPlayThisCard(superStarName, cardInPlayFormat);
    }
}