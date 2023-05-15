using RawDeal.Cards;
using RawDealView;

namespace RawDeal.GameLogic;

public class DamagedCardsDisplayer
{
    private View _view;

    public DamagedCardsDisplayer(View view)
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
    
    public void ShowCardOverturnByTakingDamage(Card damagedCard, int indexShowedCard, int actualDamage)
    {
        string cardFormattedInfo = damagedCard.GetCardFormattedInfo();
        _view.ShowCardOverturnByTakingDamage(cardFormattedInfo, indexShowedCard, actualDamage);
    }
}