using Newtonsoft.Json;
using RawDealView;

namespace RawDeal
{
    public class Card
    {
        [JsonProperty("Title")] public string Title { get; set; }

        [JsonProperty("Types")] public List<string> Types { get; set; }

        [JsonProperty("Subtypes")] public List<string> Subtypes { get; set; }

        [JsonProperty("Fortitude")] public int Fortitude { get; set;  }

        [JsonProperty("Damage")] public string Damage { get; set; }

        [JsonProperty("StunValue")] private int StunValue { get; set; }

        [JsonProperty("CardEffect")] private string CardEffect { get; set; }
        public int Id;

        public Card()
        {
        }

        public string GetCardFormattedInfo()
        {
            return Formatter.CardToString(
                Title,
                Fortitude.ToString(),
                Damage,
                StunValue.ToString(),
                Types,
                Subtypes,
                CardEffect);
        }

        public string GetCardInPlayFormat()
        {
            return Formatter.PlayToString(GetCardFormattedInfo(), Types[0].ToUpper());
        }

        public int GetDamage()
        {
            return Convert.ToInt32(Damage);
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
}