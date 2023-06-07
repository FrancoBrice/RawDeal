using Newtonsoft.Json;
using RawDeal.Cards;

namespace RawDeal.JsonReaders;

public static class CardsJsonReader
{
    public static List<Card> GenerateAllCardsListFromCardsFromJson()
    {
        string pathCardsJson = Path.Combine("data", "cards.json");
        string allCardsJson = File.ReadAllText(pathCardsJson);
        List<Card> allCardsList = JsonConvert.DeserializeObject<List<Card>>(allCardsJson);
        foreach (var card in allCardsList)
        {
            card.SetDefaultValues();
        }
        return allCardsList;
    }
}
