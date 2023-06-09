using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.EffectsForNextCards;

public class ReversalsRequiresMoreFortitudeByTypeAndSubtype : Effect
{
    private readonly int? _extraFortitude;
    private readonly string _playedTypeThatAppliesEffect;
    private readonly string _subtypeThatAppliesEffect;

    public ReversalsRequiresMoreFortitudeByTypeAndSubtype(View view, string type, string subtype, int extraFortitude) : base(view)
    {
        _playedTypeThatAppliesEffect = type;
        _subtypeThatAppliesEffect = subtype;
        _extraFortitude = extraFortitude;
    }
    
    protected override void ApplyCustomEffect(Play currentPlay)
    {
        currentPlay.IsAPendingEffect = true;
        List<Card> opponentReversals = _currentPlayer.GetAllReversalCards();
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
}