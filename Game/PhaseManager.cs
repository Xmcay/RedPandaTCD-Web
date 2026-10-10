namespace RedPandaTCD_Web.Game;

public static class PhaseManager
{
    // Keep the original one-argument API for existing callers and Hot Reload.
    public static void CompleteCurrentPlayerPhase(
        MatchState state)
    {
        CompleteCurrentPlayerPhaseCore(
            state,
            resolveEndPhase: true);
    }

    // Use only after ResolveAnimated has already applied this player's End Phase.
    public static void CompleteCurrentPlayerPhaseAfterResolvedEnd(
        MatchState state)
    {
        CompleteCurrentPlayerPhaseCore(
            state,
            resolveEndPhase: false);
    }

    private static void CompleteCurrentPlayerPhaseCore(
        MatchState state,
        bool resolveEndPhase)
    {
        if (MatchManager.IsMatchOver(
                state.Player1,
                state.Player2))
        {
            return;
        }

        Player first =
            state.GetFirstPlayer();

        Player acting =
            state.GetActingPlayer();

        // Resolve the acting player's End Phase exactly once
        // before passing control or advancing the turn.
        if (state.CurrentPhase == MatchPhase.End && resolveEndPhase)
        {
            EndPhaseManager.Resolve(
                acting,
                state.TurnNumber,
                state.ActingPlayerIndex);

            if (MatchManager.IsMatchOver(
                    state.Player1,
                    state.Player2))
            {
                return;
            }
        }

        // The Initiative owner completed this phase.
        // Give the same phase to the second player.
        if (acting == first)
        {
            state.ActingPlayerIndex =
                state.ActingPlayerIndex == 1
                    ? 2
                    : 1;

            return;
        }

        // Both players completed the current phase.
        AdvancePhase(state);
    }

    private static void AdvancePhase(
        MatchState state)
    {
        bool startingNewTurn = false;

        switch (state.CurrentPhase)
        {
            case MatchPhase.Utility:
                state.CurrentPhase =
                    MatchPhase.Placement;
                break;

            case MatchPhase.Placement:
                state.CurrentPhase =
                    MatchPhase.Attack;
                break;

            case MatchPhase.Attack:
                state.CurrentPhase =
                    MatchPhase.End;
                break;

            case MatchPhase.End:
                state.TurnNumber++;

                state.CurrentPhase =
                    MatchPhase.Utility;

                startingNewTurn = true;
                break;
        }

        // TurnNumber has already changed here, so
        // GetFirstPlayer() now returns the new
        // Initiative owner for the upcoming turn.
        if (startingNewTurn)
        {
            Player initiativePlayer =
                state.GetFirstPlayer();

            BattleLog.WriteSystem(
                $"Initiative swapped → {initiativePlayer.Name}");

            BattleLog.WriteSystem(
                $"Turn {state.TurnNumber} begins.");
        }

        BattleLog.SetPhase(
            state.CurrentPhase);

        BattleLog.Write(
            $"--- {state.CurrentPhase} Phase begins ---");

        // Every phase begins with the current
        // Initiative owner acting first.
        state.ActingPlayerIndex =
            state.DoesPlayerOneActFirst()
                ? 1
                : 2;
    }

    public static bool RunBotPhaseIfNeeded(
        MatchState state)
    {
        if (MatchManager.IsMatchOver(
                state.Player1,
                state.Player2))
        {
            return false;
        }

        Player acting =
            state.GetActingPlayer();

        if (!acting.IsBot)
        {
            return false;
        }

        // Refresh replay context before this bot takes its turn.
        BattleLog.SetReplayContext(
            state.TurnNumber,
            state.ActingPlayerIndex,
            acting.Deck.Structure,
            state.Player1,
            state.Player2,
            state);

        Player opponent =
            state.GetOpponent();


        switch (state.CurrentPhase)
        {
            case MatchPhase.Utility:
                BotManager.RunUtility(
                    acting,
                    opponent);
                break;

            case MatchPhase.Placement:
                BotManager.RunPlacement(
                    acting);
                break;

            case MatchPhase.Attack:
                BotManager.RunAttack(
                    acting,
                    opponent);
                break;

            case MatchPhase.End:
                // EndPhaseManager.Resolve() is called by
                // CompleteCurrentPlayerPhase().
                break;
        }

        CompleteCurrentPlayerPhase(
            state);

        return true;
    }
}