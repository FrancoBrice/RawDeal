using Newtonsoft.Json;
using RawDeal.Cards;
using RawDeal.GameLogic.Players;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.SuperStars;

public abstract class SuperStar
{
    private protected View _view;
    private protected bool _hasInitialAbility;

    protected SuperStar(string name, string logo, int handSize, int superstarValue,
        string superstarAbility)
    {
        Name = name;
        Logo = logo;
        HandSize = handSize;
        SuperstarValue = superstarValue;
        SuperstarAbility = superstarAbility;
        _hasInitialAbility = false;
    }

    public string Name { get; }
    public string Logo { get; }
    [JsonProperty("Hand Size")] public int HandSize { get; set; }
    [JsonProperty("Superstar Value")] public int SuperstarValue { get; set; }
    [JsonProperty("Superstar Ability")] public string SuperstarAbility { get; set; }

    public abstract void UseAbility(Player player, Player opponentPlayer);

    public void AddView(View view)
    {
        _view = view;
    }

    public bool HasInitialAbility()
    {
        return _hasInitialAbility;
    }

    public abstract bool CanUseAbility(Player player);

    public abstract bool IsAbilityAutomatic();

    public virtual void UseInitialAbility(Player player)
    {
    }

    protected void MakePlayerDiscardACard(Player player)
    {
        int indexCardFromPlayerHand = _view.AskPlayerToSelectACardToDiscard(
            player.GetCardsInStringFormatFromHand(), player.GetSuperStarName(),
            player.GetSuperStarName(), 1);
        CardMobilizer.FromHandToRingsideByIndex(player, indexCardFromPlayerHand);
    }

    protected Card ApplyDamageToOpponent(Player opponentPlayer, int damage)
    {
        Card discardedCard = opponentPlayer.GetCardsFromArsenal(damage)[0];
        _view.SayThatSuperstarWillTakeSomeDamage(opponentPlayer.GetSuperStarName(), damage);
        CardDamageController.PlayerReceiveDamage(opponentPlayer, damage: 1);
        return discardedCard;
    }

    protected bool DoesPlayerWantToUseAbility(Player player)
    {
        if (player.GetRingsideSize() > 0 && !player.HasUsedHisAbilityInTheTurn)
            return _view.DoesPlayerWantToUseHisAbility(Name);

        return false;
    }

    protected void RecoverCardFromRingide(Player player)
    {
        int indexCardFromRingside =
            _view.AskPlayerToSelectCardsToPutInHisHand(player.GetSuperStarName(), 1,
                player.GetCardsInStringFormatFromRingside());
        CardMobilizer.FromRingsideToHandByIndex(player, indexCardFromRingside);
    }

    protected void MakePlayerDiscardCardsWithSelection(Player player, int numberOfCardsToDiscard)
    {
        for (int i = 0; i < 2; i++)
        {
            int indexCardFromHand = _view.AskPlayerToSelectACardToDiscard(
                player.GetCardsInStringFormatFromHand(), player.GetSuperStarName(),
                player.GetSuperStarName(), numberOfCardsToDiscard);
            CardMobilizer.FromHandToRingsideByIndex(player, indexCardFromHand);
            numberOfCardsToDiscard--;
        }
    }
}