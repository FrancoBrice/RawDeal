using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.EffectsForNextCards;

public class ReversalsRequiresMoreFortitudeByTypeAndSubtype : Effect
{
    private readonly int? _extraFortitude;
    private readonly string _playedTypeThatAppliesEffect;
    private readonly string _subtypeThatAppliesEffect;
    private PlayManager _playManager;

    public ReversalsRequiresMoreFortitudeByTypeAndSubtype(View view, PlayManager playManager,  string type, string subtype, 
        int extraFortitude) : base(view)
    {
        _playManager = playManager;
        _playedTypeThatAppliesEffect = type;
        _subtypeThatAppliesEffect = subtype;
        _extraFortitude = extraFortitude;
    }
    
    protected override void ApplyCustomEffect(Play currentPlay)
    {
        if (currentPlay.PlayedCardsCount > 0)
        {
            BeginEffectApplication(currentPlay);
        }
        currentPlay.RemoveAPendingEffect(this);
    }

    private void BeginEffectApplication(Play currentPlay)
    {
        List<Card> opponentReversals = _notCurrentPlayer.GetAllReversalCards();
        Card lastCard = currentPlay.GetLastCard();
        if (TypeIsCorrect(lastCard) && SubtypeIsCorrect(lastCard))
        {
            SetFortitudeInReversals(opponentReversals);
        }
    }

    private bool TypeIsCorrect(Card lastCard)
    {
        return lastCard.PlayedType == _playedTypeThatAppliesEffect;
    }
    
    private bool SubtypeIsCorrect(Card lastCard)
    {
        return lastCard.Subtypes.Contains(_subtypeThatAppliesEffect) || _subtypeThatAppliesEffect == "All";
    }

    private void SetFortitudeInReversals(List<Card> opponentReversals)
    {
        foreach (Card opponentCard in opponentReversals)
        {
            opponentCard.SetCurrentFortitude(Convert.ToInt32(opponentCard.Fortitude) +
                                             _extraFortitude);
        }
    }

    protected override bool CheckIfIsImportable()
    {
        Play previousPlay = _playManager.GetPreviousPlay();
        return previousPlay.ReversalCard != null;
    }
}