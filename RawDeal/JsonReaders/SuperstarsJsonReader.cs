using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RawDeal.SuperStars;

namespace RawDeal.JsonReaders;

public static class SuperstarsJsonReader
{
    public static SuperStar[] GenerateAllSuperStarsArrayFromJson()
    {
        string pathSuperStarJson = Path.Combine("data", "superstar.json");
        string allSuperStarJson = File.ReadAllText(pathSuperStarJson);
        JArray jsonArrayAllSuperStar = JArray.Parse(allSuperStarJson);
        SuperStar[] allSuperStarArray = new SuperStar[jsonArrayAllSuperStar.Count];

        for (int i = 0; i < jsonArrayAllSuperStar.Count; i++)
        {
            JObject jObjectSuperStar = (JObject)jsonArrayAllSuperStar[i];
            SuperStar superstar = CreateSuperStarUsingName(jObjectSuperStar);
            allSuperStarArray[i] = superstar;
        }

        return allSuperStarArray;
    }

    private static SuperStar CreateSuperStarUsingName(JObject jObject)
    {
        string name = jObject["Name"].ToString();
        SuperStar superstar = null;
        switch (name)
        {
            case "HHH":
                superstar = JsonConvert.DeserializeObject<HHH>(jObject.ToString());
                break;
            case "KANE":
                superstar = JsonConvert.DeserializeObject<Kane>(jObject.ToString());
                break;
            case "THE ROCK":
                superstar = JsonConvert.DeserializeObject<TheRock>(jObject.ToString());
                break;
            case "THE UNDERTAKER":
                superstar = JsonConvert.DeserializeObject<Undertaker>(jObject.ToString());
                break;
            case "CHRIS JERICHO":
                superstar = JsonConvert.DeserializeObject<Jericho>(jObject.ToString());
                break;
            case "MANKIND":
                superstar = JsonConvert.DeserializeObject<Mankind>(jObject.ToString());
                break;
            case "STONE COLD STEVE AUSTIN":
                superstar = JsonConvert.DeserializeObject<StoneCold>(jObject.ToString());
                break;
        }
        return superstar;
    }

    public static List<string> GenerateSuperStarLogosList()
    {
        SuperStar[] allSuperStarsArray = GenerateAllSuperStarsArrayFromJson();
        List<string> superStarLogosList = new List<string>();
        foreach (SuperStar superstar in allSuperStarsArray) superStarLogosList.Add(superstar.Logo);
        return superStarLogosList;
    }
}