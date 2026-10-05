namespace RedPandaTCD_Web.Game;

public class PlayerSessionState
{
    public string PlayerName { get; set; } = "";

    public bool HasPlayerName =>
        !string.IsNullOrWhiteSpace(PlayerName);
}