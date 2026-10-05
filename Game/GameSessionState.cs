namespace RedPandaTCD_Web.Game;

public class GameSessionState
{
    // Shared player/session information.
    public string PlayerName { get; set; } = "";

    public bool HasPlayerName =>
        !string.IsNullOrWhiteSpace(PlayerName);

    // Training setup.
    public Deck? TrainingPlayerDeck { get; set; }

    public Deck? TrainingBotDeck { get; set; }

    public MatchState? CurrentMatch { get; set; }

    // Grand Tournament progress for the current app session.
public TournamentState Tournament { get; } =
new TournamentState();

public TutorialState Tutorial { get; } =
    new TutorialState();

    public void ResetTraining()
    {
        TrainingPlayerDeck = null;
        TrainingBotDeck = null;
    }
}