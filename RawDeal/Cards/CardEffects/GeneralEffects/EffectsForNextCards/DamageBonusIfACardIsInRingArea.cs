using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.EffectsForNextCards;

public class DamageBonusIfACardIsInRingArea : Effect
{
    private string _cardTitleThatMustBeInRingArea;
    private int _damageBonus;
    
    public DamageBonusIfACardIsInRingArea(View view, string cardTitleThatMustBeInRingArea, 
        int damageBonus) : base(view)
    {
        _cardTitleThatMustBeInRingArea = cardTitleThatMustBeInRingArea;
        _damageBonus = damageBonus;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        foreach (Card ringAreaCard in _currentPlayer.GetAllRingAreaCards())
        {
            if (ringAreaCard.Title == _cardTitleThatMustBeInRingArea)
            {
                ApplyBonusToHandCard();
            }
        }
    }

    private void ApplyBonusToHandCard()
    {
        foreach (Card handCard in _currentPlayer.GetAllHandCards())
        {
            if (handCard.PlayedType == "Maneuver")
            {
                handCard.SetCurrentDamage(handCard.GetDefaultDamage() + _damageBonus);
            }
        }
    }

    protected override bool CheckIfIsImportable()
    {
        return true;
    }
}