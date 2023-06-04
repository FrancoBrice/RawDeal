using RawDeal.Cards;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.GameLogic;

public class ViewManager
{
    private View _view;

    public ViewManager(View view)
    {
        _view = view;
    }

    public void ShowDamagedCards(List<Card> damagedCards, int actualDamage)
    {
        int indexShowedCard = 1;
        foreach (Card damagedCard in damagedCards)
        {
            ShowCardOverturnByTakingDamage(damagedCard, indexShowedCard, actualDamage);
            indexShowedCard++;
        }
    }
    
    public void ShowPlayersInfo(Play currentPlay)
    {
        _view.ShowGameInfo(GeneratePlayerInfo(currentPlay.CurrentPlayer), GeneratePlayerInfo(currentPlay.NotCurrentPlayer));
    }
    
    public void ShowCardsBasedOnSelection(Play currentPlay)
    {

        CardSet setOfCardsSelected = _view.AskUserWhatSetOfCardsHeWantsToSee();
        List<string> cardstringsList = GetCardsAsStringListFromSelectedSet(currentPlay, setOfCardsSelected);
        _view.ShowCards(cardstringsList);
    }
    
    private List<string> GetCardsAsStringListFromSelectedSet(Play currentPlay, CardSet setOfCardsSelected)
    {
        List<string> cardStrings = new List<string>();
        
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

    private PlayerInfo GeneratePlayerInfo(Player player)
    {
        return new PlayerInfo(player.GetSuperStarName(), (int)player.Fortitude, player.GetHandSize(),
            player.GetArsenalSize());
    }

    private void ShowCardOverturnByTakingDamage(Card damagedCard, int indexShowedCard, int actualDamage)
    {
        string cardFormattedInfo = damagedCard.GetCardFormattedInfo();
        _view.ShowCardOverturnByTakingDamage(cardFormattedInfo, indexShowedCard, actualDamage);
    }

    public void SayPlayerIsTryingToPlayCard(Play currentPlay)
    {
        Card selectedCard = currentPlay.GetLastCard();
        string superStarName = currentPlay.CurrentPlayer.GetSuperStarName();
        string cardInPlayFormat = selectedCard.GetCardInPlayFormat(selectedCard.PlayedType);
        _view.SayThatPlayerIsTryingToPlayThisCard(superStarName, cardInPlayFormat);
    }
    
    
}