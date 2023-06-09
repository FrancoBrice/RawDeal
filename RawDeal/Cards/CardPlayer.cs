using RawDeal.Cards.CardEffects;
using RawDeal.GameLogic;
using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards;

public class CardPlayer
{
    private readonly CardDamageController _cardDamageController;
    private readonly Play _currentPlay;
    private Player _currentPlayer;
    private readonly Game _game;
    private List<(int, Card)> _listOfTuplesOfPlayableCards;
    private readonly Player _notCurrentPlayer;
    private readonly UserAsker _userAsker;
    private readonly View _view;
    private PlayManager _playManager;

    public CardPlayer(Game game, View view)
    {
        _game = game;
        _currentPlayer = game.CurrentPlayer;
        _notCurrentPlayer = game.NotCurrentPlayer;
        _currentPlay = game.GetCurrentPlay();
        _view = view;
        _userAsker = new UserAsker(_view);
        _cardDamageController = new CardDamageController(_game, _view);
    }

    public void PlayCard(PlayManager gamePlayManager)
    {
        _playManager = gamePlayManager;
        int selectedCardIndex = _userAsker.SelectACard(_playManager);
        if (selectedCardIndex == -1) return;
        Card attackingCard = SetAttackingCardWithPlayedType(selectedCardIndex);
        ViewManager.SayPlayerIsTryingToPlayCard(_view, _currentPlay);
        HandleReversalsIfPossible(_playManager, attackingCard);
        PlayManeuverOrActionCard(attackingCard);
    }

    private Card SetAttackingCardWithPlayedType(int selectedCardIndex)
    {
        (int, Card) tupleWithIndexInHandAndAttackingCard =
            _userAsker.ListOfTuplesOfPlayableCards[selectedCardIndex];
        Card attackingCard = TupleManager.ExtractCard(tupleWithIndexInHandAndAttackingCard);
        attackingCard.PlayedType = _userAsker.TypesOfPlayableCards[selectedCardIndex];
        _currentPlay.SetAttackingCardTuple(tupleWithIndexInHandAndAttackingCard);
        return attackingCard;
    }

    private void HandleReversalsIfPossible(PlayManager gamePlayManager, Card attackingCard)
    {
        List<(int, Card)> validReversals =
            _notCurrentPlayer.GetReversalTuplesFromHand(gamePlayManager);
        if (validReversals.Count > 0 && attackingCard.CanBeReversed)
            BeginReversalPlay(validReversals, attackingCard);
    }

    private void PlayManeuverOrActionCard(Card attackingCard)
    {
        if (attackingCard.PlayedType is not ("Maneuver" or "Action")) return;
        HandleEffects(_view, _game, attackingCard);
        if (!_game.IsGameOver()) _cardDamageController.ApplyCardDamage(_playManager);
    }

    private void BeginReversalPlay(List<(int, Card)> validReversals, Card attackingCard)
    {
        List<string> validReversalsInString =
            PlayableCardsFormatter.GetReversalCards(validReversals);
        int selectedReversalIndex =
            _view.AskUserToSelectAReversal(_notCurrentPlayer.GetSuperStarName(),
                validReversalsInString);
        if (selectedReversalIndex == -1) return;
        HandleSelectedReversal(validReversals, attackingCard, selectedReversalIndex);
    }

    private void HandleSelectedReversal(List<(int, Card)> validReversals, Card attackingCard,
        int selectedReversalIndex)
    {
        (int, Card) tupleWithIndexInHandAndReverseCard = validReversals[selectedReversalIndex];
        _currentPlay.SetReversalCardTuple(tupleWithIndexInHandAndReverseCard);
        Card selectedReversalCard = TupleManager.ExtractCard(tupleWithIndexInHandAndReverseCard);
        SetPlayedTypesAndPlayedFrom(attackingCard, selectedReversalCard);
        HandleEffects(_view, _game, selectedReversalCard);
        _currentPlay.SwapCurrentAndNotCurrentPlayer();
        _cardDamageController.ApplyCardDamage(_playManager);
    }

    private static void SetPlayedTypesAndPlayedFrom(Card attackingCard, Card reversalCard)
    {
        attackingCard.PlayedType = "Reversed";
        reversalCard.PlayedFrom = "Hand";
        reversalCard.PlayedType = "Reversal";
    }

    public static void HandleEffects(View view, Game game, Card card)
    {
        List<Effect> assignedEffects = EffectAssigner.AssignEffect(view, game, card);
        EffectsApplier.ApplyAssignedEffects(game, assignedEffects);
    }
}