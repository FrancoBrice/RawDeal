using RawDeal.Cards;

namespace RawDeal.CardCollections;

public class RingArea : CardCollection
{
    public int GetFortitude()
    {
        int fortitude = 0;
        foreach (Card card in CardList)
        {
          int cardDamageInt = card.CurrentDamage;
          fortitude += cardDamageInt;
        }
        return fortitude;
    }
    
}