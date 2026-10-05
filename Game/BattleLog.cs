namespace RedPandaTCD_Web.Game;

public class BattleLogEntry
{
    public MatchPhase Phase { get; }

    public string Message { get; }

    public bool IsSystem { get; }

    public string Label =>
        IsSystem
            ? "System"
            : Phase.ToString();

    public BattleLogEntry(
        MatchPhase phase,
        string message,
        bool isSystem = false)
    {
        Phase = phase;
        Message = message;
        IsSystem = isSystem;
    }
}

public static class BattleLog
{
    private static readonly List<BattleLogEntry>
        currentPhaseActions = new();

    private static MatchPhase currentPhase =
        MatchPhase.Utility;

    public static void SetPhase(
        MatchPhase phase)
    {
        currentPhase = phase;
    }

    public static void Write(
        string message)
    {
        currentPhaseActions.Add(
            new BattleLogEntry(
                currentPhase,
                message));
    }

    public static void WriteSystem(
        string message)
    {
        currentPhaseActions.Add(
            new BattleLogEntry(
                currentPhase,
                message,
                isSystem: true));
    }

    public static IReadOnlyList<BattleLogEntry>
        GetPhaseActions()
    {
        return currentPhaseActions.AsReadOnly();
    }

    public static void ClearPhaseActions()
    {
        currentPhaseActions.Clear();

        currentPhase =
            MatchPhase.Utility;
    }
}