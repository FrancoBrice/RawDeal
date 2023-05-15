using RawDeal.Cards.CardEffects.ReversalsEffects;
using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects;

public abstract class Effect
{
    protected Player CurrentPlayer;
    protected Player NotCurrentPlayer;
    protected readonly View _view;
    protected CardMobilizer _cardMobilizer;

    protected Effect(View view)
    {
        _view = view;
        _cardMobilizer = new CardMobilizer();
    }
    public abstract void ApplyEffect(Play currentPlay);

    protected bool CanReverseByDamage(int maximumDamageThatCanReverse, int actualDamage)
    {
        return actualDamage <= maximumDamageThatCanReverse;
    }

    protected void ApplyReversalDamageIfPossible(Card revesalCard)
    {
        if (revesalCard.GetCurrentDamage() < 0) return;
        int reversalDamage = revesalCard.GetCurrentDamage();
        CurrentPlayer.ReceiveDamageWithView(reversalDamage);
        
    }
}