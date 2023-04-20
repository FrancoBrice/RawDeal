using Newtonsoft.Json;
using RawDealView;
namespace RawDeal;
public class Card
{
    [JsonProperty("Title")]
    public string Title { get; set; }

    [JsonProperty("Types")]
    public List<string> Types { get; set; }

    [JsonProperty("Subtypes")]
    public List<string> Subtypes { get; set; }

    [JsonProperty("Fortitude")]
    public int Fortitude { get; set; }

    [JsonProperty("Damage")]
    public string Damage { get; set; }

    [JsonProperty("StunValue")]
    public int StunValue { get; set; }

    [JsonProperty("CardEffect")]
    public string CardEffect { get; set; }

    public string GetCardFormattedInfo()
    {
        return Formatter.CardToString(Title, Fortitude.ToString(),
            Damage, StunValue.ToString(), Types, Subtypes, CardEffect);
    }

    public string GetCardInPlayFormat()
    { 
        return Formatter.PlayToString(GetCardFormattedInfo(), Types[0].ToUpper());
    }

    public int GetDamage()
    {
        return Convert.ToInt32(Damage);
    } 
}