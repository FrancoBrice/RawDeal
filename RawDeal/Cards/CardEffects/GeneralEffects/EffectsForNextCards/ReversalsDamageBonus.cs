using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.EffectsForNextCards;

public class ReversalsDamageBonus : Effect
{
    private readonly int _damageBonus;
    private PlayManager _playManager;
    
    public ReversalsDamageBonus(View view, PlayManager playManager, int damageBonus) : base(view)
    {
        _playManager = playManager;
        _damageBonus = damageBonus;
    }
    
    protected override void ApplyCustomEffect(Play currentPlay)
    {
        List<Card> opponentReversals = _notCurrentPlayer.GetAllReversalCards();
        Card lastCard = currentPlay.GetLastCard();
        if (lastCard.PlayedType != "Reversal")
        {
            currentPlay.RemoveAPendingEffect(this);
            return;
        }
        AddBonusToOpponentReversals(opponentReversals);
        currentPlay.RemoveAPendingEffect(this);
    }

    private void AddBonusToOpponentReversals(List<Card> opponentReversals)
    {
        foreach (Card opponentCard in opponentReversals)
        {
            opponentCard.SetCurrentDamage(opponentCard.GetDefaultDamage() + _damageBonus);
            opponentCard.HasPendingEffect = true;
        }
    }
    
    protected override bool CheckIfIsImportable()
    {
        return true;
    }
    
}