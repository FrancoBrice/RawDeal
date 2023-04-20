using Newtonsoft.Json;
using RawDealView;

namespace RawDeal
{
    public abstract class SuperStar
    {
        public string Name { get; }
        public string Logo { get; }
        [JsonProperty("Hand Size")] public int HandSize { get; set; }
        [JsonProperty("Superstar Value")] public int SuperstarValue { get; set; }
        [JsonProperty("Superstar Ability")] public string SuperstarAbility { get; set; }
        protected View _view;
        public bool HasInitialAbility;

        public SuperStar(string name, string logo, int handSize, int superstarValue, string superstarAbility)
        {
            Name = name;
            Logo = logo;
            HandSize = handSize;
            SuperstarValue = superstarValue;
            SuperstarAbility = superstarAbility;
            HasInitialAbility = false;
        }

        public abstract void UseAbility(Player player, Player opponentPlayer);

        public void AddView(View view)
        {
            _view = view;
        }

        public abstract bool CheckIfCanUseAbility(Player player);

        public abstract bool CheckIfAbilityIsAutomatic();
        public virtual void UseInitialAbility(Player player){}
    }
}