namespace RedPandaTCD_Web.Game;

using System.Security.Cryptography;
using System.Text;

public enum BattleLogEventType
{
    General,
    MatchStart,
    TurnStart,
    PhaseStart,
    Draw,
    Utility,
    Placement,
    Attack,
    Ability,
    Damage,
    EnergyChange,
    CharacterDefeated,
    EndPhase,
    MatchEnd
}

public class BattleLogEntry
{
    // ==========================
    // DISPLAY INFORMATION
    // ==========================

    public MatchPhase Phase { get; }

    public string Message { get; }

    public bool IsSystem { get; }

    public bool ShowInLog { get; }

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


    // ==========================
    // REPLAY STATE INFORMATION
    // ==========================

    public int? TargetPlayerIndex { get; }

    public string? TargetCardName { get; }

    public int? Amount { get; }

    public int? SlotNumber { get; }

    public int? TargetSlotNumber { get; }

    public int? CharacterHp { get; }

    public int? PlayerEnergy { get; }

    public int? PlayerShield { get; }

    public int? OpponentEnergy { get; }

    public int? OpponentShield { get; }


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
        DeckStructure? deckStructure = null,

        int? targetPlayerIndex = null,
        string? targetCardName = null,
        int? amount = null,
        int? slotNumber = null,
        int? targetSlotNumber = null,
        int? characterHp = null,
        int? playerEnergy = null,
        int? playerShield = null,
        int? opponentEnergy = null,
        int? opponentShield = null,

        bool showInLog = true)
    {
        Phase = phase;
        Message = message;
        IsSystem = isSystem;
        ShowInLog = showInLog;

        TurnNumber = turnNumber;
        PlayerIndex = playerIndex;
        EventType = eventType;

        CardName = cardName;
        CardType = cardType;
        DeckStructure = deckStructure;

        TargetPlayerIndex = targetPlayerIndex;
        TargetCardName = targetCardName;
        Amount = amount;

        SlotNumber = slotNumber;
        TargetSlotNumber = targetSlotNumber;

        CharacterHp = characterHp;

        PlayerEnergy = playerEnergy;
        PlayerShield = playerShield;

        OpponentEnergy = opponentEnergy;
        OpponentShield = opponentShield;
    }
}


// ==========================================================
// REPLAY DECK INFORMATION
// ==========================================================

public class ReplayDeckInfo
{
    public int PlayerIndex { get; }

    public string DeckName { get; }

    public DeckStructure Structure { get; }

    public string Fingerprint { get; }

    public ReplayDeckInfo(
        int playerIndex,
        string deckName,
        DeckStructure structure,
        string fingerprint)
    {
        PlayerIndex = playerIndex;
        DeckName = deckName;
        Structure = structure;
        Fingerprint = fingerprint;
    }
}


public static class BattleLog
{
    private const string ReplayHeader =
        "REDPANDA_REPLAY_V1";

    private static readonly List<BattleLogEntry>
        currentPhaseActions = new();

    private static MatchPhase currentPhase =
        MatchPhase.Utility;


    // ==========================
    // REPLAY CONTEXT
    // ==========================

    public static int CurrentTurnNumber { get; set; }

    public static int CurrentPlayerIndex { get; set; } = -1;

    private static DeckStructure? currentDeckStructure;


    // ==========================
    // REPLAY DECK IDENTITY
    // ==========================

    private static ReplayDeckInfo? replayPlayer1Deck;

    private static ReplayDeckInfo? replayPlayer2Deck;


    // ==========================
    // SET REPLAY CONTEXT
    // ==========================

    public static void SetReplayContext(
        int turnNumber,
        int playerIndex,
        DeckStructure? deckStructure = null)
    {
        CurrentTurnNumber = turnNumber;
        CurrentPlayerIndex = playerIndex;
        currentDeckStructure = deckStructure;
    }


    public static int GetOpponentPlayerIndex()
    {
        if (CurrentPlayerIndex == 1)
        {
            return 2;
        }

        if (CurrentPlayerIndex == 2)
        {
            return 1;
        }

        return -1;
    }


    public static DeckStructure? GetCurrentDeckStructure()
    {
        return currentDeckStructure;
    }


    // ==========================
    // DECK REGISTRATION
    // ==========================

    public static void SetReplayDeck(
        int playerIndex,
        Deck deck)
    {
        if (playerIndex != 1 &&
            playerIndex != 2)
        {
            return;
        }

        ReplayDeckInfo deckInfo =
            CreateReplayDeckInfo(
                playerIndex,
                deck);

        if (playerIndex == 1)
        {
            replayPlayer1Deck = deckInfo;
        }
        else
        {
            replayPlayer2Deck = deckInfo;
        }
    }


    public static ReplayDeckInfo? GetReplayDeck(
        int playerIndex)
    {
        return playerIndex switch
        {
            1 => replayPlayer1Deck,
            2 => replayPlayer2Deck,
            _ => null
        };
    }


    public static ReplayDeckInfo? GetPlayer1ReplayDeck()
    {
        return replayPlayer1Deck;
    }


    public static ReplayDeckInfo? GetPlayer2ReplayDeck()
    {
        return replayPlayer2Deck;
    }


    public static bool ReplayDeckMatches(
        int playerIndex,
        Deck deck)
    {
        ReplayDeckInfo? recordedDeck =
            GetReplayDeck(playerIndex);

        if (recordedDeck == null)
        {
            return false;
        }

        string fingerprint =
            CreateDeckFingerprint(deck);

        return recordedDeck.Fingerprint ==
               fingerprint;
    }


    // ==========================
    // DECK FINGERPRINT
    // ==========================

    public static string CreateDeckFingerprint(
        Deck deck)
    {
        StringBuilder signature =
            new StringBuilder();

        signature.Append(
            deck.Structure.ToString());

        signature.Append('\n');

        for (int i = 0;
             i < deck.Cards.Count;
             i++)
        {
            Card card = deck.Cards[i];

            signature.Append(i);
            signature.Append(':');

            signature.Append(card.Id ?? "");

            signature.Append('|');

            signature.Append(card.Type.ToString());

            signature.Append('|');

            signature.Append(card.Name ?? "");

            signature.Append('\n');
        }

        byte[] data =
            Encoding.UTF8.GetBytes(
                signature.ToString());

        byte[] hash =
            SHA256.HashData(data);

        return Convert.ToHexString(
            hash);
    }


    private static ReplayDeckInfo
        CreateReplayDeckInfo(
            int playerIndex,
            Deck deck)
    {
        return new ReplayDeckInfo(
            playerIndex,
            deck.Name ?? "",
            deck.Structure,
            CreateDeckFingerprint(deck));
    }


    // ==========================
    // PHASE MANAGEMENT
    // ==========================

    public static void SetPhase(
        MatchPhase phase)
    {
        currentPhase = phase;
    }


    // ==========================
    // BASIC LOGGING
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
    // STRUCTURED REPLAY LOGGING
    // ==========================

    public static void WriteReplay(
        string message,
        int turnNumber,
        int playerIndex,
        BattleLogEventType eventType,

        string? cardName = null,
        CardType? cardType = null,
        DeckStructure? deckStructure = null,

        bool isSystem = false,

        int? targetPlayerIndex = null,
        string? targetCardName = null,
        int? amount = null,

        int? slotNumber = null,
        int? targetSlotNumber = null,

        int? characterHp = null,

        int? playerEnergy = null,
        int? playerShield = null,

        int? opponentEnergy = null,
        int? opponentShield = null,

        bool showInLog = true)
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
                deckStructure,

                targetPlayerIndex,
                targetCardName,
                amount,

                slotNumber,
                targetSlotNumber,

                characterHp,

                playerEnergy,
                playerShield,

                opponentEnergy,
                opponentShield,

                showInLog));
    }


    // ==========================
    // CONVENIENCE REPLAY METHODS
    // ==========================

    public static void WriteDraw(
        string message,
        int turnNumber,
        int playerIndex,
        string cardName,
        CardType cardType,
        DeckStructure deckStructure)
    {
        WriteReplay(
            message,
            turnNumber,
            playerIndex,
            BattleLogEventType.Draw,
            cardName,
            cardType,
            deckStructure,
            showInLog: false);
    }


    public static void WriteCardAction(
        string message,
        int turnNumber,
        int playerIndex,
        BattleLogEventType eventType,
        string cardName,
        CardType cardType,
        DeckStructure deckStructure,
        int? slotNumber = null,
        int? targetSlotNumber = null)
    {
        WriteReplay(
            message,
            turnNumber,
            playerIndex,
            eventType,
            cardName,
            cardType,
            deckStructure,
            slotNumber: slotNumber,
            targetSlotNumber: targetSlotNumber);
    }


    public static void WriteDamage(
        string message,
        int turnNumber,
        int playerIndex,
        int targetPlayerIndex,
        int amount,
        string? cardName = null,
        CardType? cardType = null)
    {
        WriteReplay(
            message,
            turnNumber,
            playerIndex,
            BattleLogEventType.Damage,
            cardName,
            cardType,
            targetPlayerIndex: targetPlayerIndex,
            amount: amount);
    }


    public static void WriteEnergyChange(
        string message,
        int turnNumber,
        int playerIndex,
        int amount,
        int playerEnergy)
    {
        WriteReplay(
            message,
            turnNumber,
            playerIndex,
            BattleLogEventType.EnergyChange,
            amount: amount,
            playerEnergy: playerEnergy);
    }


    // ==========================
    // LOG ACCESS
    // ==========================

    public static IReadOnlyList<BattleLogEntry>
        GetPhaseActions()
    {
        return currentPhaseActions
            .Where(entry => entry.ShowInLog)
            .ToList()
            .AsReadOnly();
    }


    public static IReadOnlyList<BattleLogEntry>
        GetReplayEvents()
    {
        return currentPhaseActions
            .Where(entry =>
                entry.TurnNumber > 0 &&
                entry.PlayerIndex >= 1)
            .ToList()
            .AsReadOnly();
    }


    // ==========================
    // REPLAY EXPORT
    // ==========================

    public static string ExportReplay()
    {
        List<string> lines = new();

        lines.Add(ReplayHeader);

        // Deck identity metadata.
        if (replayPlayer1Deck != null)
        {
            lines.Add(
                SerializeDeckInfo(
                    replayPlayer1Deck));
        }

        if (replayPlayer2Deck != null)
        {
            lines.Add(
                SerializeDeckInfo(
                    replayPlayer2Deck));
        }

        lines.Add(
            $"EVENT_COUNT={currentPhaseActions.Count}");

        lines.Add("");

        foreach (BattleLogEntry entry in currentPhaseActions)
        {
            lines.Add(
                SerializeEntry(entry));
        }

        return string.Join(
            Environment.NewLine,
            lines);
    }


    private static string SerializeDeckInfo(
        ReplayDeckInfo deckInfo)
    {
        return string.Join(
            "|",
            "DECK",
            deckInfo.PlayerIndex,
            Escape(deckInfo.DeckName),
            deckInfo.Structure.ToString(),
            deckInfo.Fingerprint);
    }


    private static string SerializeEntry(
        BattleLogEntry entry)
    {
        return string.Join(
            "|",
            Escape(entry.Phase.ToString()),
            entry.TurnNumber,
            entry.PlayerIndex,
            Escape(entry.EventType.ToString()),
            entry.IsSystem ? "1" : "0",

            Escape(entry.CardName),
            entry.CardType?.ToString() ?? "",
            entry.DeckStructure?.ToString() ?? "",

            entry.TargetPlayerIndex?.ToString() ?? "",
            Escape(entry.TargetCardName),

            entry.Amount?.ToString() ?? "",

            entry.SlotNumber?.ToString() ?? "",
            entry.TargetSlotNumber?.ToString() ?? "",

            entry.CharacterHp?.ToString() ?? "",

            entry.PlayerEnergy?.ToString() ?? "",
            entry.PlayerShield?.ToString() ?? "",

            entry.OpponentEnergy?.ToString() ?? "",
            entry.OpponentShield?.ToString() ?? "",

            Escape(entry.Message));
    }


    // ==========================
    // REPLAY IMPORT
    // ==========================

    public static List<BattleLogEntry> ImportReplay(
        string replayData)
    {
        List<BattleLogEntry> entries = new();

        replayPlayer1Deck = null;
        replayPlayer2Deck = null;

        if (string.IsNullOrWhiteSpace(replayData))
        {
            return entries;
        }

        string[] lines =
            replayData.Split(
                new[]
                {
                    "\r\n",
                    "\n"
                },
                StringSplitOptions.None);

        if (lines.Length == 0 ||
            lines[0].Trim() != ReplayHeader)
        {
            return entries;
        }

        foreach (string rawLine in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                continue;
            }

            if (rawLine.StartsWith("DECK|"))
            {
                ReplayDeckInfo? deckInfo =
                    DeserializeDeckInfo(rawLine);

                if (deckInfo != null)
                {
                    if (deckInfo.PlayerIndex == 1)
                    {
                        replayPlayer1Deck =
                            deckInfo;
                    }
                    else if (deckInfo.PlayerIndex == 2)
                    {
                        replayPlayer2Deck =
                            deckInfo;
                    }
                }

                continue;
            }

            if (rawLine.StartsWith("EVENT_COUNT="))
            {
                continue;
            }

            BattleLogEntry? entry =
                DeserializeEntry(rawLine);

            if (entry != null)
            {
                entries.Add(entry);
            }
        }

        return entries;
    }


    private static ReplayDeckInfo?
        DeserializeDeckInfo(
            string line)
    {
        string[] parts =
            SplitEscapedLine(line);

        if (parts.Length < 5 ||
            parts[0] != "DECK")
        {
            return null;
        }

        if (!int.TryParse(
                parts[1],
                out int playerIndex))
        {
            return null;
        }

        if (playerIndex != 1 &&
            playerIndex != 2)
        {
            return null;
        }

        if (!Enum.TryParse(
                parts[3],
                out DeckStructure structure))
        {
            return null;
        }

        string deckName =
            Unescape(parts[2]);

        string fingerprint =
            parts[4].Trim();

        if (string.IsNullOrWhiteSpace(
                fingerprint))
        {
            return null;
        }

        return new ReplayDeckInfo(
            playerIndex,
            deckName,
            structure,
            fingerprint);
    }


    private static BattleLogEntry? DeserializeEntry(
        string line)
    {
        string[] parts =
            SplitEscapedLine(line);

        if (parts.Length < 19)
        {
            return null;
        }

        if (!Enum.TryParse(
                parts[0],
                out MatchPhase phase))
        {
            return null;
        }

        if (!int.TryParse(
                parts[1],
                out int turnNumber))
        {
            return null;
        }

        if (!int.TryParse(
                parts[2],
                out int playerIndex))
        {
            return null;
        }

        if (!Enum.TryParse(
                parts[3],
                out BattleLogEventType eventType))
        {
            return null;
        }

        bool isSystem =
            parts[4] == "1";

        CardType? cardType = null;

        if (!string.IsNullOrWhiteSpace(parts[6]) &&
            Enum.TryParse(
                parts[6],
                out CardType parsedCardType))
        {
            cardType = parsedCardType;
        }

        DeckStructure? deckStructure = null;

        if (!string.IsNullOrWhiteSpace(parts[7]) &&
            Enum.TryParse(
                parts[7],
                out DeckStructure parsedDeckStructure))
        {
            deckStructure = parsedDeckStructure;
        }

        return new BattleLogEntry(
            phase,
            Unescape(parts[18]),
            isSystem,

            turnNumber,
            playerIndex,
            eventType,

            Unescape(parts[5]),
            cardType,
            deckStructure,

            ParseNullableInt(parts[8]),
            Unescape(parts[9]),

            ParseNullableInt(parts[10]),

            ParseNullableInt(parts[11]),
            ParseNullableInt(parts[12]),

            ParseNullableInt(parts[13]),

            ParseNullableInt(parts[14]),
            ParseNullableInt(parts[15]),

            ParseNullableInt(parts[16]),
            ParseNullableInt(parts[17]),

            showInLog:
                eventType != BattleLogEventType.Draw);
    }


    // ==========================
    // ESCAPED LINE PARSING
    // ==========================

    private static string[] SplitEscapedLine(
        string line)
    {
        List<string> parts = new();
        StringBuilder current =
            new StringBuilder();

        bool escaped = false;

        foreach (char character in line)
        {
            if (escaped)
            {
                current.Append('\\');
                current.Append(character);
                escaped = false;
                continue;
            }

            if (character == '\\')
            {
                escaped = true;
                continue;
            }

            if (character == '|')
            {
                parts.Add(
                    current.ToString());

                current.Clear();
                continue;
            }

            current.Append(character);
        }

        if (escaped)
        {
            current.Append('\\');
        }

        parts.Add(
            current.ToString());

        return parts.ToArray();
    }


    // ==========================
    // SERIALIZATION HELPERS
    // ==========================

    private static string Escape(
        string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "";
        }

        return value
            .Replace("\\", "\\\\")
            .Replace("|", "\\|")
            .Replace("\r", "\\r")
            .Replace("\n", "\\n");
    }


    private static string Unescape(
        string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "";
        }

        return value
            .Replace("\\n", "\n")
            .Replace("\\r", "\r")
            .Replace("\\|", "|")
            .Replace("\\\\", "\\");
    }


    private static int? ParseNullableInt(
        string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return int.TryParse(
            value,
            out int result)
                ? result
                : null;
    }


    // ==========================
    // CLEAR
    // ==========================

    public static void ClearPhaseActions()
    {
        currentPhaseActions.Clear();

        currentPhase =
            MatchPhase.Utility;

        CurrentTurnNumber = 0;
        CurrentPlayerIndex = -1;
        currentDeckStructure = null;

        replayPlayer1Deck = null;
        replayPlayer2Deck = null;
    }
}