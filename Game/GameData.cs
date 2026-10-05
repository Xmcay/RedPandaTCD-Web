namespace RedPandaTCD_Web.Game;

public static class GameData
{
    public static string Player1Name { get; set; } =
        string.Empty;

    public static string Player2Name { get; set; } =
        string.Empty;

    public static bool HasPlayer1Name()
    {
        return !string.IsNullOrWhiteSpace(
            Player1Name);
    }

    public static bool HasPlayer2Name()
    {
        return !string.IsNullOrWhiteSpace(
            Player2Name);
    }

    public static void SetPlayerNames(
        string player1Name,
        string player2Name)
    {
        Player1Name =
            player1Name.Trim();

        Player2Name =
            player2Name.Trim();
    }

    public static void SetPlayer1Name(
        string playerName)
    {
        Player1Name =
            playerName.Trim();
    }
}