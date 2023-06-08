using RawDeal.Cards;

namespace RawDeal.Tools;

public static class TupleManager
{
    public static Card ExtractCard((int, Card) tuple)
    {
        return tuple.Item2;
    }

    public static int ExtractIndex((int, Card) tuple)
    {
        return tuple.Item1;
    }
}