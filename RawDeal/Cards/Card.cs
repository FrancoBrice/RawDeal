using Newtonsoft.Json;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class Card : IViewableCardInfo
{
    private int? _currentDamage;
    private int? _currentFortitude;

    public Card()
    {
    }

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

    public string PlayedType { get; set; }
    public string PlayedFrom { get; set; }

    public bool ItsTypeManeuver => Types.Contains("Maneuver");

    public bool IsTypeAction => Types.Contains("Action");


    public bool IsHybrid => Types.Count > 1;

    public bool IsTypeReversal => Types.Contains("Reversal");

    public bool CanBeReversed
    {
        get
        {
            string[] titlesThatCannotReverse =
                { "Tree of Woe", "Austin Elbow Smash", "Leaping Knee to the Face" };
            return !titlesThatCannotReverse.Contains(Title);
        }
    }

    [JsonProperty("Title")] public string Title { get; set; }

    [JsonProperty("Types")] public List<string> Types { get; set; }

    [JsonProperty("Subtypes")] public List<string> Subtypes { get; set; }

    [JsonProperty("Fortitude")] public string Fortitude { get; set; }

    [JsonProperty("Damage")] public string Damage { get; set; }

    [JsonProperty("StunValue")] public string StunValue { get; set; }

    [JsonProperty("CardEffect")] public string CardEffect { get; set; }

    public string GetCardFormattedInfo()

    {
        return Formatter.CardToString(this);
    }

    public string GetCardInPlayFormat(string typeOfCardPlayedAs)
    {
        PlayInfo playInfo = new(this, typeOfCardPlayedAs.ToUpper());
        return playInfo.GetCardInPlayFormat();
    }

    public int? GetCurrentDamage()
    {
        switch (Title)
        {
            case "Chop" when PlayedType == "Action":
            case "Arm Bar Takedown" when PlayedType == "Action":
            case "Collar & Elbow Lockup" when PlayedType == "Action":
            case "Undertaker's Tombstone Piledriver" when PlayedType == "Action":
                return 0;
            default:
                return _currentDamage;
        }
    }

    public int? GetCurrentFortitude(string playedType)
    {
        if (Title == "Undertaker's Tombstone Piledriver")
        {
            if (playedType == "Maneuver") return _currentFortitude;
            return 0;
        }

        return _currentFortitude;
    }

    private void SetDefaultDamage()
    {
        string cardDamageString = Damage;
        if (cardDamageString == "#")
        {
            _currentDamage = 0;
            return;
        }

        _currentDamage = Convert.ToInt32(cardDamageString);
    }

    private void SetDefaultFortitude()
    {
        string cardFortitudeString = Fortitude;
        _currentFortitude = Convert.ToInt32(cardFortitudeString);
    }

    public void SetCurrentDamage(int? currentDamage)
    {
        _currentDamage = currentDamage;
    }

    public void SetCurrentFortitude(int? currentFortitude)
    {
        _currentFortitude = currentFortitude;
    }

    public int GetStunValue()
    {
        return Convert.ToInt32(StunValue);
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

    public bool TypeIsPlayable()
    {
        return ItsTypeManeuver || IsTypeAction;
    }

    public bool CurrentPlayedTypeIsPlayable()
    {
        return PlayedType is "Maneuver" or "Action";
    }

    public void SetDefaultValues()
    {
        SetDefaultDamage();
        SetDefaultFortitude();
    }
}