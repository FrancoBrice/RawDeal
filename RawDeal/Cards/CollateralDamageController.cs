using RawDeal.CardCollections;
using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards;

public class CollateralDamageController : CardDamageController
{
    
    public CollateralDamageController(Game game, View view) : base(game, view)
    {
    }
    
    public void BeginCollateralDamage(int damageAmount)
    {
        if (damageAmount == 0) return;
        SetPlayers(_currentPlay);
        _view.SayThatSuperstarWillTakeSomeDamage(_currentPlayer.GetSuperStarName(), damageAmount);
        CardCollection cardsToBeDamaged = _currentPlayer.GetCardsFromArsenal(damageAmount);
        ViewManager.ShowDamagedCards(_view, cardsToBeDamaged, damageAmount);
        AddCardsToCardToBeDamaged(cardsToBeDamaged);
        ApplyCollateralDamage(damageAmount, cardsToBeDamaged);
    }

    private void ApplyCollateralDamage(int damageAmount, CardCollection cardsToBeDamaged)
    {
        PlayerReceiveDamage(_currentPlayer, cardsToBeDamaged.Count - 1);
        _opponentRanOutOfCards =
            GameEndChecker.PlayerRanOutOfCardsDuringDamage(_currentPlayer, damageAmount);
        if (_opponentRanOutOfCards)
            _view.SayThatPlayerLostDueToSelfDamage(_currentPlayer.GetSuperStarName());
        EndGameIfPlayerRanOutOfCardsAndNotReverse(_notCurrentPlayer);
    }
    
    private static void AddCardsToCardToBeDamaged(CardCollection cardsToBeDamaged)
    {
        for (int index = cardsToBeDamaged.Count - 1; index >= 0; index--)
        {
            Card damagedCard = cardsToBeDamaged.CardList[index];
            cardsToBeDamaged.Add(damagedCard);
        }
    }
}