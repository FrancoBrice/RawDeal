using RawDeal.GameLogic;
using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class DamageBonusIfPlayedAfterSpecificDamageAndType : Effect
{
    private int _minimumDamage;
    private string _typeOfPreviusCardThatApliesBonus;
    private int _damageBonus;
    
    public DamageBonusIfPlayedAfterSpecificDamageAndType(View view, int damageBonus, int minimumDamage, string typeOfPreviousCardThatAppliesBonus) : base(view)
    {
        _minimumDamage = minimumDamage;
        _typeOfPreviusCardThatApliesBonus = typeOfPreviousCardThatAppliesBonus;
        _damageBonus = damageBonus;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (currentPlay.PlayedCardsCount < 2 || currentPlay.NotCurrentPlayer.DamagesReceived.Count == 0) return;
        Card attackingCard = currentPlay.AttackingCard;
        Card previousCardPlayed = currentPlay.PlayedCards.GetPenultimateCard;
        int lastDamage = currentPlay.NotCurrentPlayer.DamagesReceived[^1];
        if (previousCardPlayed.PlayedType == _typeOfPreviusCardThatApliesBonus && lastDamage >= _minimumDamage)
        {
            attackingCard.SetCurrentDamage(attackingCard.GetCurrentDamage() + _damageBonus);
        }
    }
}