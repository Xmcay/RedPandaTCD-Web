namespace RedPandaTCD_Web.Game;

public class ReplayMatchState
{
    public MatchState Match { get; }

    public List<BattleLogEntry> Events { get; }

    public int CurrentEventIndex { get; private set; }

    public bool IsPlaying { get; set; }

    public bool IsFinished =>
        CurrentEventIndex >= Events.Count;

    public BattleLogEntry? CurrentEvent =>
        CurrentEventIndex >= 0 &&
        CurrentEventIndex < Events.Count
            ? Events[CurrentEventIndex]
            : null;

    public Player Player1 =>
        Match.Player1;

    public Player Player2 =>
        Match.Player2;

    public int TurnNumber =>
        Match.TurnNumber;

    public MatchPhase CurrentPhase =>
        Match.CurrentPhase;

    public ReplayMatchState(
        Player player1,
        Player player2,
        bool playerOneStarts,
        IEnumerable<BattleLogEntry> events)
    {
        Match = new MatchState(
            player1,
            player2,
            playerOneStarts);

        Events = events.ToList();
        CurrentEventIndex = 0;
        IsPlaying = false;
    }

    public void Reset()
    {
        CurrentEventIndex = 0;
        IsPlaying = false;

        Match.TurnNumber = 1;
        Match.CurrentPhase = MatchPhase.Utility;
        Match.IsCompleted = false;
        Match.IsDraw = false;
        Match.ActingPlayerIndex =
            Match.DoesPlayerOneActFirst()
                ? 1
                : 2;
    }

    public bool MoveNext()
    {
        if (IsFinished)
        {
            return false;
        }

        CurrentEventIndex++;

        return true;
    }

    public bool MovePrevious()
    {
        if (CurrentEventIndex <= 0)
        {
            return false;
        }

        CurrentEventIndex--;

        return true;
    }

    public void JumpToStart()
    {
        CurrentEventIndex = 0;
        IsPlaying = false;
    }

    public void JumpToEnd()
    {
        CurrentEventIndex = Events.Count;
        IsPlaying = false;
    }

    public void SetEventIndex(int index)
    {
        if (index < 0)
        {
            CurrentEventIndex = 0;
            return;
        }

        if (index > Events.Count)
        {
            CurrentEventIndex = Events.Count;
            return;
        }

        CurrentEventIndex = index;
    }

    public void UpdateMatchContext(BattleLogEntry entry)
    {
        if (entry.TurnNumber > 0)
        {
            Match.TurnNumber = entry.TurnNumber;
        }

        if (entry.PlayerIndex == 1 ||
            entry.PlayerIndex == 2)
        {
            Match.ActingPlayerIndex =
                entry.PlayerIndex;
        }

        Match.CurrentPhase = entry.Phase;

        if (entry.EventType ==
            BattleLogEventType.MatchEnd)
        {
            Match.IsCompleted = true;
        }
    }
}