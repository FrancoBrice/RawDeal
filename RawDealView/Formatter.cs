namespace RawDealView;

public static class Formatter
{
    public static string PlayToString(string cardInfo, string playedAs)
        => $"[{playedAs}] {cardInfo}";

    public static string CardToString(string title, string fortitude, string damage, string stunValue,
        List<string> cardTypes, List<string> cardSubtypes, string cardEffect)
    {
        string cardInfo = GetFormattedTitle(title);
        cardInfo += GetFormattedCardStats(fortitude, damage, stunValue);
        cardInfo += GetFormattedCardTypesAndSubtypes(cardTypes, cardSubtypes);
        cardInfo += GetFormattedCardEffect(cardEffect);
        return cardInfo;
    }

    private static string GetFormattedTitle(string title)
        => "*" + title + "*";
    
    private static string GetFormattedCardStats(string fortitude, string damage, string stunValue)
    {
        string info = ". Info: ";
        info += $"{fortitude}F/{damage}D";
        if (stunValue != "0")
            info += $"/{stunValue}SV";
        return info;
    }

    private static string GetFormattedCardTypesAndSubtypes(List<string> cardTypes, List<string> cardSubtypes)
    {
        string types = String.Join('/', cardTypes);
        if(cardSubtypes.Any())
            types += ", " + String.Join('/', cardSubtypes);
        return ", " + types + ".";
    }

    private static string GetFormattedCardEffect(string cardEffect)
    {
        if(cardEffect.Any())
            return $" Effect: {cardEffect}";
        return "";
    }
    
}