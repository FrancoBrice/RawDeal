using RawDeal.Cards.CardEffects.GeneralEffects;
using RawDeal.Cards.CardEffects.GeneralEffects.DiscardCards;
using RawDeal.Cards.CardEffects.GeneralEffects.DrawCards;
using RawDeal.Cards.CardEffects.GeneralEffects.EffectsForNextCards;
using RawDeal.Cards.CardEffects.GeneralEffects.MovementEffects;
using RawDeal.Cards.CardEffects.GeneralEffects.SelectableEffects;
using RawDeal.Cards.CardEffects.ReversalsEffects;
using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.Cards.CardEffects;

public static class EffectAssigner
{
    private static Player _currentPlayer;
    private static Player _notCurrentPlayer;
    private static Game _game;
    private static Play _currentPlay;
    private static View _view;

    public static List<Effect> AssignEffect(View view, Game game, Card card)
    {
        SetObjects(view, game);
        List<Effect> assignedEffects = new List<Effect>();
        switch (card.PlayedType)
        {
            case "Maneuver":
                assignedEffects = AssignManeuverEffect();
                break;
            case "Action":
                assignedEffects = AssignActionEffect();
                break;
            case "Reversal":
                assignedEffects = AssignReversalEffect();
                break;
        }

        return assignedEffects;
    }

    private static List<Effect> AssignManeuverEffect()
    {
        Card attackingCard = _currentPlay.AttackingCard;
        List<Effect> effectsAssigned = new List<Effect> { new ManeuverInitialEffect(_view) };
        switch (attackingCard.Title)
        {
            case "Head Butt":
            case "Arm Drag":
            case "Arm Bar":
                effectsAssigned.Add(new MakePlayerDiscardCard(_view, 
                    playerThatMustDiscard: _currentPlayer, numberOfCardToDiscard: 1));
                break;
            case "Bear Hug":
            case "Choke Hold":
            case "Ankle Lock":
            case "Spinning Heel Kick":
            case "Samoan Drop":
            case "Power Slam":
            case "Boston Crab":
            case "Torture Rack":
            case "Figure Four Leg Lock":
            case "Lionsault":
            case "Leaping Knee to the Face":
                effectsAssigned.Add(new MakePlayerDiscardCard(_view,
                    playerThatMustDiscard: _notCurrentPlayer, numberOfCardToDiscard: 1));
                break;
            case "Pump Handle Slam":
            case "Tree of Woe":
                effectsAssigned.Add(new MakePlayerDiscardCard(_view, 
                    playerThatMustDiscard: _notCurrentPlayer, numberOfCardToDiscard: 2));
                break;
            case "Bulldog":
                effectsAssigned.Add(new MakePlayerDiscardCard(_view, 
                    playerThatMustDiscard: _currentPlayer, numberOfCardToDiscard: 1));
                effectsAssigned.Add(new DiscardCardFromOpponentsHand(_view));
                break;
            case "Kick":
            case "Running Elbow Smash":
                effectsAssigned.Add(new TopCardOfArsenalToRingsidePile(_view, _game));
                break;
            case "Double Leg Takedown":
            case "Reverse DDT":
                effectsAssigned.Add(new DrawCardsAskingNumber(_view,
                    playerThatMustDraw: _currentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Headlock Takedown":
            case "Standing Side Headlock":
                effectsAssigned.Add(new PlayerDrawCards(_view,
                    player: _notCurrentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Fisherman's Suplex":
                effectsAssigned.Add(new TopCardOfArsenalToRingsidePile(_view, _game));
                effectsAssigned.Add(new DrawCardsAskingNumber(_view,
                    playerThatMustDraw: _currentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Press Slam":
            case "DDT":
                effectsAssigned.Add(new TopCardOfArsenalToRingsidePile(_view, _game));
                effectsAssigned.Add(new MakePlayerDiscardCard(_view, 
                    playerThatMustDiscard: _notCurrentPlayer, numberOfCardToDiscard: 2));
                break;
            case "Guillotine Stretch":
                effectsAssigned.Add(new MakePlayerDiscardCard(_view,  
                    playerThatMustDiscard: _notCurrentPlayer, numberOfCardToDiscard: 1));
                effectsAssigned.Add(new DrawCardsAskingNumber(_view, 
                    playerThatMustDraw: _currentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Chicken Wing":
                effectsAssigned.Add(new ShuffleFromRingsideToArsenal(_view,
                    pretendedNumberCardsToShuffle: 2));
                break;

            case "Back Body Drop":
                effectsAssigned.Add(new DrawCardsOrForceOpponentDiscard(_view));
                break;
            case "Haymaker":
                NextCardDamageBonusByTypeAndSubtype haymakerEffect =
                    new NextCardDamageBonusByTypeAndSubtype(_view, type: "Maneuver", 
                    subtype: "Strike", bonus: 1);

                effectsAssigned.Add(
                    new EffectForTheRestOfTheTurn(_view, _currentPlay, haymakerEffect));
                break;
            case "Superkick":
                effectsAssigned.Add(new DamageBonusIfPlayedAfterSpecificDamageAndType(_view, 
                    damageBonus: 5, minimumDamage: 5, 
                    typeOfPreviousCardThatAppliesBonus: "Maneuver"));
                break;
            case "Clothesline":
            case "Atomic Drop":
                NextCardDamageBonusByTypeAndSubtype damageBonusEffect =
                    new NextCardDamageBonusByTypeAndSubtype(_view, type: "Maneuver", subtype: "All",
                        bonus: 2);

                _currentPlay.AddPendingEffect(pendingEffect: damageBonusEffect);
                break;
            case "Snap Mare":
                NextCardDamageBonusByTypeAndSubtype snapMaraEffect =
                    new NextCardDamageBonusByTypeAndSubtype(_view, type: "Maneuver", 
                    subtype: "Strike", bonus: 2);
                _currentPlay.AddPendingEffect(pendingEffect: snapMaraEffect);
                break;
        }

        return effectsAssigned;
    }

    private static List<Effect> AssignActionEffect()
    {
        Card card = _currentPlay.AttackingCard;
        List<Effect> effectsAssigned = new List<Effect>();
        _view.SayThatPlayerSuccessfullyPlayedACard();
        switch (card.Title)
        {
            case "Jockeying for Position":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                JockeyingForPositionSelectableEffect selectableEffect =
                    new JockeyingForPositionSelectableEffect(_view);
                selectableEffect.ApplyEffect(currentPlay: _currentPlay);
                break;
            case "Offer Handshake":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                effectsAssigned.Add(new DrawCardsAskingNumber(_view, 
                    _currentPlayer, numberOfCardsToDraw: 3));
                effectsAssigned.Add(new MakePlayerDiscardCard(_view, 
                    _currentPlayer, numberOfCardToDiscard: 1));
                break;
            case "Irish Whip":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                NextCardDamageBonusByTypeAndSubtype irishWhipBonusEffect =
                    new NextCardDamageBonusByTypeAndSubtype(_view, type: "Maneuver", 
                        subtype: "Strike", bonus: 5);
                effectsAssigned.Add(new EffectForNextCard(_view, irishWhipBonusEffect));
                break;
            case "I Am the Game":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                NextCardDamageBonusByTypeAndSubtype iAmTheGameBonusEffect =
                    new NextCardDamageBonusByTypeAndSubtype(_view, type: "Maneuver",
                        subtype: "All", bonus: 3);
                effectsAssigned.Add(new EffectForTheRestOfTheTurn(_view, 
                        _currentPlay, effect: iAmTheGameBonusEffect));
                effectsAssigned.Add(new DrawCardsOrForceOpponentDiscard(_view));
                break;
            case "Y2J":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                SelectedEffect selectedEffectY2J =
                    _view.AskUserToChooseBetweenDrawingOrForcingOpponentToDiscardCards(
                        superstarName: _currentPlayer.GetSuperStarName());
                if (selectedEffectY2J == SelectedEffect.DrawCards)
                    effectsAssigned.Add(new DrawCardsAskingNumber(_view,
                        _currentPlayer, numberOfCardsToDraw: 5));
                else if (selectedEffectY2J == SelectedEffect.ForceOpponentToDiscard)
                    effectsAssigned.Add(new MakePlayerDiscardCard(_view,
                        _notCurrentPlayer, numberOfCardToDiscard: 5));
                break;
            case "Roll Out of the Ring":
                int maximumNumberOfCardsToDiscard;
                if (_currentPlayer.GetHandSize() - 1 == 0) maximumNumberOfCardsToDiscard = 0;
                else if (_currentPlayer.GetHandSize() - 1 == 1) maximumNumberOfCardsToDiscard = 1;
                else maximumNumberOfCardsToDiscard = 2;
                int numberOfCards =
                    _view.AskHowManyCardsToDiscard(superstarName: _currentPlayer.GetSuperStarName(),
                        maximumNumberOfCardsToDiscard);
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                effectsAssigned.Add(new MakePlayerDiscardCard(_view, 
                    _currentPlayer, numberOfCards));
                effectsAssigned.Add(new ShuffleFromRingsideToHand(_view, 
                    pretendedNumberCardsToShuffle: numberOfCards));
                break;
            case "Spit At Opponent":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                effectsAssigned.Add(new MakePlayerDiscardCard(_view,
                 _currentPlayer, numberOfCardToDiscard: 1));
                effectsAssigned.Add(new MakePlayerDiscardCard(_view,
                 _notCurrentPlayer, numberOfCardToDiscard: 4));
                break;
            case "Recovery":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                effectsAssigned.Add(new ShuffleFromRingsideToArsenal(_view, 
                    pretendedNumberCardsToShuffle: 2));
                effectsAssigned.Add(new PlayerDrawCards(_view, 
                    _currentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Puppies! Puppies!":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                effectsAssigned.Add(new ShuffleFromRingsideToArsenal(_view, 
                    pretendedNumberCardsToShuffle: 5));
                effectsAssigned.Add(new PlayerDrawCards(_view, 
                    _currentPlayer, numberOfCardsToDraw: 2));
                break;
            case "Chop":
            case "Arm Bar Takedown":
            case "Collar & Elbow Lockup":
            case "Undertaker's Tombstone Piledriver":
                effectsAssigned.Add(new DiscardToDrawWithoutDamage(_view));
                break;
        }

        return effectsAssigned;
    }

    private static List<Effect> AssignReversalEffect()
    {
        Card selectedCard = _currentPlay.ReversalCard;
        List<Effect> effectsAssigned = new List<Effect>();
        switch (selectedCard.Title)
        {
            case "Step Aside":
            case "Escape Move":
            case "Break the Hold":
            case "No Chance in Hell":
            case "Cross Body Block":
            case "Belly to Belly Suplex":
            case "Vertical Suplex":
            case "Belly to Back Suplex":
            case "Ensugiri":
            case "Drop Kick":
            case "Double Arm DDT":
            case "Elbow to the Face":
            case "Shoulder Block":
            case "Spear":
                effectsAssigned.Add(new ReversalSimple(_view));
                break;
            case "Rolling Takedown":
            case "Knee to the Gut":
                effectsAssigned.Add(new ReversalSimple(_view));
                effectsAssigned.Add(new SetDamageFromAttackingCardToReversal(_view));
                break;
            case "Manager Interferes":
                if (selectedCard.PlayedFrom == "Hand")
                {
                    effectsAssigned.Add(new ReversalSimple(_view));
                    effectsAssigned.Add(new PlayerDrawCards(_view,
                        _notCurrentPlayer, numberOfCardsToDraw: 1));
                }
                break;
            case "Chyna Interferes":
                if (selectedCard.PlayedFrom == "Hand")
                {
                    effectsAssigned.Add(new ReversalSimple(_view));
                    effectsAssigned.Add(new PlayerDrawCards(_view, 
                        _notCurrentPlayer, numberOfCardsToDraw: 2));
                }
                break;
            case "Clean Break":
                effectsAssigned.Add(new ReversalSimple(_view));
                effectsAssigned.Add(new MakePlayerDiscardCard(_view, _currentPlayer, 
                    numberOfCardToDiscard:4));
                effectsAssigned.Add(new PlayerDrawCards(_view, _notCurrentPlayer, 
                    numberOfCardsToDraw:1));
                break;
            case "Jockeying for Position":
                effectsAssigned.Add(new ReversalSimple(_view));
                effectsAssigned.Add(new EndPlayEffect(_view));
                effectsAssigned.Add(new JockeyingForPositionSelectableEffect(_view));
                break;
            case "Irish Whip":
                effectsAssigned.Add(new ReversalSimple(_view));
                _currentPlay.AddPendingEffect(new NextCardDamageBonusByTypeAndSubtype(_view, 
                    type: "Maneuver", subtype: "Strike", bonus:5));
                break;
            case "Facebuster":
                effectsAssigned.Add(new ReversalSimple(_view));
                effectsAssigned.Add(new DrawCardsAskingNumber(_view, 
                    _notCurrentPlayer, numberOfCardsToDraw: 2));
                break;
            case "Lou Thesz Press":
                effectsAssigned.Add(new ReversalSimple(_view));
                effectsAssigned.Add(new DrawCardsAskingNumber(_view, 
                    _notCurrentPlayer, numberOfCardsToDraw: 1));
                break;
        }

        return effectsAssigned;
    }

    private static void SetObjects(View view, Game game)
    {
        _game = game;
        _view = view;
        _currentPlay = _game.GetCurrentPlay();
        SetPlayers();
    }

    private static void SetPlayers()
    {
        Dictionary<string, Player> playersDictionary =
            _game.GetDictionaryOfCurrentAndNotCurrentPlayer();
        _currentPlayer = playersDictionary["CurrentPlayer"];
        _notCurrentPlayer = playersDictionary["NotCurrentPlayer"];
    }
}