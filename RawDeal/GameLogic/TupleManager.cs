using RawDeal.Cards;

namespace RawDeal.GameLogic;

public class TupleManager
{
    public Card ExtractCard((int, Card) tuple)
    {
        return tuple.Item2;
    }
    public int ExtractCardIndexInHand((int, Card) tuple)
    {
        return tuple.Item1;
    }
}