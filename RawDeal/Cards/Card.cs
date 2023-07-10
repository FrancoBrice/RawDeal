using Newtonsoft.Json;
using RawDeal.GameLogic.Plays;
using RawDealView.Formatters;

namespace RawDeal.Cards;

public class Card : IViewableCardInfo
{
    private int? _currentDamage;
    private int? _currentFortitude;
    public bool HasVariableFortitude;
    public string PlayedType { get; set; }
    public string PlayedFrom { get; set; }

    public bool ItsTypeManeuver => Types.Contains("Maneuver");

    public bool IsTypeAction => Types.Contains("Action");

    public bool HasPendingEffect;

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
    
    public Card()
    {
        HasPendingEffect = false;
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
        SetVaribaleFortitude();
    }

    private void SetVaribaleFortitude()
    {
        switch (Title)
        {
            case "Undertaker's Tombstone Piledriver":
            case "Stone Cold Stunner":
                HasVariableFortitude = true;
                break;
            default:
                HasVariableFortitude = false;
                break;
        }
    }


    public string GetCardFormattedInfo()
    {
        return Formatter.CardToString(this);
    }

    public string GetCardInPlayFormat(string typeOfCardPlayedAs)
    {
        PlayInfo playInfo = new PlayInfo(this, typeOfCardPlayedAs.ToUpper());
        return playInfo.GetCardInPlayFormat();
    }

    public int? GetCurrentDamage()
    {
        if (PlayedType != "Action") return _currentDamage;
        switch (Title)
        {
            case "Chop":
            case "Arm Bar Takedown":
            case "Collar & Elbow Lockup":
            case "Undertaker's Tombstone Piledriver":
            case "The People's Elbow":
                return 0;
            default:
                return _currentDamage;
        }
    }
    
    public int GetDefaultDamage()
    {
        string cardDamageString = Damage;
        return cardDamageString == "#" ? 0 : Convert.ToInt32(cardDamageString);
    }

    public int? GetCurrentFortitude(Play currentPlay, string playedType)
    {
        Card lastCardPlayed = currentPlay.AttackingCard;
        switch (Title)
        {
            case "Undertaker's Tombstone Piledriver" when playedType == "Maneuver":
                return _currentFortitude;
            case "Undertaker's Tombstone Piledriver":
                return 0;
            case "Stone Cold Stunner":
                if (lastCardPlayed is { Title: "Kick" }) return _currentFortitude - 6;
                break;
            case "Kane's Tombstone Piledriver":
                if (lastCardPlayed is { Title: "Kane's Chokeslam" }) return _currentFortitude - 6;
                break;
                
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