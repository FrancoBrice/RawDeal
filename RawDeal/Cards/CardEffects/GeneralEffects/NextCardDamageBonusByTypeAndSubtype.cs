using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class NextCardDamageBonusByTypeAndSubtype : Effect
{
    private string _playedTypeThatAppliesBonus;
    private string _subtypeThatAppliesBonus;
    private int? _damageBonus;
    
    
    public NextCardDamageBonusByTypeAndSubtype(View view) : base(view)
    {
    }

    public void SetPlayedTypeThatAppliesBonus(string type)
    {
        _playedTypeThatAppliesBonus = type;
    }
    
    public void SetSubtypeThatAppliesBonus(string subtype)
    {
        _subtypeThatAppliesBonus = subtype;
    }
    
    public void SetDamageBonus(int? bonus)
    {
        _damageBonus = bonus;
    }


    protected override void ApplyCustomEffect(Play currentPlay)
    {
        currentPlay.IsAPendingEffect = true;
        try
        {
            if (string.IsNullOrEmpty(_playedTypeThatAppliesBonus) || string.IsNullOrEmpty(_subtypeThatAppliesBonus) || !_damageBonus.HasValue)
            {
                throw new InvalidOperationException("Played type or subtype not set.");
            }

            Card card = currentPlay.GetLastCard();
            if (card.PlayedType == _playedTypeThatAppliesBonus && card.Subtypes.Contains(_subtypeThatAppliesBonus))
            {
                card.SetCurrentDamage(card.GetCurrentDamage() + _damageBonus);
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Error applying effect: " + ex.Message);
        }
    }
}