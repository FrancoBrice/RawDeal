using Newtonsoft.Json;
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

    private View _view; 

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

    public void ApplyActionEffect(Player currentPlayer, Player notCurrentPlayer)
    {
        currentPlayer.MoveCardFromArsenalToHand();
        _view.SayThatPlayerMustDiscardThisCard(currentPlayer.GetSuperStarName(), Title);
        _view.SayThatPlayerDrawCards(currentPlayer.GetSuperStarName(), 1);
    }


    public int GetDamage()
    {
        return Convert.ToInt32(Damage);
    }

    public int GetFortitude()
    {
        return Convert.ToInt32(Fortitude);
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

    public void SetReversalType()
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

    }

}