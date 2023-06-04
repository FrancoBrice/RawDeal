using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.ManeuverEffects;

public class HeadButt : Effect
{
    public HeadButt(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        _cardMobilizer.MakePlayerDiscardCards(CurrentPlayer, 1);
    }
}