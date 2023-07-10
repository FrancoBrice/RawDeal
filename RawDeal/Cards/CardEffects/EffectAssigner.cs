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

    public static ComplexEffect AssignEffect(View view, Game game, Card card)
    {
        SetObjects(view, game);
        ComplexEffect complexEffect = new ComplexEffect(_view, _game);
        switch (card.PlayedType)
        {
            case "Maneuver":
                complexEffect = AssignManeuverEffect();
                break;
            case "Action":
                complexEffect = AssignActionEffect();
                break;
            case "Reversal":
                complexEffect = AssignReversalEffect();
                break;
        }

        return complexEffect;
    }

    private static ComplexEffect AssignManeuverEffect()
    {
        Card attackingCard = _currentPlay.AttackingCard;
        ComplexEffect complexEffect = new ComplexEffect(_view, _game);
        complexEffect.Add( new ManeuverInitialEffect(_view) );
        switch (attackingCard.Title)
        {
            case "Head Butt":
            case "Arm Drag":
            case "Arm Bar":
                complexEffect.Add(new MakePlayerDiscardCard(_view, 
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
                complexEffect.Add(new MakePlayerDiscardCard(_view,
                    playerThatMustDiscard: _notCurrentPlayer, numberOfCardToDiscard: 1));
                break;
            case "Pump Handle Slam":
            case "Tree of Woe":
                complexEffect.Add(new MakePlayerDiscardCard(_view, 
                    playerThatMustDiscard: _notCurrentPlayer, numberOfCardToDiscard: 2));
                break;
            case "Bulldog":
                complexEffect.Add(new MakePlayerDiscardCard(_view, 
                    playerThatMustDiscard: _currentPlayer, numberOfCardToDiscard: 1));
                complexEffect.Add(new DiscardCardFromOpponentsHand(_view));
                break;
            case "Kick":
                complexEffect.Add(new TopCardOfArsenalToRingsidePile(_view, _game));
                complexEffect.Add(new NextCardFortitudeBonusByTitle(_view, 
                    "Stone Cold Stunner", fortitudeBonus: -6));
                break;
            case "Running Elbow Smash":
                complexEffect.Add(new TopCardOfArsenalToRingsidePile(_view, _game));
                break;
            case "Double Leg Takedown":
            case "Reverse DDT":
                complexEffect.Add(new DrawCardsAskingNumber(_view,
                    playerThatMustDraw: _currentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Headlock Takedown":
            case "Standing Side Headlock":
                complexEffect.Add(new PlayerDrawCards(_view,
                    player: _notCurrentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Fisherman's Suplex":
                complexEffect.Add(new TopCardOfArsenalToRingsidePile(_view, _game));
                complexEffect.Add(new DrawCardsAskingNumber(_view,
                    playerThatMustDraw: _currentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Press Slam":
            case "DDT":
                complexEffect.Add(new TopCardOfArsenalToRingsidePile(_view, _game));
                complexEffect.Add(new MakePlayerDiscardCard(_view, 
                    playerThatMustDiscard: _notCurrentPlayer, numberOfCardToDiscard: 2));
                break;
            case "Guillotine Stretch":
                complexEffect.Add(new MakePlayerDiscardCard(_view,  
                    playerThatMustDiscard: _notCurrentPlayer, numberOfCardToDiscard: 1));
                complexEffect.Add(new DrawCardsAskingNumber(_view, 
                    playerThatMustDraw: _currentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Chicken Wing":
                complexEffect.Add(new ShuffleFromRingsideToArsenal(_view,
                    pretendedNumberCardsToShuffle: 2));
                break;

            case "Back Body Drop":
                complexEffect.Add(new DrawCardsOrForceOpponentDiscard(_view));
                break;
            case "Haymaker":
                NextCardDamageBonusByTypeAndSubtype haymakerEffect =
                    new NextCardDamageBonusByTypeAndSubtype(_view, _game.GetPlayManager(), type: "Maneuver", 
                    subtype: "Strike", bonus: 1);
                complexEffect.Add(
                    new EffectForTheRestOfTheTurn(_view, _currentPlay, haymakerEffect));
                break;
            case "Superkick":
                complexEffect.Add(new DamageBonusIfSatisfiesConditions(_view, 
                    damageBonus: 5, minimumDamage: 5, 
                    typeOfPreviousCardThatAppliesBonus: "Maneuver", 
                    subtypeOfPreviousCardThatAppliesBonus: "All"));
                break;
            case "Clothesline":
            case "Atomic Drop":
                _currentPlay.AddPendingEffect(new NextCardDamageBonusByTypeAndSubtype(_view, _game.GetPlayManager(),
                    type: "Maneuver", subtype: "All", bonus: 2));
                break;
            case "Snap Mare":
                _currentPlay.AddPendingEffect(new NextCardDamageBonusByTypeAndSubtype(_view, _game.GetPlayManager(),
                    type: "Maneuver", subtype: "Strike", bonus: 2));
                break;
            case "Rock Bottom":
                complexEffect.Add(new MoveSpecificCardFromRingSideToArsenal(_view, _currentPlayer, 
                     cardTitleToRecover: "The People's Elbow"));
                break;
            case "Stone Cold Stunner":
                break;
            case "Kane's Tombstone Piledriver":
                break;
            case "Pedigree":
                complexEffect.Add(new DamageBonusIfSatisfiesConditions(_view, 
                    damageBonus: 2, subtypeOfPreviousCardThatAppliesBonus:"Strike", 
                    typeOfPreviousCardThatAppliesBonus: "Maneuver", minimumDamage: 0));
                break;
            case "Powerbomb":
                complexEffect.Add(new DrawCardsAskingNumber(_view, _currentPlayer, numberOfCardsToDraw: 1));
                complexEffect.Add(new BonusBySlamCardsInRingArea(_view, attackingCard));
                break;
        }

        return complexEffect;
    }

    private static ComplexEffect AssignActionEffect()
    {
        Card card = _currentPlay.AttackingCard;
        ComplexEffect complexEffect = new ComplexEffect(_view, _game);
        _view.SayThatPlayerSuccessfullyPlayedACard();
        switch (card.Title)
        {
            case "Jockeying for Position":
                complexEffect.Add(new MoveCardFromHandToRingArea(_view));
                JockeyingForPositionSelectableEffect selectableEffect =
                    new JockeyingForPositionSelectableEffect(_view, _game);
                selectableEffect.ApplyEffect(currentPlay: _currentPlay);
                break;
            case "Offer Handshake":
                complexEffect.Add(new MoveCardFromHandToRingArea(_view));
                complexEffect.Add(new DrawCardsAskingNumber(_view, 
                    _currentPlayer, numberOfCardsToDraw: 3));
                complexEffect.Add(new MakePlayerDiscardCard(_view, 
                    _currentPlayer, numberOfCardToDiscard: 1));
                break;
            case "Irish Whip":
                complexEffect.Add(new MoveCardFromHandToRingArea(_view));
                NextCardDamageBonusByTypeAndSubtype irishWhipBonusEffect =
                    new NextCardDamageBonusByTypeAndSubtype(_view,_game.GetPlayManager(), type: "Maneuver", 
                        subtype: "Strike", bonus: 5);
                complexEffect.Add(new EffectForNextCard(_view, irishWhipBonusEffect));
                break;
            case "I Am the Game":
                complexEffect.Add(new MoveCardFromHandToRingArea(_view));
                NextCardDamageBonusByTypeAndSubtype iAmTheGameBonusEffect =
                    new NextCardDamageBonusByTypeAndSubtype(_view,_game.GetPlayManager(), type: "Maneuver",
                        subtype: "All", bonus: 3);
                complexEffect.Add(new EffectForTheRestOfTheTurn(_view, 
                        _currentPlay, effect: iAmTheGameBonusEffect));
                complexEffect.Add(new DrawCardsOrForceOpponentDiscard(_view));
                break;
            case "Y2J":
                complexEffect.Add(new MoveCardFromHandToRingArea(_view));
                SelectedEffect selectedEffectY2J =
                    _view.AskUserToChooseBetweenDrawingOrForcingOpponentToDiscardCards(
                        superstarName: _currentPlayer.GetSuperStarName());
                if (selectedEffectY2J == SelectedEffect.DrawCards)
                    complexEffect.Add(new DrawCardsAskingNumber(_view,
                        _currentPlayer, numberOfCardsToDraw: 5));
                else if (selectedEffectY2J == SelectedEffect.ForceOpponentToDiscard)
                    complexEffect.Add(new MakePlayerDiscardCard(_view,
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
                complexEffect.Add(new MoveCardFromHandToRingArea(_view));
                complexEffect.Add(new MakePlayerDiscardCard(_view, 
                    _currentPlayer, numberOfCards));
                complexEffect.Add(new ShuffleFromRingsideToHand(_view, 
                    pretendedNumberCardsToShuffle: numberOfCards));
                break;
            case "Spit At Opponent":
                complexEffect.Add(new MoveCardFromHandToRingArea(_view));
                complexEffect.Add(new MakePlayerDiscardCard(_view,
                 _currentPlayer, numberOfCardToDiscard: 1));
                complexEffect.Add(new MakePlayerDiscardCard(_view,
                 _notCurrentPlayer, numberOfCardToDiscard: 4));
                break;
            case "Recovery":
                complexEffect.Add(new MoveCardFromHandToRingArea(_view));
                complexEffect.Add(new ShuffleFromRingsideToArsenal(_view, 
                    pretendedNumberCardsToShuffle: 2));
                complexEffect.Add(new PlayerDrawCards(_view, 
                    _currentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Puppies! Puppies!":
                complexEffect.Add(new MoveCardFromHandToRingArea(_view));
                complexEffect.Add(new ShuffleFromRingsideToArsenal(_view, 
                    pretendedNumberCardsToShuffle: 5));
                complexEffect.Add(new PlayerDrawCards(_view, 
                    _currentPlayer, numberOfCardsToDraw: 2));
                break;
            case "Chop":
            case "Arm Bar Takedown":
            case "Collar & Elbow Lockup":
            case "Undertaker's Tombstone Piledriver":
                complexEffect.Add(new DiscardToDrawWithoutDamage(_view));
                break;
            case "The People's Elbow":
                complexEffect.Add(new PlaceCardFromHandAtArsenalEnd(_view));
                complexEffect.Add(new PlayerDrawCards(_view, _currentPlayer, numberOfCardsToDraw: 2));
                break;
            case "Shake It Off":
                complexEffect.Add(new ShakeItOff(_view));
                complexEffect.Add(new MoveCardFromHandToRingArea(_view));
                break;
            case "Mr. Socko":
                _currentPlay.AddPendingEffect(new DamageBonusIfACardIsInRingArea(_view,
                    cardTitleThatMustBeInRingArea: "Mr. Socko", damageBonus: 1));
                complexEffect.Add(new MrSockoSelectable(_view));
                complexEffect.Add(new MoveCardFromHandToRingArea(_view));
                break;
            case "Get Crowd Support":
                var damageBonusGCS = new NextCardDamageBonusByTypeAndSubtype(_view, _game.GetPlayManager(),
                    type: "Maneuver", subtype: "All", bonus: 4);
                var fortitudeBonusGCS = new ReversalsRequiresMoreFortitudeByTypeAndSubtype(_view,
                    _game.GetPlayManager(), type: "Maneuver", subtype: "All", extraFortitude: 12);
                complexEffect.Add(new PlayerDrawCards(_view, _currentPlayer, numberOfCardsToDraw: 1));
                _currentPlay.AddPendingEffect(damageBonusGCS);
                _currentPlay.AddPendingEffect(fortitudeBonusGCS);
                complexEffect.Add(new MoveCardFromHandToRingArea(_view));
                break;
            case "Open Up a Can of Whoop-A%$":
                var damageBonusOUACOW = new NextCardDamageBonusByTypeAndSubtype(_view, _game.GetPlayManager(),
                    type: "Maneuver", subtype: "All", bonus: 6);
                var fortitudeBonusOUACOW = new ReversalsRequiresMoreFortitudeByTypeAndSubtype(_view,
                    _game.GetPlayManager(), type: "Maneuver", subtype: "All", extraFortitude: 20);
                _currentPlay.AddPendingEffect(damageBonusOUACOW);
                _currentPlay.AddPendingEffect(fortitudeBonusOUACOW);
                complexEffect.Add(new PlayerDrawCards(_view, _currentPlayer, numberOfCardsToDraw: 1));
                complexEffect.Add(new MoveCardFromHandToRingArea(_view));
                break;
            case "Power of Darkness":
                var damageBonusPowerOfDarkness = new NextCardDamageBonusByTypeAndSubtype(_view, _game.GetPlayManager(),
                    type: "Maneuver", subtype: "All", bonus: 5);
                var fortitudeBonusPowerOfDarkness = new ReversalsRequiresMoreFortitudeByTypeAndSubtype(_view,
                    _game.GetPlayManager(), type: "Maneuver", subtype: "All", extraFortitude: 20);
                _currentPlay.AddPendingEffect(new EffectForTheRestOfTheTurn(_view, _currentPlay, damageBonusPowerOfDarkness));
                _currentPlay.AddPendingEffect(new EffectForTheRestOfTheTurn(_view, _currentPlay, fortitudeBonusPowerOfDarkness));
                complexEffect.Add(new MoveCardFromHandToRingArea(_view));
                break;
        }

        return complexEffect;
    }

    private static ComplexEffect AssignReversalEffect()
    {
        Card selectedCard = _currentPlay.ReversalCard;
        ComplexEffect complexEffect = new ComplexEffect(_view, _game);
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
            case "Pedigree":
                complexEffect.Add(new ReversalSimple(_view));
                break;
            case "Rolling Takedown":
            case "Knee to the Gut":
                complexEffect.Add(new ReversalSimple(_view));
                complexEffect.Add(new SetDamageFromAttackingCardToReversal(_view));
                break;
            case "Manager Interferes":
                if (selectedCard.PlayedFrom == "Hand")
                {
                    complexEffect.Add(new ReversalSimple(_view));
                    complexEffect.Add(new PlayerDrawCards(_view,
                        _notCurrentPlayer, numberOfCardsToDraw: 1));
                }
                break;
            case "Chyna Interferes":
                if (selectedCard.PlayedFrom == "Hand")
                {
                    complexEffect.Add(new ReversalSimple(_view));
                    complexEffect.Add(new PlayerDrawCards(_view, 
                        _notCurrentPlayer, numberOfCardsToDraw: 2));
                }
                break;
            case "Clean Break":
                complexEffect.Add(new ReversalSimple(_view));
                complexEffect.Add(new MakePlayerDiscardCard(_view, _currentPlayer, 
                    numberOfCardToDiscard:4));
                complexEffect.Add(new PlayerDrawCards(_view, _notCurrentPlayer, 
                    numberOfCardsToDraw:1));
                break;
            case "Jockeying for Position":
                complexEffect.Add(new ReversalSimple(_view));
                complexEffect.Add(new EndPlayEffect(_view));
                complexEffect.Add(new JockeyingForPositionSelectableEffect(_view, _game));
                break;
            case "Irish Whip":
                complexEffect.Add(new ReversalSimple(_view));
                _currentPlay.AddPendingEffect(new NextCardDamageBonusByTypeAndSubtype(_view, _game.GetPlayManager(),
                    type: "Maneuver", subtype: "Strike", bonus:5));
                break;
            case "Facebuster":
                complexEffect.Add(new ReversalSimple(_view));
                complexEffect.Add(new DrawCardsAskingNumber(_view, 
                    _notCurrentPlayer, numberOfCardsToDraw: 2));
                break;
            case "Lou Thesz Press":
                complexEffect.Add(new ReversalSimple(_view));
                complexEffect.Add(new DrawCardsAskingNumber(_view, 
                    _notCurrentPlayer, numberOfCardsToDraw: 1));
                break;
            
            case "Rock Bottom":
                complexEffect.Add(new ReversalSimple(_view));
                complexEffect.Add(new MakePlayerDiscardCard(_view, _notCurrentPlayer, 
                    numberOfCardToDiscard: 1));
                complexEffect.Add(new MoveSpecificCardFromRingSideToArsenal(_view, _notCurrentPlayer, 
                    cardTitleToRecover: "The People's Elbow"));
                break;
        }

        return complexEffect;
    }

    public static void AssignPreEffects(Card card)
    {
        switch (card.Title)
        {
            case "Discus Punch":
                _currentPlay.AddPendingEffect(new ReversalsDamageBonus(_view, _game.GetPlayManager(), damageBonus: 2));
                break;
            case "Undertaker's Flying Clothesline" :
            case "Kane's Flying Clothesline":
                _currentPlay.AddPendingEffect(new ReversalsDamageBonus(_view, _game.GetPlayManager(), damageBonus: 6));
                break;
        }
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
        PlayersPackage playersPackage =
            _game.GetPlayersPackage();
        _currentPlayer = playersPackage.CurrentPlayer;
        _notCurrentPlayer = playersPackage.NotCurrentPlayer;
    }
}