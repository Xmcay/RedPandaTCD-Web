namespace RedPandaTCD_Web.Game;

public static class TournamentManager
{
    public const int TOTAL_ROUNDS = 3;


    public static void StartTournament(
        TournamentState state,
        Deck playerDeck)
    {
state.ResetTournamentProgress();

        state.IsActive =
            true;

        state.CurrentRound =
            1;

        state.LockedPlayerDeck =
            DeckCloner.Clone(playerDeck);

        state.CurrentOpponentName =
            GetOpponentName(
                state.CurrentRound);
    }


    public static string GetOpponentName(
        int round)
    {
        return round switch
        {
            1 =>
                "Professor Sage",

            2 =>
                "Captain Guardian",

            3 =>
                 "Rogue Nomad",

            _ =>
                ""
        };
    }


    public static Deck? GetOpponentDeck(
        int round)
    {
        return round switch
        {
            1 =>
                StarterDeckFactory
                    .CreateQueueStarter(),

            2 =>
                StarterDeckFactory
                    .CreateStackStarter(),

            3 =>
                StarterDeckFactory
                    .CreateRandomStarter(),

            _ =>
                null
        };
    }


    public static string GetRoundTitle(
        int round)
    {
        return round switch
        {
            1 =>
                "Opening Round",

            2 =>
                "Semifinal",

            3 =>
                "Final Round",

            _ =>
                "Grand Tournament"
        };
    }


    public static string GetOpponentDescription(
        int round)
    {
        return round switch
        {
            1 =>
                "Professor Sage challenges your planning against the Queue.",

            2 =>
                "Captain Guardian tests your strategy against the Stack.",

            3 =>
                "Rogue Nomad brings the unpredictability of the Random List.",

            _ =>
                ""
        };
    }


  public static bool AdvanceAfterWin(
    TournamentState state)
{
    if (!state.IsActive ||
        state.IsCompleted)
    {
        return false;
    }

    state.LastDefeatedOpponent =
        state.CurrentOpponentName;

    state.CompletedRounds =
        state.CurrentRound;

    if (state.CurrentRound >=
        TOTAL_ROUNDS)
    {
        state.IsCompleted =
            true;

        state.PlayerWonTournament =
            true;

        state.IsActive =
            false;

        state.HasMasterStrategistTitle =
            true;

        return false;
    }

    state.CurrentRound++;

    state.CurrentOpponentName =
        GetOpponentName(
            state.CurrentRound);

    return true;
}


    public static void EndAfterLoss(
        TournamentState state)
    {
        if (!state.IsActive ||
            state.IsCompleted)
        {
            return;
        }

        state.IsCompleted =
            true;

        state.PlayerWonTournament =
            false;

        state.IsActive =
            false;
    }


    public static string GetLossQuote(
        string opponentName)
    {
        return opponentName switch
        {
            "Professor Sage" =>
                "\"Planning is the first step toward victory.\"",

            "Captain Guardian" =>
                "\"A strategy must survive contact with reality.\"",

            "Rogue Nomad" =>
                "\"The strongest plans evolve.\"",

            _ =>
                ""
        };
    }


    public static string GetVictoryQuote(
        DeckStructure structure)
    {
        return structure switch
        {
            DeckStructure.Queue =>
                "\"The future belongs to those who prepare for it.\"",

            DeckStructure.PriorityQueue =>
                "\"You mastered the art of choosing what matters most.\"",

            DeckStructure.Stack =>
                "\"You learned to trust decisive action.\"",

            DeckStructure.RandomList =>
                "\"You turned chaos into opportunity.\"",

            DeckStructure.LinkedList =>
                "\"You reshaped destiny itself.\"",

            _ =>
                "\"Master Strategist.\""
        };
    }


   public static void ResetTournament(
    TournamentState state)
{
    state.ResetTournamentProgress();
}
}