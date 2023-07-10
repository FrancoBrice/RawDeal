using RawDealView.Formatters;

namespace RawDeal.Cards;

public class PlayInfo : IViewablePlayInfo
{
    public PlayInfo(IViewableCardInfo cardInfo, string playedAs)
    {
        CardInfo = cardInfo;
        PlayedAs = playedAs;
    }

    public IViewableCardInfo CardInfo { get; }
    public string PlayedAs { get; }

    public string GetCardInPlayFormat()
    {
        return Formatter.PlayToString(this);
    }
}