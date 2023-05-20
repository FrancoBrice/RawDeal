using RawDeal.Cards.CardEffects;
using RawDeal.GameLogic;
using RawDeal.Tools;
using RawDealView;

namespace RawDeal.Cards;

public class CardPlayer
{
    private Game _game;
    private Player _currentPlayer;
    private Player _notCurrentPlayer;
    private Play _currentPlay;
    private List<string> _typesOfPlayableCards;
    private List<(int, Card)> _listOfTuplesOfPlayableCards;
    private View _view;
    private ViewManager _viewManager;
    private UserAsker _userAsker;
    private TupleManager _tupleManager;
    private PlayableCardsFormatter _playableCardsFormatter;
    private CardDamager _cardDamager;

    public CardPlayer(Game game, View view)
    {
        _game = game;
        _currentPlayer = game.CurrentPlayer;
        _notCurrentPlayer = game.NotCurrentPlayer;
        _currentPlay = game.CurrentPlay;
        _view = view;
        _viewManager = new ViewManager(_view);
        _userAsker = new UserAsker(_view);
        _tupleManager = new TupleManager();
        _playableCardsFormatter = new PlayableCardsFormatter();
        _cardDamager = new CardDamager(_game, _view);
    }
    
    public void PlayCard() 
    {
        int selectedCardIndex = _userAsker.SelectACard(_currentPlayer);
        if (selectedCardIndex == -1) return;
        (int, Card) tupleWithIndexInHandAndAttackingCard = _userAsker.ListOfTuplesOfPlayableCards[selectedCardIndex];
        Card attackingCard = _tupleManager.ExtractCard(tupleWithIndexInHandAndAttackingCard);
        attackingCard.PlayedType = _userAsker.TypesOfPlayableCards[selectedCardIndex];
        _currentPlay.SetAttackingCardTuple(tupleWithIndexInHandAndAttackingCard);
        _viewManager.SayPlayerIsTryingToPlayCard(_currentPlay);
        List<(int, Card)> validReversals = _notCurrentPlayer.GetReversalTuplesFromHand(attackingCard);
        if (validReversals.Count > 0) 
        {
            HandleReversals(validReversals, attackingCard);
        }
        PlayManeuverOrActionCard(attackingCard);
    }

    private void PlayManeuverOrActionCard(Card attackingCard)
    {
        EffectAsigner effectAssigner = new EffectAsigner(_game.GetDictionaryOfCurrentAndNotCurrentPlayer(), _view);
        switch (attackingCard.PlayedType)
        {
            case "Maneuver":
                Effect maneuverEffect = effectAssigner.AssignManeuverEffect(attackingCard);
                maneuverEffect.ApplyEffect(_currentPlay);
                _cardDamager.ApplyCardDamage();
                break;
            case "Action":
                Effect actionEffect = effectAssigner.AssignActionEffect(attackingCard);
                actionEffect.ApplyEffect(_currentPlay);
                break;
        }
    }

    private void HandleReversals(List<(int, Card)> validReversals, Card attackingCard) 
    {
        List<string> validReversalsInString = _playableCardsFormatter.GetReversalCards(validReversals);
        int selectedReversalIndex = _view.AskUserToSelectAReversal(_notCurrentPlayer.GetSuperStarName(), validReversalsInString);
        if (selectedReversalIndex == -1) return;
        attackingCard.PlayedType = "Reversed";
        (int, Card) tupleWithIndexInHandAndReverseCard = validReversals[selectedReversalIndex];
        _currentPlay.SetReversalCardTuple(tupleWithIndexInHandAndReverseCard);
        Card selectedReversalCard = _tupleManager.ExtractCard(tupleWithIndexInHandAndReverseCard);
        selectedReversalCard.PlayedType = "Reversal";
        selectedReversalCard.SetReversalTypeAndSubtype();
        HandleReversalEffects();
        _currentPlay.SwapCurrentAndNotCurrentPlayer();
        _cardDamager.ApplyCardDamage();
    }

    private void HandleReversalEffects()
    {
        EffectAsigner effectAssigner = new EffectAsigner(_game.GetDictionaryOfCurrentAndNotCurrentPlayer(), _view);
        Effect assignedEffect = effectAssigner.AssignReversalEffect(_currentPlay);
        assignedEffect.ApplyEffect(_currentPlay);
    }


}