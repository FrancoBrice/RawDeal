using RawDeal.Cards;

namespace RawDeal.CardCollections;

public class RingArea : CardCollection
{
    public int? GetFortitude()
    {
        int? fortitude = 0;
        foreach (Card card in CardList)
        {
            int cardDamageInt;
            try
            {
                cardDamageInt = Convert.ToInt32(card.Damage);
                fortitude += cardDamageInt;
            }
            catch (FormatException)
            {
                // El valor de "Damage" no se puede convertir a entero
                // Puedes agregar aquí el manejo de error apropiado o tomar otra acción
                // Por ejemplo, puedes asignar un valor predeterminado a fortitude o lanzar una excepción personalizada.
            }
        }
        return fortitude;
    }

    
}