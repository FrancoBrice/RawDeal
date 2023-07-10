using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.EffectsForNextCards;

public class NextCardFortitudeBonusByTitle : Effect
{
    private string _cardTitleThatApplies;
    private readonly int _fortitudeBonus;
    
    public NextCardFortitudeBonusByTitle(View view, string cardTitleThatApplies,
        int fortitudeBonus) : base(view)
    {
        _cardTitleThatApplies = cardTitleThatApplies;
        _fortitudeBonus = fortitudeBonus;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        List<Card> playerCardsFromHand = _currentPlayer.GetAllHandCards();
        foreach (Card card in playerCardsFromHand)
        {
            if (card.Title == _cardTitleThatApplies)
            {
                card.SetCurrentFortitude(Convert.ToInt32(card.Fortitude) + _fortitudeBonus);
            }

        }
        currentPlay.RemoveAPendingEffect(this);
    }
    
}
