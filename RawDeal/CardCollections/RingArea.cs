using RawDeal.Cards;

namespace RawDeal.CardCollections
{
    public class RingArea : CardCollection
    {
        public int GetFortitude()
        {
            int fortitude = 0;
            foreach (Card card in CardList)
            {
                if (int.TryParse(card.Damage, out int cardDamageInt))
                {
                    fortitude += cardDamageInt;
                }
            }
            return fortitude;
        }
    }
}