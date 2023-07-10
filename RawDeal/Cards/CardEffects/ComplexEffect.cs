using RawDeal.Cards.CardEffects;
using RawDeal.GameLogic.Plays;
using RawDealView;

namespace RawDeal.Cards;

public class ComplexEffect: Effect
{
    private readonly List<Effect> _effects;
    private Game _game;
    
    public ComplexEffect(View view, Game game) : base(view)
    {
        _effects = new List<Effect>();
        _game = game;
    }

    protected override void ApplyCustomEffect(Play currentPlay)
    {
        foreach (Effect effect in _effects)
        {
            if (_game.IsGameOver()) return;
            effect.ApplyEffect(_game.GetCurrentPlay());
        }
    }
    
    public void Add(Effect effect)
    {
        _effects.Add(effect);
    }


}