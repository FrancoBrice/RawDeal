using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;

namespace RawDeal.Cards.CardPreConditions;

public static class ReversalsChecker
{
    public static bool IsCorrectReversalCard(PlayManager playManager, Card reversalCard)
    {
        Play currentPlay = playManager.GetCurrentPlay();
        Card attackingCard = currentPlay.AttackingCard;
        Player damagedPlayer = currentPlay.NotCurrentPlayer;
        if (!attackingCard.CanBeReversed) return false;
        if (reversalCard.GetCurrentFortitude(currentPlay, reversalCard.PlayedType) > damagedPlayer.Fortitude)
            return false;
        if (reversalCard.Subtypes.Contains("ReversalStrike"))
        {
            if (attackingCard.PlayedType == "Maneuver" &&
                attackingCard.Subtypes.Contains("Strike"))
                return true;
        }
        else if (reversalCard.Subtypes.Contains("ReversalGrapple") &&
                 attackingCard.PlayedType == "Maneuver" &&
                 attackingCard.Subtypes.Contains("Grapple"))
        {
            return true;
        }
        else if (reversalCard.Subtypes.Contains("ReversalSubmission") &&
                 attackingCard.PlayedType == "Maneuver" &&
                 attackingCard.Subtypes.Contains("Submission"))
        {
            return true;
        }
        else if (reversalCard.Subtypes.Contains("ReversalAction") &&
                 attackingCard.PlayedType == "Action")
        {
            return true;
        }
        else if (reversalCard.Subtypes.Contains("ReversalGrappleSpecial"))
        {
            const int maximumDamageThatCanReverse = 7;
            if (attackingCard.PlayedType == "Maneuver" &&
                attackingCard.Subtypes.Contains("Grapple") &&
                attackingCard.GetCurrentDamage() <= maximumDamageThatCanReverse)
                return true;
        }
        else if (reversalCard.Subtypes.Contains("ReversalStrikeSpecial"))
        {
            const int maximumDamageThatCanReverse = 7;
            if (attackingCard.PlayedType == "Maneuver" &&
                attackingCard.Subtypes.Contains("Strike") &&
                attackingCard.GetCurrentDamage() <= maximumDamageThatCanReverse)
                return true;
        }
        else if (reversalCard.Subtypes.Contains("ReversalSpecial"))
        {
            return IsValidTheConditionOfReversalSpecial(playManager, reversalCard);
        }

        return false;
    }

    private static bool IsValidTheConditionOfReversalSpecial(PlayManager playManager,
        Card reversalCard)
    {
        Play currentPlay = playManager.GetCurrentPlay();
        Card attackingCard = currentPlay.AttackingCard;
        Player playerThatCanReverse = currentPlay.NotCurrentPlayer;
        switch (reversalCard.Title)
        {
            case "Elbow to the Face":
                if (attackingCard.PlayedType == "Maneuver" &&
                    playerThatCanReverse.CalculateDamage(attackingCard) <= 7)
                    return true;
                break;
            case "Manager Interferes" when attackingCard.PlayedType == "Maneuver":
            case "Chyna Interferes" when attackingCard.PlayedType == "Maneuver":
                return true;
            case "Clean Break" when attackingCard.Title == "Jockeying for Position":
            case "Jockeying for Position" when attackingCard.Title == "Jockeying for Position":
                return true;
            case "Irish Whip" when attackingCard.Title == "Irish Whip":
                return true;
            case "Shoulder Block":
            case "Spear":
            case "Cross Body Block":
                if (playManager.GetNumberOfPlayedCards() >= 2 &&
                    attackingCard.PlayedType == "Maneuver" &&
                    playManager.GetPenultimateCardPlayed().Title == "Irish Whip")
                    return true;
                break;
            case "Facebuster":
            case "Lou Thesz Press":
                if (playManager.GetNumberOfPlayedCards() >= 2 &&
                    attackingCard.PlayedType == "Maneuver" && reversalCard.PlayedFrom == "Hand" &&
                    playManager.GetPenultimateCardPlayed().Title == "Irish Whip")
                    return true;
                break;
            case "Belly to Belly Suplex" when attackingCard.Title == "Belly to Belly Suplex":
                return true;
            case "Vertical Suplex" when attackingCard.Title == "Vertical Suplex":
                return true;
            case "Belly to Back Suplex" when attackingCard.Title == "Belly to Back Suplex":
                return true;
            case "Ensugiri" when attackingCard.Title == "Kick":
                return true;
            case "Drop Kick" when attackingCard.Title == "Drop Kick":
                return true;
            case "Double Arm DDT" when attackingCard.Title == "Back Body Drop":
                return true;
            case "Pedigree" when attackingCard.Title == "Back Body Drop":
                return true;
            case "Rock Bottom":
                if (reversalCard.PlayedFrom == "Hand" && attackingCard.PlayedType == "Maneuver" && 
                    attackingCard.Subtypes.Contains("Grapple") 
                    && playerThatCanReverse.GetHandSize() >= 2)
                {
                    return true;
                }
                break;
        }

        return false;
    }
}