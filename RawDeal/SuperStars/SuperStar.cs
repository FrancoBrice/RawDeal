using Newtonsoft.Json;
using RawDeal.Cards;
using RawDeal.GameLogic.Players;
using RawDealView;

namespace RawDeal.SuperStars
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

        public abstract bool CanUseAbility(Player player);

        public abstract bool IsAbilityAutomatic();
        public virtual void UseInitialAbility(Player player){}
        protected void MakePlayerDiscardACard(Player player)
        {
            int indexCardFromPlayerHand = _view.AskPlayerToSelectACardToDiscard(player.GetCardsInStringFormatFromHand(), player.GetSuperStarName(),
                player.GetSuperStarName(), 1);
            player.MoveCardFromHandToRingsideByIndex(indexCardFromPlayerHand);
        }

        protected Card ApplyDamageToOpponent(Player opponentPlayer, int damage)
        {
            Card discartedCard = opponentPlayer.GetCardsFromArsenal(damage)[0];
            _view.SayThatSuperstarWillTakeSomeDamage(opponentPlayer.GetSuperStarName(), (int)damage);
            opponentPlayer.ReceiveDamage(1);
            return discartedCard;
        }
        protected bool DoesPlayerWantToUseAbility(Player player)
        {
            if (player.GetRingsideSize() > 0 && !player.HasUsedHisAbilityInTheTurn)
            { 
                return _view.DoesPlayerWantToUseHisAbility(Name);
            }
            
            return false;
        }

        protected void RecoverCardFromRingide(Player player)
        {
            int indexCardFromRingside =
                _view.AskPlayerToSelectCardsToPutInHisHand(player.GetSuperStarName(), 1, player.GetCardsInStringFormatFromRingside());
            player.MoveCardFromRingsideToHandByIndex(indexCardFromRingside);
        }

        protected void MakePlayerDiscardCardsWithSelection(Player player, int numberOfCardsToDiscard)
        {
            for (int i = 0; i < 2; i++)
            {
                int indexCardFromHand = _view.AskPlayerToSelectACardToDiscard(player.GetCardsInStringFormatFromHand(), player.GetSuperStarName(),
                    player.GetSuperStarName(), numberOfCardsToDiscard);
                player.MoveCardFromHandToRingsideByIndex(indexCardFromHand);
                numberOfCardsToDiscard--;
            }
        }
    }
}