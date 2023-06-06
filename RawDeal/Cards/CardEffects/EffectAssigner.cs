using System.Reflection.Metadata;
using RawDeal.Cards.CardEffects.ActionEffects;
using RawDeal.Cards.CardEffects.GeneralEffects;
using RawDeal.Cards.CardEffects.ReversalsEffects;
using RawDeal.Cards.CardEffects.ReversalsEffects.SpecificCards;
using RawDeal.Cards.CardEffects.SpecificCardEffects;
using RawDeal.GameLogic;
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
    
    public static List<Effect> AssignManeuverEffect(Game game)
    {
        SetObjects(game);
        Card attackingCard = _currentPlay.AttackingCard; 
        List<Effect> effectsAssigned = new List<Effect> { new ManeuverInitialEffect(_view) };
        switch (attackingCard.Title)
        {
            case "Head Butt":
            case "Arm Drag":
            case "Arm Bar":
                effectsAssigned.Add(new MakePlayerDiscardCard(_game,  playerThatMustDiscard: _currentPlayer, numberOfCardToDiscard: 1));
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
                effectsAssigned.Add(new MakePlayerDiscardCard(_game, playerThatMustDiscard: _notCurrentPlayer, numberOfCardToDiscard: 1));
                break;
            case "Pump Handle Slam":
            case "Tree of Woe":
                effectsAssigned.Add(new MakePlayerDiscardCard(_game, playerThatMustDiscard: _notCurrentPlayer, numberOfCardToDiscard: 2));
                break;
            case "Bulldog":
                effectsAssigned.Add(new MakePlayerDiscardCard(_game, playerThatMustDiscard: _currentPlayer, numberOfCardToDiscard: 1));
                effectsAssigned.Add( new DiscardCardFromOpponentsHand(view: _view));
                break;
            case "Kick":
            case "Running Elbow Smash":
                effectsAssigned.Add( new TopCardOfArsenalToRingsidePile(_game));
                break;
            case "Double Leg Takedown":
            case "Reverse DDT":
                effectsAssigned.Add(new DrawCardsAskingNumber(_view, playerThatMustDraw: _currentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Headlock Takedown":
            case "Standing Side Headlock":
                effectsAssigned.Add(new DrawCards(_view, playerThatMustDraw: _notCurrentPlayer,
                    numberOfCardsToDraw: 1));
                break;
            case "Fisherman's Suplex":
                effectsAssigned.Add(new TopCardOfArsenalToRingsidePile(_game));
                effectsAssigned.Add(new DrawCardsAskingNumber(_view, playerThatMustDraw: _currentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Press Slam":
            case "DDT":
                effectsAssigned.Add( new TopCardOfArsenalToRingsidePile(_game));
                effectsAssigned.Add(new MakePlayerDiscardCard(_game, _notCurrentPlayer,  numberOfCardToDiscard: 2));
                break;
            case "Guillotine Stretch":
                effectsAssigned.Add(new MakePlayerDiscardCard(_game, playerThatMustDiscard: _notCurrentPlayer, numberOfCardToDiscard: 1));
                effectsAssigned.Add( new DrawCardsAskingNumber(_view, playerThatMustDraw: _currentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Chicken Wing":
                effectsAssigned.Add(new ShuffleFromRingsideToArsenal(_view, pretendedNumberCardsToShuffle: 2));
                break;

            case "Back Body Drop":
                effectsAssigned.Add(new DrawCardsOrForceOpponentDiscardSelectableEffect(_game));
                break;
            case "Haymaker":
                var haymakerEffect = new NextCardDamageBonusByTypeAndSubtype(view: _view);
                haymakerEffect.SetDamageBonus(bonus: 1);
                haymakerEffect.SetTypeAndSubtypeThatAppliesBonus(type: "Maneuver", subtype: "Strike");
                effectsAssigned.Add(new EffectForTheRestOfTheTurn(_view, _currentPlay, haymakerEffect));
                break;
            case "Superkick":
                effectsAssigned.Add(new DamageBonusIfPlayedAfterSpecificDamageAndType(_view, damageBonus: 5, minimumDamage: 5, typeOfPreviousCardThatAppliesBonus: "Maneuver"));
                break;
            case "Clothesline":
            case "Atomic Drop":
                var damageBonusEffect = new NextCardDamageBonusByTypeAndSubtype(view: _view);
                damageBonusEffect.SetDamageBonus(bonus: 2);
                damageBonusEffect.SetTypeAndSubtypeThatAppliesBonus(type: "Maneuver", subtype: "All");
                _currentPlay.AddPendingEffect(damageBonusEffect);
                break;
            case "Snap Mare":
                var snapMaraEffect = new NextCardDamageBonusByTypeAndSubtype(view: _view);
                snapMaraEffect.SetDamageBonus(bonus: 2);
                snapMaraEffect.SetTypeAndSubtypeThatAppliesBonus(type: "Maneuver", subtype: "Strike");
                _currentPlay.AddPendingEffect(snapMaraEffect);
                break;
        }
        return effectsAssigned;
    }

    public static List<Effect> AssignActionEffect(Game game)
    {
        SetObjects(game);
        Card card = _currentPlay.AttackingCard;
        List<Effect> effectsAssigned = new List<Effect>();
        _view.SayThatPlayerSuccessfullyPlayedACard();
        switch (card.Title)
        {
            case "Jockeying for Position":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                JockeyingForPositionSelectableEffect selectableEffect = new JockeyingForPositionSelectableEffect(_view);
                selectableEffect.ApplyEffect(_currentPlay);
                break;
            case "Offer Handshake":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                effectsAssigned.Add(new DrawCardsAskingNumber(_view, _currentPlayer, numberOfCardsToDraw: 3));
                effectsAssigned.Add(new MakePlayerDiscardCard(_game, _currentPlayer, numberOfCardToDiscard: 1));
                break;
            case "Irish Whip":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                var irishWhipBonusEffect = new NextCardDamageBonusByTypeAndSubtype(_view);
                irishWhipBonusEffect.SetDamageBonus(bonus: 5);
                irishWhipBonusEffect.SetTypeAndSubtypeThatAppliesBonus("Maneuver","Strike");
                effectsAssigned.Add(new EffectForNextCard(_view, irishWhipBonusEffect));
                break;
            case "I Am the Game":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                var iAmTheGameBonusEffect = new NextCardDamageBonusByTypeAndSubtype(_view);
                iAmTheGameBonusEffect.SetDamageBonus(bonus: 3);
                iAmTheGameBonusEffect.SetTypeAndSubtypeThatAppliesBonus("Maneuver", "All");
                effectsAssigned.Add(new EffectForTheRestOfTheTurn(_view, _currentPlay, iAmTheGameBonusEffect));
                effectsAssigned.Add(new DrawCardsOrForceOpponentDiscardSelectableEffect(_game));
                break;
            case "Y2J":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                var selectedEffectY2J =
                    _view.AskUserToChooseBetweenDrawingOrForcingOpponentToDiscardCards(_currentPlayer.GetSuperStarName());
                if (selectedEffectY2J == SelectedEffect.DrawCards)
                    effectsAssigned.Add(new DrawCardsAskingNumber(_view, playerThatMustDraw: _currentPlayer, numberOfCardsToDraw: 5));
                else if (selectedEffectY2J == SelectedEffect.ForceOpponentToDiscard)
                    effectsAssigned.Add( new MakePlayerDiscardCard(_game, playerThatMustDiscard: _notCurrentPlayer, numberOfCardToDiscard: 5));
                break;
            case "Roll Out of the Ring":
                int maximumNumberOfCardsToDiscard;
                if (_currentPlayer.GetHandSize() - 1 == 0) maximumNumberOfCardsToDiscard = 0;
                else if (_currentPlayer.GetHandSize() - 1 == 1) maximumNumberOfCardsToDiscard = 1;
                else maximumNumberOfCardsToDiscard = 2;
                int numberOfCards =
                    _view.AskHowManyCardsToDiscard(_currentPlayer.GetSuperStarName(), maximumNumberOfCardsToDiscard);
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                effectsAssigned.Add(new DiscardCardsWithSelection(_view, _currentPlayer, numberOfCardsToDiscard: numberOfCards));
                effectsAssigned.Add(new ShuffleFromRingsideToHand(_view, numberOfCards));
                break;
            case "Spit At Opponent":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                effectsAssigned.Add(new MakePlayerDiscardCard( _game, _currentPlayer, numberOfCardToDiscard: 1));
                effectsAssigned.Add(new MakePlayerDiscardCard(_game, _notCurrentPlayer, numberOfCardToDiscard: 4));
                break;
            case "Recovery":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                effectsAssigned.Add(new ShuffleFromRingsideToArsenal(_view, pretendedNumberCardsToShuffle: 2));
                effectsAssigned.Add(new DrawCards(_view, playerThatMustDraw: _currentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Puppies! Puppies!":
                effectsAssigned.Add(new MoveCardFromHandToRingArea(_view));
                effectsAssigned.Add(new ShuffleFromRingsideToArsenal(_view, pretendedNumberCardsToShuffle: 5));
                effectsAssigned.Add(new DrawCards(_view, playerThatMustDraw: _currentPlayer, numberOfCardsToDraw: 2));
                break;
            case "Chop":
            case "Arm Bar Takedown":
            case "Collar & Elbow Lockup":
            case "Undertaker's Tombstone Piledriver":
                effectsAssigned.Add( new DiscardToDrawWithoutDamage(_view));
                break;
        }
        return effectsAssigned;
    }

    public static List<Effect> AssignReversalEffect(Game game)
    {
        SetObjects(game);
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
                    effectsAssigned.Add(new ManagerInterferesEffect(_view));
                }
                break;
            case "Chyna Interferes":
                if (selectedCard.PlayedFrom == "Hand")
                {
                    effectsAssigned.Add(new ReversalSimple(_view));
                    effectsAssigned.Add(new DrawCards(_view, _notCurrentPlayer, numberOfCardsToDraw: 2));
                }
                break;
            case "Clean Break":
                effectsAssigned.Add(new CleanBreakEffect(_view));
                break;
            case "Jockeying for Position":
                effectsAssigned.Add(new ReversalSimple(_view));
                effectsAssigned.Add(new EndPlayEffect(_view));
                effectsAssigned.Add(new JockeyingForPositionSelectableEffect(_view));
                break;
            case "Irish Whip":
                effectsAssigned.Add(new ReversalSimple(_view));
                NextCardDamageBonusByTypeAndSubtype irishBonusEffect = new NextCardDamageBonusByTypeAndSubtype(_view);
                irishBonusEffect.SetDamageBonus(bonus: 5);
                irishBonusEffect.SetTypeAndSubtypeThatAppliesBonus("Maneuver", "Strike");
                _currentPlay.AddPendingEffect(irishBonusEffect);
                break;
            case "Facebuster":
                effectsAssigned.Add(new ReversalSimple(_view));
                effectsAssigned.Add(new DrawCardsAskingNumber(_view, _notCurrentPlayer, numberOfCardsToDraw: 2));
                break;
            case "Lou Thesz Press":
                effectsAssigned.Add(new ReversalSimple(_view));
                effectsAssigned.Add(new DrawCardsAskingNumber(_view, _notCurrentPlayer, numberOfCardsToDraw: 1));
                break;
        }

        return effectsAssigned;
    }

    private static void SetObjects(Game game)
    {
        _game = game;
        _view = game.ViewObject;
        _currentPlay = _game.CurrentPlay;
        SetPlayers();
    }
    
    private static void SetPlayers()
    {
        Dictionary<string, Player> playersDictionary = _game.GetDictionaryOfCurrentAndNotCurrentPlayer();
        _currentPlayer = playersDictionary["CurrentPlayer"];
        _notCurrentPlayer = playersDictionary["NotCurrentPlayer"];
    }


}

    
