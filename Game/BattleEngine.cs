namespace RedPandaTCD_Web.Game;

public static class BattleEngine
{
    public static MatchState StartMatch(
        Player player1,
        Player player2,
        GameMode mode,
        bool? forcePlayerOneStarts = null)
    {
        // Clear any previous battle/replay log state.
        BattleLog.ClearPhaseActions();

        // Initialize both players and prepare their decks.
        MatchManager.InitializePlayer(
            player1,
            1);

        MatchManager.InitializePlayer(
            player2,
            2);

        // Register the exact decks used for this match.
        // ExportReplay() will include these fingerprints
        // so replay loading can verify exact deck identity.
        BattleLog.SetReplayDeck(
            1,
            player1.Deck);

        BattleLog.SetReplayDeck(
            2,
            player2.Deck);

        bool playerOneStarts =
            forcePlayerOneStarts ??
            GameRandom.Instance.Next(2) == 0;

        Player initiativePlayer =
            playerOneStarts
                ? player1
                : player2;

        BattleLog.WriteSystem(
            $"{initiativePlayer.Name} won the coin flip!");

        BattleLog.WriteSystem(
            $"Initiative → {initiativePlayer.Name}");

        BattleLog.WriteSystem(
            "Turn 1 begins.");

        MatchState state =
            new MatchState(
                player1,
                player2,
                playerOneStarts);

        return state;
    }


    public static Player? CompleteMatch(
        MatchState state)
    {
        if (!MatchManager.IsMatchOver(
                state.Player1,
                state.Player2))
        {
            return null;
        }

        if (state.IsCompleted)
        {
            return MatchManager.GetWinner(
                state.Player1,
                state.Player2);
        }

        if (MatchManager.IsDraw(
                state.Player1,
                state.Player2))
        {
            state.IsCompleted = true;
            state.IsDraw = true;

            BattleLog.Write(
                "Both players reached 0 Energy. The match ended in a Draw.");

            return null;
        }

        Player? winner =
            MatchManager.GetWinner(
                state.Player1,
                state.Player2);

        if (winner == null)
        {
            return null;
        }

        state.IsCompleted = true;
        state.IsDraw = false;

        winner.Wins++;

        if (winner == state.Player1)
        {
            state.Player2.Losses++;
        }
        else
        {
            state.Player1.Losses++;
        }

        return winner;
    }
}