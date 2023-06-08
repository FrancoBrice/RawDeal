using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RawDeal.SuperStars;

namespace RawDeal.JsonReaders;

public static class SuperstarsJsonReader
{
    public static List<SuperStar> GenerateAllSuperStarsListFromJson()
    {
        string pathSuperStarJson = Path.Combine("data", "superstar.json");
        string allSuperStarJson = File.ReadAllText(pathSuperStarJson);
        JArray jsonArrayAllSuperStar = JArray.Parse(allSuperStarJson);
        List<SuperStar> allSuperStarList = new();

        foreach (JObject jObjectSuperStar in jsonArrayAllSuperStar)
        {
            SuperStar superstar = CreateSuperStarUsingName(jObjectSuperStar);
            allSuperStarList.Add(superstar);
        }

        return allSuperStarList;
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
        List<SuperStar> allSuperStarsList = GenerateAllSuperStarsListFromJson();
        List<string> superStarLogosList = new();
        foreach (SuperStar superstar in allSuperStarsList) superStarLogosList.Add(superstar.Logo);
        return superStarLogosList;
    }
}