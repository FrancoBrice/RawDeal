using RawDeal.Cards;

namespace RawDeal.GameLogic;

public class PlayManager
{
    private List<Play> _plays;

    public PlayManager()
    {
        _plays = new List<Play>();
    }

    public void AddPlay(Play play)
    {
        _plays.Add(play);
    }

    public void ApplyPendingEffects()
    {
        if (_plays.Count > 0)
        {
            Play lastPlay = _plays[_plays.Count - 1];
            
            if (lastPlay.AttackingCard.Title == "Jockeying for Position")
            {
                _plays.RemoveAt(_plays.Count - 1);
            }
        }
    }
}