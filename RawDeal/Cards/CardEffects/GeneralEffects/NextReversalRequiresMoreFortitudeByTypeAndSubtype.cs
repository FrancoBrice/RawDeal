using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects;

public class NextReversalRequiresMoreFortitudeByTypeAndSubtype : Effect
{
    private string _playedTypeThatAppliesEffect;
    private string _subtypeThatAppliesEffect;
    private int? _extraFortitude;

    public NextReversalRequiresMoreFortitudeByTypeAndSubtype(View view) : base(view)
    {
    }
    
    protected override void ApplyCustomEffect(Play currentPlay)
    {
        try
        {
            if (string.IsNullOrEmpty(_playedTypeThatAppliesEffect) || string.IsNullOrEmpty(_subtypeThatAppliesEffect) || !_extraFortitude.HasValue)
            {
                throw new InvalidOperationException("Played type, subtype or extraFortitude not set.");
            }
            List<Card> opponentReversals = CurrentPlayer.GetReversalCards();
            Card lastCard = currentPlay.GetLastCard();
            if (lastCard.PlayedType != _playedTypeThatAppliesEffect ||
                !lastCard.Subtypes.Contains(_subtypeThatAppliesEffect)) return;
            foreach (Card opponentCard in opponentReversals)
            {
                opponentCard.SetCurrentFortitude(Convert.ToInt32(opponentCard.Fortitude) + _extraFortitude);
            }
        }
        catch (InvalidOperationException ex) 
        {
            Console.WriteLine("Error applying effect: " + ex.Message);
        }
    }

    public void SetPlayedTypeThatAppliesExtraFortitude(string type)
    {
        _playedTypeThatAppliesEffect = type;
    }
    
    public void SetSubtypeThatAppliesExtraFortitude(string subtype)
    {
        _subtypeThatAppliesEffect = subtype;
    }
    
    public void SetExtraFortitude(int? extraFortitude)
    {
        _extraFortitude = extraFortitude;
    }


    
}