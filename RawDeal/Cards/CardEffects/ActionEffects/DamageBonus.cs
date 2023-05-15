using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ActionEffects;

public class DamageBonus : Effect
{
    public int AmountOfBonus { get; set; }
    
    public DamageBonus(View view) : base(view)
    {
    }

    public override void ApplyEffect(Play currentPlay)
    {
        
    }
}