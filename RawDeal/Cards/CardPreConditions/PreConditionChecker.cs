using RawDeal.GameLogic;
using RawDeal.GameLogic.Players;
using RawDeal.GameLogic.Plays;

namespace RawDeal.Cards.CardPreConditions;

public static class PreConditionChecker
{
    public static bool IsPlayableCard(Card cardToCheck, PlayManager playManager)
    {
        Play currentPlay = playManager.GetCurrentPlay();
        Player currentPlayer = currentPlay.CurrentPlayer;
        Player notCurrentPlayer = currentPlay.NotCurrentPlayer;
        if (!cardToCheck.TypeIsPlayable()) return false;
        if (cardToCheck.GetCurrentFortitude(currentPlay, "") > currentPlayer.Fortitude) return false;
        Card lastCardPlayed = new Card();
        if (currentPlay.PlayedCards.Count >= 1) lastCardPlayed = currentPlay.GetLastCard();
        switch (cardToCheck.Title)
        {
            case "Back Body Drop":
            case "Leaping Knee to the Face":
            case "Cross Body Block":
                bool hasNotPlayedMoreThanOneCard =
                    currentPlayer.PlayIdAndPlayedCards.Count == 0;
                if (hasNotPlayedMoreThanOneCard) return false;
                IndexedCard lastCardIndexed = currentPlayer.PlayIdAndPlayedCards[^1];
                string titleOfLastCardPlayed = lastCardIndexed.Card.Title;
                int playId = lastCardIndexed.Index;
                bool cardWasPlayedInCurrenOrPreviousPlay = playId > currentPlay.Id - 2;
                if (titleOfLastCardPlayed == "Irish Whip" && cardWasPlayedInCurrenOrPreviousPlay)
                    return true;
                break;
            case "Spit At Opponent":
                const int minimumHandSizeSpitAtOpponent = 2;
                return currentPlayer.GetHandSize() >= minimumHandSizeSpitAtOpponent;
            case "Lionsault":
            case "Kane's Flying Clothesline":
                int minimumDamageOfPreviousCard = 4;
                if (currentPlay.PlayedCardsCount < 1) break;
                return notCurrentPlayer.LastDamageReceived() >= minimumDamageOfPreviousCard &&
                       lastCardPlayed.PlayedType == "Maneuver";
            case "Undertaker's Flying Clothesline":
                const int minimumDamageOfPreviousCardUFC = 5;
                if (currentPlay.PlayedCardsCount < 1) break;
                return notCurrentPlayer.LastDamageReceived() >= minimumDamageOfPreviousCardUFC &&
                       lastCardPlayed.PlayedType == "Maneuver";
            case "Austin Elbow Smash":
                minimumDamageOfPreviousCard = 5;
                if (currentPlay.PlayedCardsCount < 1) break;
                return notCurrentPlayer.LastDamageReceived() >= minimumDamageOfPreviousCard &&
                       lastCardPlayed.PlayedType == "Maneuver";
            case "The People's Elbow" when cardToCheck.PlayedType == "Maneuver":
                return currentPlayer.GetAllRingAreaCards().Any(card => card.Title == "Rock Bottom");
            case "Shake It Off":
                return currentPlayer.Fortitude < notCurrentPlayer.Fortitude;
            default:
                return true;
        }

        return false;
    }
}