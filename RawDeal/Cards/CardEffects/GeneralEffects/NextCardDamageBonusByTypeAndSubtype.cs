using RawDeal.GameLogic;
using RawDeal.GameLogic.Plays;
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

    public void SetTypeAndSubtypeThatAppliesBonus(string type, string subtype)
    {
        _playedTypeThatAppliesBonus = type;
        _subtypeThatAppliesBonus = subtype;
    }
    
    public void SetDamageBonus(int? bonus)
    {
        _damageBonus = bonus;
    }


    protected override void ApplyCustomEffect(Play currentPlay)
    {
        try
        {
            if (string.IsNullOrEmpty(_playedTypeThatAppliesBonus) || string.IsNullOrEmpty(_subtypeThatAppliesBonus) || !_damageBonus.HasValue)
            {
                throw new InvalidOperationException("Played type or subtype not set.");
            }
            currentPlay.IsAPendingEffect = true;
            Card card = currentPlay.GetLastCard();
            if (card.PlayedType == _playedTypeThatAppliesBonus && (card.Subtypes.Contains(_subtypeThatAppliesBonus) || _subtypeThatAppliesBonus == "All"))
            {
                card.SetCurrentDamage(card.GetCurrentDamage() + _damageBonus);
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine("Error applying effect: " + ex.Message);
        }
        currentPlay.PendingEffects.Remove(this);
    }
}