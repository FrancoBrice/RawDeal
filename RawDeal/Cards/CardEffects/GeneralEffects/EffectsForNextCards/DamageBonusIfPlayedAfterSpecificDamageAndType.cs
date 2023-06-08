using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

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
        if (currentPlay.PlayedCardsCount < 2 || NotCurrentPlayer.DamagesReceived.Count == 0) return;
        Card attackingCard = AttackingCard;
        Card previousCardPlayed = currentPlay.PlayedCards.GetPenultimateCard;
        int lastDamage = NotCurrentPlayer.DamagesReceived[^1];
        if (previousCardPlayed.PlayedType == _typeOfPreviousCardThatAppliesBonus &&
            lastDamage >= _minimumDamage)
            attackingCard.SetCurrentDamage(attackingCard.GetCurrentDamage() + _damageBonus);
    }
}