using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ActionEffects;

public class ExtraFortitude : Effect
{
    public int AmountOfExtraFortitude { get; set; }

    public ExtraFortitude(View view) : base(view)
    {
    }

    public override void ApplyEffect(Play currentPlay)
    {
        throw new NotImplementedException();
    }
    
}