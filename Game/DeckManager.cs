namespace RedPandaTCD_Web.Game;

public static class DeckManager
{
    public static List<Deck> Decks { get; } =
        new List<Deck>();

    public static void InitializeDefaultDecks()
    {
        if (Decks.Count > 0)
        {
            return;
        }

        Decks.Add(
            StarterDeckFactory.CreateQueueStarter());

        Decks.Add(
            StarterDeckFactory.CreateStackStarter());

        Decks.Add(
            StarterDeckFactory.CreatePriorityStarter());

        Decks.Add(
            StarterDeckFactory.CreateRandomStarter());

        Decks.Add(
            StarterDeckFactory.CreateLinkedStarter());
    }

    public static Deck? GetDeck(int index)
    {
        InitializeDefaultDecks();

        if (index < 0 ||
            index >= Decks.Count)
        {
            return null;
        }

        return DeckCloner.Clone(
            Decks[index]);
    }
    public static bool SaveDeck(
    Deck deck,
    string name,
    out string errorMessage)
{
    if (string.IsNullOrWhiteSpace(name))
    {
        errorMessage =
            "Deck name cannot be empty.";

        return false;
    }

    if (!DeckBuilder.ValidateDeck(
            deck,
            out errorMessage))
    {
        return false;
    }

    Deck savedDeck =
        DeckCloner.Clone(deck);

    savedDeck.Name =
        name.Trim();

    Decks.Add(savedDeck);

    errorMessage =
        string.Empty;

    return true;
}

public static bool DeleteDeck(
    int index)
{
    InitializeDefaultDecks();

    if (index < 0 ||
        index >= Decks.Count)
    {
        return false;
    }

    Decks.RemoveAt(index);

    return true;
}
public static bool UpdateDeck(
    int index,
    Deck deck,
    string name,
    out string errorMessage)
{
    InitializeDefaultDecks();

    if (index < 0 ||
        index >= Decks.Count)
    {
        errorMessage =
            "Deck not found.";

        return false;
    }

    if (string.IsNullOrWhiteSpace(name))
    {
        errorMessage =
            "Deck name cannot be empty.";

        return false;
    }

    if (!DeckBuilder.ValidateDeck(
            deck,
            out errorMessage))
    {
        return false;
    }

    Deck updatedDeck =
        DeckCloner.Clone(deck);

    updatedDeck.Name =
        name.Trim();

    Decks[index] =
        updatedDeck;

    errorMessage =
        string.Empty;

    return true;
}
public static void DeleteTutorialDeck()
{
    for (int i = Decks.Count - 1; i >= 0; i--)
    {
        if (string.Equals(
                Decks[i].Name,
                "Tutorial Deck",
                StringComparison.OrdinalIgnoreCase))
        {
            Decks.RemoveAt(i);
        }
    }
}
}