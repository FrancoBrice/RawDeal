namespace RawDeal.Cards.CardEffects;

public static class EffectsApplier
{
    public static void ApplyAssignedEffects(Game game, List<Effect> assignedEffects)
    {
        foreach (Effect effect in assignedEffects)
        {
            if (game.IsGameOver()) return;
            effect.ApplyEffect(game.GetCurrentPlay());
        }
    }
}