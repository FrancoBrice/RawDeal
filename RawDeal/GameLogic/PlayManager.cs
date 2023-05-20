using RawDealView;

namespace RawDeal.GameLogic;

public class PlayManager
{
    private List<Play> _plays;
    private View _view;
    public Play CurrentPlay => _plays.Count > 0 ? _plays[^1] : null;

    public Play PreviousPlay => _plays.Count > 1 ? _plays[^2] : null;
    public int NextPlayId;

    public PlayManager(View view)
    {
        _view = view;
        _plays = new List<Play>();
        NextPlayId = 1;
    }

    public void AddPlay(Play play)
    {
        play.Id = NextPlayId;
        NextPlayId++;
        _plays.Add(play);
        ApplyPendingEffectsIfPossible();
    }

    public void ApplyPendingEffectsIfPossible()
    {
        if (_plays.Count < 2) return;
        if (!PreviousPlay.IsAPendingEffect) return;
        if (PreviousPlay.ReversalCard is { Title: "Jockeying for Position" })
        {
            var pendingEffect = PreviousPlay.PendingEffect; 
            CurrentPlay.SetPendingEffect(pendingEffect);
            pendingEffect.ApplyEffect(PreviousPlay);
        }
    }
    
    public void RemoveEffectsOnCards()
    {
        foreach (Player player in CurrentPlay.Players)
        {
            player.SetDefaultValuesInCards();
        }
    }
}