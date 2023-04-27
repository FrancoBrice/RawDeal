using Newtonsoft.Json;
using RawDeal.Cards;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal;
public class Card : IViewableCardInfo
{
    [JsonProperty("Title")]
    public string Title { get; set; }

    [JsonProperty("Types")]
    public List<string> Types { get; set; }

    [JsonProperty("Subtypes")]
    public List<string> Subtypes { get; set; }

    [JsonProperty("Fortitude")]
    public string Fortitude { get; set; }

    [JsonProperty("Damage")]
    public string Damage { get; set; }

    [JsonProperty("StunValue")]
    public string StunValue { get; set; }

    [JsonProperty("CardEffect")]
    public string CardEffect { get; set; }

    public Card() { }

    public Card(IViewableCardInfo cardInfo)
    {
        Title = cardInfo.Title;
        Types = cardInfo.Types;
        Subtypes = cardInfo.Subtypes;
        Fortitude = cardInfo.Fortitude;
        Damage = cardInfo.Damage;
        StunValue = cardInfo.StunValue;
        CardEffect = cardInfo.CardEffect;
    }

    public string GetCardFormattedInfo()
    
    {   
        return Formatter.CardToString(this);
    }

    public string GetCardInPlayFormat()
    {
        PlayInfo playInfo = new PlayInfo(this, Types[0].ToUpper());
        return playInfo.GetCardInPlayFormat();
    }

    public int GetDamage()
    {
        return Convert.ToInt32(Damage);
    }

    public int GetFortitude()
    {
        return Convert.ToInt32(Fortitude);
    }

    public bool ItsUnique()
    {
        return Subtypes.Contains("Unique");
    }

    public bool ItsSetUp()
    {
        return Subtypes.Contains("SetUp");
    }

    public bool HasSubtypeHeel()
    {
        return Subtypes.Contains("Heel");
    }
    public bool HasSubtypeFace()
    {
        return Subtypes.Contains("Face");
    }
    public bool ItsTypeManeuver()
    {
        return Types.Contains("Maneuver");
    }
    public bool IsTypeAction()
    {
        return Types.Contains("Action");
    }
}