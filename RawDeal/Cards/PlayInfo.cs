using RawDealView.Formatters;

namespace RawDeal.Cards;

public class PlayInfo : IViewablePlayInfo
{
    public IViewableCardInfo CardInfo { get; set; }
    public string PlayedAs { get; set; }

    public PlayInfo(IViewableCardInfo cardInfo, string playedAs)
    {
        CardInfo = cardInfo;
        PlayedAs = playedAs;
    }

    public string GetCardInPlayFormat()
    {
        return Formatter.PlayToString(this);
    }
}