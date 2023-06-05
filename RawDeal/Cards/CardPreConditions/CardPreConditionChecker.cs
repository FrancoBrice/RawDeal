using RawDeal.GameLogic;

namespace RawDeal.Cards.CardPreConditions;

public static class CardPreConditionChecker
{


    public static bool IsPlayableCard(Card cardToCheck, PlayManager playManager)
    {
        Play currentPlay = playManager.CurrentPlay;
        Player notCurrentPlayer = currentPlay.NotCurrentPlayer;
        Card lastCardPlayed = new Card();
        if (currentPlay.PlayedCards.CardListSize >= 1)
        {
            lastCardPlayed = currentPlay.GetLastCard();
        }
        switch (cardToCheck.Title)
        {
            case "Back Body Drop":
                if (playManager.PlayedCards.CardListSize < 1) return false;
                if (playManager.GetLastCard().Title == "Irish Whip") return true;
                break;
            case "Leaping Knee to the Face":
                if (playManager.PlayedCards.CardListSize < 1) return false;
                if (playManager.GetLastCard().Title == "Irish Whip") return true;
                break;
            case "Cross Body Block":
                if (playManager.PlayedCards.CardListSize < 1) return false;
                if (playManager.GetLastCard().Title == "Irish Whip") return true;
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