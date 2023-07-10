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
        HandlePreEffects(attackingCard);
        HandleReversalsIfPossible(_playManager, attackingCard);
        PlayManeuverOrActionCard(attackingCard);
    }

    private Card SetAttackingCardWithPlayedType(int selectedCardIndex)
    {
        IndexedCard indexedCardIndexInHandAndAttackingCard =
            _userAsker.ListOfIndexedCardsOfPlayableCards[selectedCardIndex];
        Card attackingCard = indexedCardIndexInHandAndAttackingCard.Card;
        attackingCard.PlayedType = _userAsker.TypesOfPlayableCards[selectedCardIndex];
        _currentPlay.SetAttackingIndexedCard(indexedCardIndexInHandAndAttackingCard);
        return attackingCard;
    }

    private void HandleReversalsIfPossible(PlayManager gamePlayManager, Card attackingCard)
    {
        List<IndexedCard> validReversals =
            _notCurrentPlayer.GetReversalIndexedCardsFromHand(gamePlayManager);
        if (CanBeginReversalPlay(attackingCard, validReversals))
            BeginReversalPlay(validReversals, attackingCard);
    }

    private static bool CanBeginReversalPlay(Card attackingCard, List<IndexedCard> validReversals)
    {
        return validReversals.Count > 0 && attackingCard.CanBeReversed;
    }

    private void PlayManeuverOrActionCard(Card attackingCard)
    {
        if (attackingCard.PlayedType is not ("Maneuver" or "Action")) return;
        HandleEffects(_view, _game, attackingCard);
        if (!_game.IsGameOver() && attackingCard.PlayedType != "Reversed") 
            _cardDamageController.BeginApplicationOfCardDamage(_playManager);
    }

    private void BeginReversalPlay(List<IndexedCard> validReversals, Card attackingCard)
    {
        List<string> validReversalsInString =
            PlayableCardsFormatter.GetReversalCards(validReversals);
        int selectedReversalIndex =
            _view.AskUserToSelectAReversal(_notCurrentPlayer.GetSuperStarName(),
                validReversalsInString);
        if (selectedReversalIndex == -1) return;
        HandleSelectedReversal(validReversals, attackingCard, selectedReversalIndex);
    }

    private void HandleSelectedReversal(List<IndexedCard> validReversals, Card attackingCard,
        int selectedReversalIndex)
    {
        IndexedCard indexedCardIndexInHandAndReverseCard = validReversals[selectedReversalIndex];
        _currentPlay.SetReversalIndexedCard(indexedCardIndexInHandAndReverseCard);
        Card selectedReversalCard = indexedCardIndexInHandAndReverseCard.Card;
        SetPlayedTypesAndPlayedFromWhenACardIsReversed(attackingCard, selectedReversalCard);
        HandleEffects(_view, _game, selectedReversalCard);
        _currentPlay.SwapCurrentAndNotCurrentPlayer();
        _cardDamageController.BeginApplicationOfCardDamage(_playManager);
    }

    private static void SetPlayedTypesAndPlayedFromWhenACardIsReversed(Card attackingCard, Card reversalCard)
    {
        attackingCard.PlayedType = "Reversed";
        reversalCard.PlayedFrom = "Hand";
        reversalCard.PlayedType = "Reversal";
    }

    public static void HandleEffects(View view, Game game, Card card)
    {
        ComplexEffect complexEffect = EffectAssigner.AssignEffect(view, game, card);
        complexEffect.ApplyEffect(game.GetCurrentPlay());
    }

    private static void HandlePreEffects(Card card)
    {
        EffectAssigner.AssignPreEffects(card);
    }
}