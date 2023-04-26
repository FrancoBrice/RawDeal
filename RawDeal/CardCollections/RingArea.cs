namespace RawDeal;

public class RingArea : CardCollection
{
    public int GetFortitude()
    {
        int fortitude = 0;
        foreach (Card card in CardList)
        {
          int cardDamageInt = TransformCardDamageToInt(card);
          fortitude += cardDamageInt;
        }
        return fortitude;
    }

    private int TransformCardDamageToInt(Card card)
    {
        string cardDamageString = card.Damage;
        if (cardDamageString != "#") return Convert.ToInt32(cardDamageString);
        return 0;
    }
}