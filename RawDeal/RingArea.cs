namespace RawDeal;

public class RingArea : CardCollection
{
    public int GetFortitude()
    {
        int fortitude = 0;
        foreach (Card card in CardList)
        {
          string cardDamageString = card.Damage;
          int cardDamageInt = 0;
          if (cardDamageString != "#")
          {
              cardDamageInt = Convert.ToInt32(cardDamageString);
          }
          fortitude += cardDamageInt;
        }
        return fortitude;
    }
}