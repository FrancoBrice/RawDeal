using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.EffectsForNextCards;

public class NextReversalRequiresMoreFortitudeByTypeAndSubtype : Effect
{
    private int? _extraFortitude;
    private string _playedTypeThatAppliesEffect;
    private string _subtypeThatAppliesEffect;

    public NextReversalRequiresMoreFortitudeByTypeAndSubtype(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        currentPlay.IsAPendingEffect = true;
        try
        {
            if (string.IsNullOrEmpty(_playedTypeThatAppliesEffect) ||
                string.IsNullOrEmpty(_subtypeThatAppliesEffect) ||
                !_extraFortitude.HasValue)
                throw new InvalidOperationException(
                    "Played type, subtype or extraFortitude not set.");
            List<Card> opponentReversals = CurrentPlayer.GetAllReversalCards();
            Card lastCard = currentPlay.GetLastCard();
            if (lastCard.PlayedType != _playedTypeThatAppliesEffect ||
                !lastCard.Subtypes.Contains(_subtypeThatAppliesEffect))
            {
                currentPlay.PendingEffects.Remove(this);
                return;
            }

            foreach (Card opponentCard in opponentReversals)
                opponentCard.SetCurrentFortitude(Convert.ToInt32(opponentCard.Fortitude) +
                                                 _extraFortitude);
            currentPlay.PendingEffects.Remove(this);
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