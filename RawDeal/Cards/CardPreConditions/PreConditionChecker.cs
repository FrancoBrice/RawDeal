using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;
using RawDeal.Tools;

namespace RawDeal.Cards.CardPreConditions;

public static class PreConditionChecker
{
    public static bool IsPlayableCard(Card cardToCheck, PlayManager playManager)
    {
        Play currentPlay = playManager.CurrentPlay;
        Player currentPlayer = currentPlay.CurrentPlayer;
        Player notCurrentPlayer = currentPlay.NotCurrentPlayer;
        Card lastCardPlayed = new Card();
        if (currentPlay.PlayedCards.CardListSize >= 1) lastCardPlayed = currentPlay.GetLastCard();
        switch (cardToCheck.Title)
        {
            case "Back Body Drop":
            case "Leaping Knee to the Face":
            case "Cross Body Block":
                bool hasNotPlayedMoreThanOneCard =
                    currentPlayer.TuplesWithPlayIdAndPlayedCards.Count < 1;
                if (hasNotPlayedMoreThanOneCard) return false;
                var lastCardPlayedTuple = currentPlayer.TuplesWithPlayIdAndPlayedCards[^1];
                string titleOfLastCardPlayed = TupleManager.ExtractCard(lastCardPlayedTuple).Title;
                int playId = TupleManager.ExtractIndex(lastCardPlayedTuple);
                bool cardWasPlayedInCurrenOrPreviousPlay = playId > currentPlay.Id - 2;
                if (titleOfLastCardPlayed == "Irish Whip" && cardWasPlayedInCurrenOrPreviousPlay)
                    return true;
                break;
            case "Spit At Opponent":
                return currentPlay.CurrentPlayer.GetHandSize() >= 2;
            case "Lionsault":
                int minimumDamageOfPreviousCard = 4;
                if (currentPlay.PlayedCards.CardListSize < 1) break;
                return notCurrentPlayer.DamagesReceived[^1] >= minimumDamageOfPreviousCard &&
                       lastCardPlayed.PlayedType == "Maneuver";
            case "Austin Elbow Smash":
                minimumDamageOfPreviousCard = 5;
                if (currentPlay.PlayedCards.CardListSize < 1) break;
                return notCurrentPlayer.DamagesReceived[^1] >= minimumDamageOfPreviousCard &&
                       lastCardPlayed.PlayedType == "Maneuver";
            default:
                return true;
        }

        return false;
    }
}