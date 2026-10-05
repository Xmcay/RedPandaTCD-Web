namespace RedPandaTCD_Web.Game;

public class MatchState
{
    public Player Player1 { get; }

    public Player Player2 { get; }

    public int TurnNumber { get; set; } = 1;

    public MatchPhase CurrentPhase { get; set; } =
        MatchPhase.Utility;

    public bool PlayerOneStarts { get; set; }

    public int ActingPlayerIndex { get; set; }

    public bool IsCompleted { get; set; }
    public bool IsDraw { get; set; }

    public MatchState(
        Player player1,
        Player player2,
        bool playerOneStarts)
    {
        Player1 = player1;
        Player2 = player2;
        PlayerOneStarts = playerOneStarts;

        ActingPlayerIndex =
            DoesPlayerOneActFirst()
            ? 1
            : 2;
    }

    public bool DoesPlayerOneActFirst()
    {
        return PlayerOneStarts
            ? TurnNumber % 2 == 1
            : TurnNumber % 2 == 0;
    }

    public Player GetFirstPlayer()
    {
        return DoesPlayerOneActFirst()
            ? Player1
            : Player2;
    }

    public Player GetSecondPlayer()
    {
        return DoesPlayerOneActFirst()
            ? Player2
            : Player1;
    }

    public Player GetActingPlayer()
    {
        return ActingPlayerIndex == 1
            ? Player1
            : Player2;
    }

    public Player GetOpponent()
    {
        return ActingPlayerIndex == 1
            ? Player2
            : Player1;
    }
}