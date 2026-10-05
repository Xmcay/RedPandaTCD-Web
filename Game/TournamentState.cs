namespace RedPandaTCD_Web.Game;

public class TournamentState
{
    public bool IsActive { get; set; }

    public int CurrentRound { get; set; }

    public Deck? LockedPlayerDeck { get; set; }

    public string CurrentOpponentName { get; set; } =
        "";

    public bool IsCompleted { get; set; }

    public bool PlayerWonTournament { get; set; }

    public bool HasMasterStrategistTitle { get; set; }

    public string LastDefeatedOpponent { get; set; } =
        "";

    public int CompletedRounds { get; set; }


    public void ResetTournamentProgress()
    {
        IsActive =
            false;

        CurrentRound =
            0;

        LockedPlayerDeck =
            null;

        CurrentOpponentName =
            "";

        IsCompleted =
            false;

        PlayerWonTournament =
            false;

        LastDefeatedOpponent =
            "";

        CompletedRounds =
            0;

        /*
         * Do NOT reset HasMasterStrategistTitle here.
         *
         * Once earned, the title remains for the
         * rest of the current application session.
         */
    }


    public void ResetAll()
    {
        ResetTournamentProgress();

        HasMasterStrategistTitle =
            false;
    }
}