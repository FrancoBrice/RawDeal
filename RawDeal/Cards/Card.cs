using Newtonsoft.Json;
using RawDeal.Cards.CardEffects;
using RawDeal.GameLogic;
using RawDealView.Formatters;
using RawDealView;

namespace RawDeal.Cards;
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
    public string PlayedType { get; set; }
    public string ReversalType { get; set; }

    public int? CurrentDamage;
    public int? CurrentFortitude;

    private View _view;
    
    public Effect Effect { get; set; }

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
    
    public void SetViewObject(View view)
    {
        _view = view;
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
    
    public bool ItsTypeManeuver => Types.Contains("Maneuver");

    public bool IsTypeAction => Types.Contains("Action");
    
    
    public bool IsHybrid => Types.Count > 1;
    
    public bool IsTypeReversal => Types.Contains("Reversal");

    public int AmountOfTypes => Types.Count;

    public int? GetCurrentDamage()
    {
        return CurrentDamage;
    }

    public void SetDefaultDamage()
    {
        string cardDamageString = Damage;
        if (cardDamageString.Contains("#"))
        {
            CurrentDamage = 0;
            return;
        }
        CurrentDamage = Convert.ToInt32(cardDamageString);


    }
    
    private void SetDefaultFortitude()
    {
        string cardFortitudeString = Fortitude;
        CurrentFortitude = Convert.ToInt32(cardFortitudeString);
    }

    public void SetCurrentDamage(int? currentDamage)
    {
        CurrentDamage = currentDamage;
    }
    
    public void SetCurrentFortitude(int? currentFortitude)
    {
        CurrentFortitude = currentFortitude;
    }

    public int? GetCurrentFortitude()
    {
        return CurrentFortitude;
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
        return PlayedType == "Maneuver" || PlayedType == "Action";
    }

    public void SetReversalTypeAndSubtype()
    {
        if (!IsTypeReversal) return;
        PlayedType = "Reversal";
        if (Subtypes.Contains("ReversalStrike"))
        {
            ReversalType = "ReversalStrike";
        }
        else if (Subtypes.Contains("ReversalGrapple"))
        {
            ReversalType = "ReversalGrapple";
        }
        else if (Subtypes.Contains("ReversalSubmission"))
        {
            ReversalType = "ReversalSubmission";
        }
        else if (Subtypes.Contains("ReversalAction"))
        {
            ReversalType = "ReversalAction";
        }
        else if (Subtypes.Contains("ReversalSpecial"))
        {
            ReversalType = "ReversalSpecial";
        }

    }

    public void SetDefaultValues()
    {
        SetDefaultDamage();
        SetDefaultFortitude();
    }

    public void ApplyEffect(Play currentPlay)
    {
        Effect.ApplyEffect(currentPlay);
    }


}