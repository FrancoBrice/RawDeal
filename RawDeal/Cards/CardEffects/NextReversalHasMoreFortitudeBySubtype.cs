using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects;

public class NextReversalHasMoreFortitudeBySubtype : Effect
{
    private string _playedTypeThatAppliesEffect;
    private string _subtypeThatAppliesEffect;
    private int? _extraFortitude;

    public NextReversalHasMoreFortitudeBySubtype(View view) : base(view)
    {
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
    

    public override void ApplyEffect(Play currentPlay)
    {
        try
        {
            if (string.IsNullOrEmpty(_playedTypeThatAppliesEffect) || string.IsNullOrEmpty(_subtypeThatAppliesEffect) || !_extraFortitude.HasValue)
            {
                throw new InvalidOperationException("Played type, subtype or extraFortitude not set.");
            }

            List<Card> opponentReversals = currentPlay.NotCurrentPlayer.GetReversalCards();
            Console.WriteLine("currentPlay.NotCurrentPlayer.GetSuperStarName()");
            Console.WriteLine(currentPlay.NotCurrentPlayer.GetSuperStarName());
            Card card = currentPlay.GetLastCard();
            if (card.PlayedType != _playedTypeThatAppliesEffect ||
                !card.Subtypes.Contains(_subtypeThatAppliesEffect)) return;
            
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
}