using System.Reflection.Metadata;
using RawDeal.Cards.CardEffects.ActionEffects;
using RawDeal.Cards.CardEffects.GeneralEffects;
using RawDeal.Cards.CardEffects.ManeuverEffects;
using RawDeal.Cards.CardEffects.ReversalsEffects;
using RawDeal.Cards.CardEffects.ReversalsEffects.SpecificCards;
using RawDeal.Cards.CardEffects.SpecificCardEffects;
using RawDeal.GameLogic;
using RawDealView;
using RawDealView.Options;

namespace RawDeal.Cards.CardEffects;

public class EffectAsigner
{
    private Player CurrentPlayer; 
    private Player NotCurrentPlayer;
    private Game _game;
    private View _view;
    

    public EffectAsigner(Game game, View view)
    {
        _view = view;
        _game = game;
        SetPlayers();
    }
    
    public List<Effect> AssignManeuverEffect(Card attackingCard)
    {
        List<Effect> effectsAssigned = new List<Effect>();
        effectsAssigned.Add(new ManeuverInitialEffect(_view));
        switch (attackingCard.Title)
        {
            case "Head Butt":
            case "Arm Drag":
            case "Arm Bar":
                effectsAssigned.Add(new MakePlayerDiscardCard(_view, playerThatMustDiscard: CurrentPlayer, numberOfCardToDiscard: 1));
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
                effectsAssigned.Add(new MakePlayerDiscardCard(_view,
                    playerThatMustDiscard: NotCurrentPlayer, numberOfCardToDiscard: 1));
                break;
            case "Pump Handle Slam":
                effectsAssigned.Add(new MakePlayerDiscardCard(_view,
                    playerThatMustDiscard: NotCurrentPlayer, numberOfCardToDiscard: 2));
                break;
            case "Bulldog":
                effectsAssigned.Add(new MakePlayerDiscardCard(_view,
                    playerThatMustDiscard: CurrentPlayer, numberOfCardToDiscard: 1));
                effectsAssigned.Add( new DiscardCardFromOpponentsHand(view: _view));
                break;
            case "Kick":
            case "Running Elbow Smash":
                effectsAssigned.Add( new TopCardOfArsenalToRingsidePile(_view, _game));
                break;
            case "Double Leg Takedown":
            case "Reverse DDT":
                effectsAssigned.Add(new DrawCardsAskingNumber(_view, playerThatMustDraw: CurrentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Headlock Takedown":
            case "Standing Side Headlock":
                effectsAssigned.Add(new OpponentDrawCards(_view, playerThatMustDraw: NotCurrentPlayer,
                    numberOfCardsToDraw: 1));
                break;
            case "Undertaker's Tombstone Piledriver":
                // Es action Código para la carta "Undertaker’s Tombstone Piledriver"
                break;
            case "Fisherman's Suplex":
                effectsAssigned.Add(new TopCardOfArsenalToRingsidePile(_view, _game));
                effectsAssigned.Add(new DrawCardsAskingNumber(view: _view, playerThatMustDraw: CurrentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Press Slam":
            case "DDT":
                effectsAssigned.Add( new TopCardOfArsenalToRingsidePile(_view, _game));
                effectsAssigned.Add(new MakePlayerDiscardCard(view: _view, NotCurrentPlayer,  numberOfCardToDiscard: 2));
                break;
            case "Guillotine Stretch":
                effectsAssigned.Add(new MakePlayerDiscardCard(_view, playerThatMustDiscard: NotCurrentPlayer, numberOfCardToDiscard: 1));
                effectsAssigned.Add( new DrawCardsAskingNumber(_view, playerThatMustDraw: CurrentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Spit At Opponent":
                // TODO add playable condition
                effectsAssigned.Add(new MakePlayerDiscardCard( _view, playerThatMustDiscard: CurrentPlayer, numberOfCardToDiscard: 1));
                effectsAssigned.Add(new MakePlayerDiscardCard(_view, playerThatMustDiscard: NotCurrentPlayer, numberOfCardToDiscard: 4));
                break;
            case "Chicken Wing":
                effectsAssigned.Add(new ShuffleFromRingsideToHand(_view, pretendedNumberCardsToShuffle: 2));
                break;
            case "Puppies! Puppies!":
                effectsAssigned.Add(new ShuffleFromRingsideToHand(_view, pretendedNumberCardsToShuffle: 5));
                effectsAssigned.Add(new DrawCardsAskingNumber(_view, playerThatMustDraw: CurrentPlayer, numberOfCardsToDraw: 2));
                break;
            case "Recovery":
                effectsAssigned.Add(new ShuffleFromRingsideToHand(_view, pretendedNumberCardsToShuffle: 2));
                effectsAssigned.Add(new DrawCardsAskingNumber(_view, playerThatMustDraw: CurrentPlayer, numberOfCardsToDraw: 1));
                break;
            case "Lionsault":
                // TODO game condition
                effectsAssigned.Add(new MakePlayerDiscardCard(_view, playerThatMustDiscard: NotCurrentPlayer, numberOfCardToDiscard: 1));
                break;
            case "Tree of Woe":
                // TODO cannot be reversed
                effectsAssigned.Add(new MakePlayerDiscardCard(_view, playerThatMustDiscard: NotCurrentPlayer, numberOfCardToDiscard: 2));
                break;
            case "Austin Elbow Smash":
                // TODO cannot be reversed and game condition
                break;

            case "Back Body Drop":
                SelectedEffect selectedEffectBBD = _view.AskUserToChooseBetweenDrawingOrForcingOpponentToDiscardCards(CurrentPlayer.GetSuperStarName());
                if (selectedEffectBBD == SelectedEffect.DrawCards)
                    effectsAssigned.Add(new DrawCardsAskingNumber(_view, playerThatMustDraw: CurrentPlayer, numberOfCardsToDraw: 2));
                else if (selectedEffectBBD == SelectedEffect.ForceOpponentToDiscard)
                    effectsAssigned.Add(new MakePlayerDiscardCard(_view, playerThatMustDiscard: NotCurrentPlayer, numberOfCardToDiscard: 2));
                break;
            
            case "Leaping Knee to the Face":
                effectsAssigned.Add(new MakePlayerDiscardCard( _view, playerThatMustDiscard: NotCurrentPlayer, numberOfCardToDiscard: 1));
                break;
            case "Haymaker":
                NextCardDamageBonusByTypeAndSubtype haymakerEffect = new NextCardDamageBonusByTypeAndSubtype(view: _view);
                haymakerEffect.SetDamageBonus(bonus: 1);
                haymakerEffect.SetPlayedTypeAndSubtypeThatAppliesBonus(type: "Maneuver", subtype: "Strike");
                effectsAssigned.Add(haymakerEffect);
                break;
            case "Superkick":
                // Código para la carta "Superkick"
                break;
            case "Clothesline":
            case "Atomic Drop":
                NextCardDamageBonusByTypeAndSubtype damageBonusEffect = new NextCardDamageBonusByTypeAndSubtype(view: _view);
                damageBonusEffect.SetDamageBonus(bonus: 2);
                damageBonusEffect.SetPlayedTypeAndSubtypeThatAppliesBonus(type: "Maneuver", subtype: "All");
                effectsAssigned.Add(damageBonusEffect);
                break;
            case "Snap Mare":
                NextCardDamageBonusByTypeAndSubtype snapMaraEffect = new NextCardDamageBonusByTypeAndSubtype(view: _view);
                snapMaraEffect.SetDamageBonus(bonus: 2);
                snapMaraEffect.SetPlayedTypeAndSubtypeThatAppliesBonus(type: "Maneuver", subtype: "Strike");
                effectsAssigned.Add(snapMaraEffect);
                break;
        }
        return effectsAssigned;
    }

    public List<Effect> AssignActionEffect(Card card)
    {
        List<Effect> effectsAssigned = new List<Effect>();
        _view.SayThatPlayerSuccessfullyPlayedACard();
        switch (card.Title)
        {
            case "Jockeying for Position":
                effectsAssigned.Add(new JockeyingForPositionActionEffect(_view));
                break;
            case "Offer Handshake":
                effectsAssigned.Add(new DrawCardsAskingNumber(_view, CurrentPlayer, numberOfCardsToDraw: 3));
                effectsAssigned.Add(new MakePlayerDiscardCard(_view, CurrentPlayer, numberOfCardToDiscard: 1));
                break;
            case "Irish Whip":
                var irishBonusEffect = new NextCardDamageBonusByTypeAndSubtype(_view);
                irishBonusEffect.SetDamageBonus(bonus: 5);
                irishBonusEffect.SetPlayedTypeAndSubtypeThatAppliesBonus("Maneuver", "Strike");
                effectsAssigned.Add(new NextCardDamageBonusByTypeAndSubtype(_view));
                break;
            case "I Am the Game":
                var iAmTheGameBonusEffect = new NextCardDamageBonusByTypeAndSubtype(_view);
                iAmTheGameBonusEffect.SetDamageBonus(bonus: 3);
                iAmTheGameBonusEffect.SetPlayedTypeAndSubtypeThatAppliesBonus("Maneuver", "All");
                effectsAssigned.Add(new NextCardDamageBonusByTypeAndSubtype(_view));
                break;
            case "Y2J":
                SelectedEffect selectedEffectY2J = _view.AskUserToChooseBetweenDrawingOrForcingOpponentToDiscardCards(CurrentPlayer.GetSuperStarName());
                if (selectedEffectY2J == SelectedEffect.DrawCards)
                    effectsAssigned.Add(new DrawCardsAskingNumber(_view, playerThatMustDraw: CurrentPlayer, numberOfCardsToDraw: 5));
                else if (selectedEffectY2J == SelectedEffect.ForceOpponentToDiscard)
                    effectsAssigned.Add( new MakePlayerDiscardCard( _view, playerThatMustDiscard: NotCurrentPlayer, numberOfCardToDiscard: 5));
                break;
            case "Roll Out of the Ring":
                effectsAssigned.Add(new MakePlayerDiscardCard(_view, CurrentPlayer, numberOfCardToDiscard: 2));
                effectsAssigned.Add(new ShuffleFromRingsideToHand(_view, pretendedNumberCardsToShuffle: 2));
                break;
            default:
                effectsAssigned.Add( new DiscardToDrawWithoutDamage(_view));
                break;
        }
        return effectsAssigned;
    }

    public List<Effect> AssignReversalEffect(Play currentPlay)
    {
        Card selectedCard = currentPlay.ReversalCard;
        List<Effect> effectsAssigned = new List<Effect>();
        switch (selectedCard.Title)
        {
            case "Step Aside":
            case "Escape Move":
            case "Break the Hold":
            case "No Chance in Hell":
                effectsAssigned.Add(new ReversalSimple(_view));
                break;
            case "Rolling Takedown":
                effectsAssigned.Add(new RollingTakedown(_view));
                break;
            case "Knee to the Gut":
                effectsAssigned.Add(new KneeToTheGut(_view));
                break;
            case "Elbow to the Face":
                const int maximumDamageReversalElbowToTheFace = 7;
                effectsAssigned.Add(new ReversalWithMaximumDamage(_view, maximumDamageReversalElbowToTheFace));;
                break;
            case "Manager Interferes":
                effectsAssigned.Add(new ManagerInterferesEffect(_view));
                break;
            case "Chyna Interferes":
                effectsAssigned.Add(new ChynaInterferesEffect(_view));
                break;
            case "Clean Break":
                effectsAssigned.Add(new CleanBreakEffect(_view));
                break;
            case "Jockeying for Position":
                effectsAssigned.Add(new JockeyingForPositionReversalEffect(_view));
                break;
            case "Irish Whip":
                effectsAssigned.Add(new ReversalByTitle(_view, cardTitleThatCanReverse: "Irish Whip"));
                NextCardDamageBonusByTypeAndSubtype irishBonusEffect = new NextCardDamageBonusByTypeAndSubtype(_view);
                irishBonusEffect.SetDamageBonus(bonus: 5);
                irishBonusEffect.SetPlayedTypeAndSubtypeThatAppliesBonus("Maneuver", "Strike");
                effectsAssigned.Add(irishBonusEffect);
                break;
            case "Shoulder Block":
                // Código para la carta "Shoulder Block"
                break;
            case "Spear":
                // Código para la carta "Spear"
                break;
            case "Facebuster":
                // Código para la carta "Facebuster"
                break;
            case "Lou Thesz Press":
                // Código para la carta "Lou Thesz Press"
                break;
            case "Cross Body Block":
                // Código para la carta "Cross Body Block"
                break;
            case "Belly to Belly Suplex":
                // Código para la carta "Belly to Belly Suplex"
                break;
            case "Vertical Suplex":
                effectsAssigned.Add(new ReversalByTitle(_view, "Vertical Suplex"));
                break;
            case "Belly to Back Suplex":
                effectsAssigned.Add(new ReversalByTitle(_view, "Belly to Back Suplex"));
                break;
            case "Ensugiri":
                effectsAssigned.Add(new ReversalByTitle(_view, "Kick"));
                break;
            case "Drop Kick":
                effectsAssigned.Add(new ReversalByTitle(_view, "Drop Kick"));
                break;
            case "Double Arm DDT":
                effectsAssigned.Add(new ReversalByTitle(_view, "Back Body Drop"));
                break;
            
        }

        return effectsAssigned;
    }

    private void SetPlayers()
    {
        Dictionary<string, Player> playersDictionary = _game.GetDictionaryOfCurrentAndNotCurrentPlayer();
        CurrentPlayer = playersDictionary["CurrentPlayer"];
        NotCurrentPlayer = playersDictionary["NotCurrentPlayer"];
    }


}

    
