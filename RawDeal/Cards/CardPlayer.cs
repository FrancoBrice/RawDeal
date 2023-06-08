using RawDeal.Cards.CardEffects;
using RawDeal.GameLogic;
using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards;

public class CardPlayer
{
    private readonly CardDamager _cardDamager;
    private readonly Play _currentPlay;
    private Player _currentPlayer;
    private readonly Game _game;
    private List<(int, Card)> _listOfTuplesOfPlayableCards;
    private readonly Player _notCurrentPlayer;
    private readonly UserAsker _userAsker;
    private readonly View _view;

    public CardPlayer(Game game, View view)
    {
        _game = game;
        _currentPlayer = game.CurrentPlayer;
        _notCurrentPlayer = game.NotCurrentPlayer;
        _currentPlay = game.CurrentPlay;
        _view = view;
        _userAsker = new UserAsker(_view);
        _cardDamager = new CardDamager(_game, _view);
    }

    public void PlayCard(PlayManager gamePlayManager)
    {
        int selectedCardIndex = _userAsker.SelectACard(gamePlayManager);
        if (selectedCardIndex == -1) return;
        Card attackingCard = SetAttackingCardWithPlayedType(selectedCardIndex);
        ViewManager.SayPlayerIsTryingToPlayCard(_view, _currentPlay);
        HandleReversalsIfPosible(attackingCard);
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

    private void HandleReversalsIfPosible(Card attackingCard)
    {
        List<(int, Card)> validReversals =
            _notCurrentPlayer.GetReversalTuplesFromHand(_game.PlayManager);
        if (validReversals.Count > 0 && attackingCard.CanBeReversed)
            BeginReversalPlay(validReversals, attackingCard);
    }

    private void PlayManeuverOrActionCard(Card attackingCard)
    {
        if (attackingCard.PlayedType is not ("Maneuver" or "Action")) return;
        HandleEffects(_game, attackingCard);
        if (!_game.GameIsOver) _cardDamager.ApplyCardDamage();
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
        HandleEffects(_game, selectedReversalCard);
        _currentPlay.SwapCurrentAndNotCurrentPlayer();
        _cardDamager.ApplyCardDamage();
    }

    private static void SetPlayedTypesAndPlayedFrom(Card attackingCard, Card reversalCard)
    {
        attackingCard.PlayedType = "Reversed";
        reversalCard.PlayedFrom = "Hand";
        reversalCard.PlayedType = "Reversal";
    }

    public static void HandleEffects(Game game, Card card)
    {
        List<Effect> assignedEffects = EffectAssigner.AssignEffect(game, card);
        EffectsApplier.ApplyAssignedEffects(game, assignedEffects);
    }
}