using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ReversalsEffects;

public class ReversalByTitle : Effect
{
    private string _cardTitleThatCanReverse;
    
    public ReversalByTitle(View view, string cardTitleThatCanReverse) : base(view)
    {
        _cardTitleThatCanReverse = cardTitleThatCanReverse;

    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        string attackingCardTitle = currentPlay.AttackingCard.Title;
        if (attackingCardTitle == _cardTitleThatCanReverse)
        {
            ReversalSimple cardEffect = new ReversalSimple(_view);
            cardEffect.ApplyEffect(currentPlay);
        }
    }
}