using RawDeal.GameLogic;

namespace RawDeal.Cards.CardPreConditions;

public static class ReversalsChecker
{
    public static bool IsCorrectReversalCard(PlayManager playManager, Card reversalCard, string playedFrom)
    {
        Play currentPlay = playManager.CurrentPlay;
        Card attackingCard = currentPlay.AttackingCard;
        Player playerThatCanReverse = currentPlay.NotCurrentPlayer;
        if (reversalCard.GetCurrentFortitude(reversalCard.PlayedType) > playerThatCanReverse.Fortitude)
        {
            return false;
        }
        if (reversalCard.Subtypes.Contains("ReversalStrike"))
        {
            if (attackingCard.PlayedType.Contains("Maneuver") && attackingCard.Subtypes.Contains("Strike"))
            {
                return true;
            }
        }
        if (reversalCard.Subtypes.Contains("ReversalGrapple"))
        {
            if (attackingCard.PlayedType.Contains("Maneuver") && attackingCard.Subtypes.Contains("Grapple"))
            {
                return true;
            }
        } 
        if (reversalCard.Subtypes.Contains("ReversalSubmission"))
        {
            if (attackingCard.PlayedType.Contains("Maneuver") && attackingCard.Subtypes.Contains("Submission"))
            {
                return true;
            }
        }
        if (reversalCard.Subtypes.Contains("ReversalAction"))
        {
            if (attackingCard.PlayedType.Contains("Action"))
            {
                return true;
            }
        }
        
        if (reversalCard.Subtypes.Contains("ReversalGrappleSpecial"))
        {
            if (attackingCard.PlayedType.Contains("Maneuver") && attackingCard.Subtypes.Contains("Grapple"))
            {
                const int maximumDamageThatCanReverse = 7;
                if (attackingCard.GetCurrentDamage(attackingCard.PlayedType) <= maximumDamageThatCanReverse)
                {
                    return true;
                }
            }
        }
        
        if (reversalCard.Subtypes.Contains("ReversalStrikeSpecial"))
        {
            if (attackingCard.PlayedType.Contains("Maneuver") && attackingCard.Subtypes.Contains("Strike"))
            {
                const int maximumDamageThatCanReverse = 7;
                if (attackingCard.GetCurrentDamage(attackingCard.PlayedType) <= maximumDamageThatCanReverse)
                {
                    return true;
                }
            }
        }
        
        if (reversalCard.Subtypes.Contains("ReversalSpecial"))
        {
            return IsValidTheConditionOfReversalSpecial(playManager, reversalCard, playedFrom);
        }

        return false;

    }

    private static bool IsValidTheConditionOfReversalSpecial(PlayManager playManager, Card reversalCard, string playedFrom)
    {
        Play currentPlay = playManager.CurrentPlay;
        Card attackingCard = currentPlay.AttackingCard;
        Player playerThatCanReverse = currentPlay.NotCurrentPlayer;
        switch (reversalCard.Title)
        {
            case "Elbow to the Face":
                if (attackingCard.PlayedType.Contains("Maneuver") && playerThatCanReverse.CalculateDamage(attackingCard) <= 7)
                {
                    return true;
                }
                break;
            case "Manager Interferes":
                if (attackingCard.PlayedType.Contains("Maneuver"))
                {
                    return true;
                }
                break;
            case "Chyna Interferes":
                if (attackingCard.PlayedType.Contains("Maneuver"))
                {
                    return true;
                }
                break;
            case "Clean Break" when attackingCard.Title == "Jockeying for Position":
                return true;
            case "Jockeying for Position" when attackingCard.Title == "Jockeying for Position":
                return true;
            case "Irish Whip" when attackingCard.Title == "Irish Whip":
                return true;
            case "Shoulder Block":
                if (playManager.PlayedCards.CardListSize >= 2 && attackingCard.PlayedType == "Maneuver")
                {
                    if (playManager.PlayedCards.GetPenultimateCard.Title == "Irish Whip")
                    {
                        return true;
                    }
                }
                break;
            case "Spear":
                if (playManager.PlayedCards.CardListSize >= 2 && attackingCard.PlayedType == "Maneuver")
                {
                    if (playManager.PlayedCards.GetPenultimateCard.Title == "Irish Whip")
                    {
                        return true;
                    }
                }
                break;
            case "Facebuster":
                if (playManager.PlayedCards.CardListSize >= 2 && attackingCard.PlayedType == "Maneuver" && playedFrom == "Hand")
                {
                    if (playManager.PlayedCards.GetPenultimateCard.Title == "Irish Whip")
                    {
                        return true;
                    }
                }
                break;
            case "Lou Thesz Press":
                if (playManager.PlayedCards.CardListSize >= 2 && attackingCard.PlayedType == "Maneuver" && playedFrom == "Hand")
                {
                    if (playManager.PlayedCards.GetPenultimateCard.Title == "Irish Whip")
                    {
                        return true;
                    }
                }
                break;
            case "Cross Body Block":
                if (playManager.PlayedCards.CardListSize >= 2 && attackingCard.PlayedType == "Maneuver")
                {
                    if (playManager.PlayedCards.GetPenultimateCard.Title == "Irish Whip")
                    {
                        return true;
                    }
                }
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
        }
        return false;
    }
}