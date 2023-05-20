using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ReversalsEffects;

public class ReversalByTitle : Effect
{
    private string _cardTitleThatCanReverse;
    
    public ReversalByTitle(View view) : base(view)
    {
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

    public void SetCardTitleThatCanReverse(string cardTitle)
    {
        _cardTitleThatCanReverse = cardTitle;
    }

}