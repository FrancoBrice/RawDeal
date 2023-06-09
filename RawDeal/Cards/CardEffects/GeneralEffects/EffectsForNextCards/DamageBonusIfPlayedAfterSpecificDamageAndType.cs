using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.EffectsForNextCards;

public class DamageBonusIfPlayedAfterSpecificDamageAndType : Effect
{
    private readonly int _damageBonus;
    private readonly int _minimumDamage;
    private readonly string _typeOfPreviousCardThatAppliesBonus;

    public DamageBonusIfPlayedAfterSpecificDamageAndType(View view, int damageBonus,
        int minimumDamage, string typeOfPreviousCardThatAppliesBonus) : base(view)
    {
        _minimumDamage = minimumDamage;
        _typeOfPreviousCardThatAppliesBonus = typeOfPreviousCardThatAppliesBonus;
        _damageBonus = damageBonus;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (currentPlay.PlayedCardsCount < 2 || _notCurrentPlayer.AmountOfDamagesReceived() == 0) return;
        Card attackingCard = _attackingCard;
        Card previousCardPlayed = currentPlay.PlayedCards.GetPenultimateCard; ;
        if (previousCardPlayed.PlayedType == _typeOfPreviousCardThatAppliesBonus &&
            _notCurrentPlayer.LastDamageReceived() >= _minimumDamage)
            attackingCard.SetCurrentDamage(attackingCard.GetCurrentDamage() + _damageBonus);
    }
}