using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards.CardEffects.GeneralEffects.MovementEffects;

public class MoveSpecificCardFromRingSideToArsenal : Effect
{
    private readonly Player _playerThatWillRecover;
    private readonly string _cardTitleToRecover;
    private bool _playerFoundTheCard;
    private string _cardLocation;
    private int _cardIndex;

    public MoveSpecificCardFromRingSideToArsenal(View view, Player playerThatWillRecover, 
        string cardTitleToRecover) : base(view)
    {
        _playerThatWillRecover = playerThatWillRecover;
        _cardTitleToRecover = cardTitleToRecover;
        _playerFoundTheCard = false;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        SearchCardFromLocation(_playerThatWillRecover.GetAllRingsideCards());
        if (!_playerFoundTheCard)
        {
            _view.SayThatPlayerDidntFindTheCard(_playerThatWillRecover.GetSuperStarName());
            SearchCardFromLocation(_playerThatWillRecover.GetAllArsenalCards());
        } 
        if (!_playerFoundTheCard)
        {
            _view.SayThatPlayerDidntFindTheCard(_playerThatWillRecover.GetSuperStarName());
            return;
        }
        MoveCardByLocation();
    }

    private void SearchCardFromLocation(List<Card> cardList)
    {
        string locationName = GetLocationName(cardList);
        PrintCurrentSearchLocation(locationName);
        _cardIndex = FindCardIndex(cardList);
        if (_cardIndex == -1) return;
        _cardLocation = locationName;
        _playerFoundTheCard = true;
    }

    private int FindCardIndex(IReadOnlyList<Card> cardList)
    {
        for (int index = 0; index < cardList.Count; index++)
        {
            Card card = cardList[index];
            if (card.Title == _cardTitleToRecover)
            {
                return index;
            }
        }
        return -1;
    }

    private string GetLocationName(List<Card> cardList)
    {
        if (cardList == _playerThatWillRecover.GetAllRingsideCards())
            return "Ringside";
        if (cardList == _playerThatWillRecover.GetAllArsenalCards())
            return "Arsenal";
        throw new Exception("Ubicación de carta no válida.");
    }

    private void MoveCardByLocation()
    {
        switch (_cardLocation)
        {
            case "Arsenal":
                CardMobilizer.MoveFromArsenalToHandByIndex(_playerThatWillRecover, _cardIndex);
                break;
            case "Ringside":
                CardMobilizer.MoveFromRingsideToHandByIndex(_playerThatWillRecover, _cardIndex);
                break;
        }
        _view.SayThatPlayerFoundTheCardAndPutItIntoHisHand(_playerThatWillRecover.GetSuperStarName());
    }
    
    private void PrintCurrentSearchLocation(string locationName)
    {
        switch (locationName)
        {
            case "Arsenal":
                _view.SayThatPlayerSearchesForTheTargetCardInHisArsenal(
                    _playerThatWillRecover.GetSuperStarName(),
                    _cardTitleToRecover);
                break;
            case "Ringside":
                _view.SayThatPlayerSearchesForTheTargetCardInHisRingside(
                    _playerThatWillRecover.GetSuperStarName(),
                    _cardTitleToRecover);
                break;
        }
    }
}