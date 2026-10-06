namespace RedPandaTCD_Web.Game;

public enum BattleLogEventType
{
    General,
    Draw,
    Utility,
    Placement,
    Attack,
    Ability,
    Damage,
    EnergyChange,
    CharacterDefeated,
    EndPhase
}

public class BattleLogEntry
{
    // ==========================
    // DISPLAY INFORMATION
    // ==========================

    public MatchPhase Phase { get; }

    public string Message { get; }

    public bool IsSystem { get; }

    public string Label =>
        IsSystem
            ? "System"
            : Phase.ToString();


    // ==========================
    // REPLAY INFORMATION
    // ==========================

    public int TurnNumber { get; }

    public int PlayerIndex { get; }

    public BattleLogEventType EventType { get; }

    public string? CardName { get; }

    public CardType? CardType { get; }

    public DeckStructure? DeckStructure { get; }


    public BattleLogEntry(
        MatchPhase phase,
        string message,
        bool isSystem = false,
        int turnNumber = 0,
        int playerIndex = -1,
        BattleLogEventType eventType =
            BattleLogEventType.General,
        string? cardName = null,
        CardType? cardType = null,
        DeckStructure? deckStructure = null)
    {
        Phase = phase;
        Message = message;
        IsSystem = isSystem;

        TurnNumber = turnNumber;
        PlayerIndex = playerIndex;
        EventType = eventType;
        CardName = cardName;
        CardType = cardType;
        DeckStructure = deckStructure;
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


    // ==========================
    // EXISTING LOGGING
    // ==========================

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


    // ==========================
    // REPLAY LOGGING
    // ==========================

    public static void WriteReplay(
        string message,
        int turnNumber,
        int playerIndex,
        BattleLogEventType eventType,
        string? cardName = null,
        CardType? cardType = null,
        DeckStructure? deckStructure = null,
        bool isSystem = false)
    {
        currentPhaseActions.Add(
            new BattleLogEntry(
                currentPhase,
                message,
                isSystem,
                turnNumber,
                playerIndex,
                eventType,
                cardName,
                cardType,
                deckStructure));
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