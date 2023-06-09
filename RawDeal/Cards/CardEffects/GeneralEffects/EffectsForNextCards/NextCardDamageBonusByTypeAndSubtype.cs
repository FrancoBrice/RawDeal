using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.EffectsForNextCards;

public class NextCardDamageBonusByTypeAndSubtype : Effect
{
    private readonly int? _damageBonus;
    private readonly string _playedTypeThatAppliesBonus;
    private readonly string _subtypeThatAppliesBonus;


    public NextCardDamageBonusByTypeAndSubtype(View view, string type, string subtype, int bonus) :
        base(view)
    {
        _playedTypeThatAppliesBonus = type;
        _subtypeThatAppliesBonus = subtype;
        _damageBonus = bonus;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        currentPlay.IsAPendingEffect = true;
        Card card = currentPlay.GetLastCard();
        if (card.PlayedType == _playedTypeThatAppliesBonus &&
            (card.Subtypes.Contains(_subtypeThatAppliesBonus) || _subtypeThatAppliesBonus == "All"))
            card.SetCurrentDamage(card.GetCurrentDamage() + _damageBonus);
        currentPlay.PendingEffects.Remove(this);
    }
}