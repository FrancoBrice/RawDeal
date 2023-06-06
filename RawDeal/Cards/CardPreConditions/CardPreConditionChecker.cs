using RawDeal.GameLogic;
using RawDeal.Tools;

namespace RawDeal.Cards.CardPreConditions;

public static class CardPreConditionChecker
{


    public static bool IsPlayableCard(Card cardToCheck, PlayManager playManager)
    {
        Play currentPlay = playManager.CurrentPlay;
        Player currentPlayer = currentPlay.CurrentPlayer;
        Player notCurrentPlayer = currentPlay.NotCurrentPlayer;
        Card lastCardPlayed = new Card();
        if (currentPlay.PlayedCards.CardListSize >= 1)
        {
            lastCardPlayed = currentPlay.GetLastCard();
        }
        switch (cardToCheck.Title)
        {
            case "Back Body Drop":
            case "Leaping Knee to the Face":
            case "Cross Body Block":
                if (currentPlayer.TuplesWithPlayIdAndPlayedCards.Count < 1) return false;
                if (TupleManager.ExtractCard(currentPlayer.TuplesWithPlayIdAndPlayedCards[^1]).Title == "Irish Whip" && TupleManager.ExtractCardIndex(currentPlayer.TuplesWithPlayIdAndPlayedCards[^1]) > currentPlay.Id - 2)
                {
                    return true;
                }
                break;
            case "Spit At Opponent":
                return currentPlay.CurrentPlayer.GetHandSize() >= 2;
            case "Lionsault":
                const int lionasaultMinimumDamageOfPreviousCard = 4;
                if (currentPlay.PlayedCards.CardListSize < 1) break;
                return notCurrentPlayer.DamagesReceived[^1] >= lionasaultMinimumDamageOfPreviousCard && lastCardPlayed.PlayedType == "Maneuver";
            case "Austin Elbow Smash":
                const int austinElbowSmashMinimumDamageOfPreviousCard = 5;
                if (currentPlay.PlayedCards.CardListSize < 1) break;
                return notCurrentPlayer.DamagesReceived[^1] >= austinElbowSmashMinimumDamageOfPreviousCard && lastCardPlayed.PlayedType == "Maneuver";
            default:
                return true;
        }
        return false;
    }
}