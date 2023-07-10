using RawDeal.Cards;

namespace RawDeal.GameLogic;

public class IndexedCard
{
    public readonly int Index;
    public readonly Card Card;

    public IndexedCard(int index, Card card)
    {
        Index = index;
        Card = card;
    }
}