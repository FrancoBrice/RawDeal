using RawDeal.Cards.CardEffects.GeneralEffects.MovementEffects;
using RawDeal.GameLogic.Plays;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.Cards.CardEffects.GeneralEffects.SelectableEffects;

public class MrSockoSelectable : Effect
{
    public MrSockoSelectable(View view) : base(view)
    {
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {

        SelectedEffect selectedEffect =
            _view.AskUserToChooseBetweenTakingACardFromYourArsenalOrRingside(
                _currentPlayer.GetSuperStarName());
        switch (selectedEffect)
        {
            case SelectedEffect.TakeCardFromArsenal:
                ShuffleFromArsenalToHand shuffleFromArsenalToHand =
                    new ShuffleFromArsenalToHand(_view, pretendedNumberCardsToShuffle: 1);
                shuffleFromArsenalToHand.ApplyEffect(currentPlay);
                break;
            case SelectedEffect.TakeCardFromRingside:
                ShuffleFromRingsideToHand shuffleFromRingsideToHand =
                    new ShuffleFromRingsideToHand(_view, pretendedNumberCardsToShuffle: 1);
                shuffleFromRingsideToHand.ApplyEffect(currentPlay);
                break;
        }
    }
}