using RawDeal.Cards.CardEffects.ActionEffects;
using RawDeal.Cards.CardEffects.ReversalsEffects;
using RawDeal.Cards.CardEffects.SpecificCardEffects;
using RawDeal.GameLogic;
using RawDealView;

namespace RawDeal.Cards.CardEffects;

public class EffectAsigner
{
    private Player CurrentPlayer; 
    private Player NotCurrentPlayer; 
    private View _view;
    

    public EffectAsigner(Dictionary<string, Player> playersDictionary, View view)
    {
        _view = view;
        SetPlayers(playersDictionary);
    }

    public Effect AssignActionEffect(Card card)
    {
        if (card.Title == "Jockeying for Position")
        {
            return new JockeyingForPositionActionEffect(_view);
        }

        return new ActionSimple(_view);
    }

    public Effect AssignReversalEffect(Play currentPlay)
    {
        Card selectedReversalCard = currentPlay.ReversalCard;
        try
        {
            switch (selectedReversalCard.Title)
            {
                case "Step Aside":
                case "Escape Move":
                case "Break the Hold":
                case "No Chance in Hell":
                    return new ReversalSimple(_view);
                case "Rolling Takedown":
                    ReversalWithMaximumDamage rollingTakeDownEffect = new ReversalWithMaximumDamage(_view);
                    const int maximumDamageReversalRollingTakeDown = 7;
                    rollingTakeDownEffect.SetMaximumDamageThatCanReverse(maximumDamageReversalRollingTakeDown);
                    selectedReversalCard.SetCurrentDamage(NotCurrentPlayer.CalculateDamage(currentPlay.AttackingCard));
                    return rollingTakeDownEffect;
                case "Knee to the Gut":
                    ReversalWithMaximumDamage kneeToTheGutEffect = new ReversalWithMaximumDamage(_view);
                    const int maximumDamageReversalKneeToTheGut = 7;
                    kneeToTheGutEffect.SetMaximumDamageThatCanReverse(maximumDamageReversalKneeToTheGut);
                    selectedReversalCard.SetCurrentDamage(NotCurrentPlayer.CalculateDamage(currentPlay.AttackingCard));
                    return kneeToTheGutEffect;
                case "Elbow to the Face":
                    ReversalWithMaximumDamage reversalElbowToTheFace = new ReversalWithMaximumDamage(_view);
                    const int maximumDamageReversalElbowToTheFace = 7;
                    reversalElbowToTheFace.SetMaximumDamageThatCanReverse(maximumDamageReversalElbowToTheFace);
                    return reversalElbowToTheFace;
                case "Manager Interferes":
                    return new ManagerInterferesEffect(_view);
                case "Chyna Interferes":
                    return new ChynaInterferesEffect(_view);
                case "Clean Break":
                    return new CleanBreakEffect(_view);
                case "Jockeying for Position":
                    return new JockeyingForPositionReversalEffect(_view);
            }

            throw new Exception("Invalid reversal card selected.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            throw;
        }
    }

    private void SetPlayers(Dictionary<string, Player> playersDictionary)
    {
        CurrentPlayer = playersDictionary["CurrentPlayer"];
        NotCurrentPlayer = playersDictionary["NotCurrentPlayer"];
    } 
}

    
