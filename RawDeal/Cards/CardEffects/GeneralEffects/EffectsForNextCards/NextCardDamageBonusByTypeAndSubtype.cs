using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.EffectsForNextCards;

public class NextCardDamageBonusByTypeAndSubtype : Effect
{
    private PlayManager _playManager;
    private readonly int? _damageBonus;
    private readonly string _playedTypeThatAppliesBonus;
    private readonly string _subtypeThatAppliesBonus;


    public NextCardDamageBonusByTypeAndSubtype(View view, PlayManager playManager, 
        string type, string subtype, int bonus) : base(view)
    {
        _playManager = playManager;
        _playedTypeThatAppliesBonus = type;
        _subtypeThatAppliesBonus = subtype;
        _damageBonus = bonus;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (currentPlay.PlayedCardsCount > 0)
        {
            Card card = currentPlay.GetLastCard();
            if (CardHaveCorrectTypeAndSubtype(card)) 
                card.SetCurrentDamage(card.GetCurrentDamage() + _damageBonus);
            
        }
        currentPlay.RemoveAPendingEffect(this);
    }

    private bool CardHaveCorrectTypeAndSubtype(Card card)
    {
        return card.PlayedType == _playedTypeThatAppliesBonus &&
               (card.Subtypes.Contains(_subtypeThatAppliesBonus) || _subtypeThatAppliesBonus == "All");
    }

    protected override bool CheckIfIsImportable()
    {
        Play previousPlay = _playManager.GetPreviousPlay();
        return previousPlay.ReversalCard != null;
    }
}