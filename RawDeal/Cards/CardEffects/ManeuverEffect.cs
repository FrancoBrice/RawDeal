using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects;

public class ManeuverEffect : Effect
{
    public ManeuverEffect(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        {
            Card attackingCard = AttackingCard;
            _cardMobilizer.MoveCardFromHandToRingArea(CurrentPlayer, AttackingCardTuple);
            _view.SayThatPlayerSuccessfullyPlayedACard();
            if (currentPlay.NotCurrentPlayer.CalculateDamage(attackingCard) > 0) 
            {
                //ApplyCardDamage(CurrentPlayer, NotCurrentPlayer, attackingCard);
            }
        }
    }
}