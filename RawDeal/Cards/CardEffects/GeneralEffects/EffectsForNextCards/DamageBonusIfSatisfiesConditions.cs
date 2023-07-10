using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.EffectsForNextCards;

public class DamageBonusIfSatisfiesConditions : Effect
{
    private readonly int _damageBonus;
    private readonly int _minimumDamage;
    private readonly string _typeOfPreviousCardThatAppliesBonus;
    private readonly string _subtypeOfPreviousCardThatAppliesBonus;

    public DamageBonusIfSatisfiesConditions(View view, int damageBonus,
        int minimumDamage, string typeOfPreviousCardThatAppliesBonus, 
        string subtypeOfPreviousCardThatAppliesBonus) : base(view)
    {
        _minimumDamage = minimumDamage;
        _typeOfPreviousCardThatAppliesBonus = typeOfPreviousCardThatAppliesBonus;
        _subtypeOfPreviousCardThatAppliesBonus = subtypeOfPreviousCardThatAppliesBonus;
        _damageBonus = damageBonus;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (!AreCardsSuccessfullyPlayed(currentPlay)) return;
        Card attackingCard = _attackingCard;
        Card previousCardPlayed = currentPlay.PlayedCards.GetPenultimateCard; ;
        if (PreviousCardSatisfiesConditions(previousCardPlayed))
        {
            attackingCard.SetCurrentDamage(attackingCard.GetCurrentDamage() + _damageBonus);
        }
    }

    private bool AreCardsSuccessfullyPlayed(Play currentPlay)
    {
        return currentPlay.PlayedCardsCount >= 2 && _notCurrentPlayer.AmountOfDamagesReceived() != 0;
    }

    private bool PreviousCardSatisfiesConditions(Card previousCardPlayed)
    {
        return previousCardPlayed.PlayedType == _typeOfPreviousCardThatAppliesBonus &&
               previousCardPlayed.Subtypes.Contains(_subtypeOfPreviousCardThatAppliesBonus) ||
               _subtypeOfPreviousCardThatAppliesBonus == "All" &&
               _notCurrentPlayer.LastDamageReceived() >= _minimumDamage;
    }
}