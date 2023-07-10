using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class BonusBySlamCardsInRingArea : Effect
{
    private readonly Card _cardThatReceivesBonus;
    private int? _damageOfTheCardThatReceivesBonus => _cardThatReceivesBonus.GetCurrentDamage();
    public BonusBySlamCardsInRingArea(View view, Card cardThatReceivesBonus) : base(view)
    {
        _cardThatReceivesBonus = cardThatReceivesBonus;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        foreach (Card card in _currentPlayer.GetAllRingAreaCards())
        {
            if (CardContainsWordSlam(card))
            {
                _cardThatReceivesBonus.SetCurrentDamage(_damageOfTheCardThatReceivesBonus + 1);
            }
        }
    }

    private static bool CardContainsWordSlam(Card card)
    {
        return card.Title.Contains(" slam") || card.Title.Contains(" Slam");
    }
}