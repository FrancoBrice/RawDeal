using RawDealView.Formatters;

namespace RawDeal.Cards;

public class PlayInfo : IViewablePlayInfo
{
    public PlayInfo(IViewableCardInfo cardInfo, string playedAs)
    {
        CardInfo = cardInfo;
        PlayedAs = playedAs;
    }

    public IViewableCardInfo CardInfo { get; set; }
    public string PlayedAs { get; set; }

    public string GetCardInPlayFormat()
    {
        return Formatter.PlayToString(this);
    }
}