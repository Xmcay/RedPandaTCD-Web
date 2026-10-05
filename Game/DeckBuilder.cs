namespace RedPandaTCD_Web.Game;

public static class DeckBuilder
{
    public static bool ValidateDeck(
        Deck deck,
        out string errorMessage)
    {
        if (deck.Cards.Count < 10 ||
            deck.Cards.Count > 15)
        {
            errorMessage =
                "Deck must contain 10-15 cards.";

            return false;
        }

        List<Card> characters =
            deck.Cards
                .Where(c =>
                    c.Type == CardType.Character)
                .ToList();

        if (characters.Count < 1 ||
            characters.Count > 2)
        {
            errorMessage =
                "Must contain 1-2 Characters.";

            return false;
        }

        if (characters
            .GroupBy(c => c.Id)
            .Any(g => g.Count() > 1))
        {
            errorMessage =
                "Characters must be unique.";

            return false;
        }

        List<Card> attacks =
            deck.Cards
                .Where(c =>
                    c.Type == CardType.Attack)
                .ToList();

        if (attacks.Count < 2)
        {
            errorMessage =
                "Must contain at least 2 Attacks.";

            return false;
        }

        if (attacks
            .GroupBy(c => c.Id)
            .Any(g => g.Count() > 2))
        {
            errorMessage =
                "Attacks may only have 2 copies.";

            return false;
        }

        List<Card> utilities =
            deck.Cards
                .Where(c =>
                    c.Type == CardType.Utility)
                .ToList();

        if (utilities.Count > 6)
        {
            errorMessage =
                "Maximum 6 Utilities.";

            return false;
        }

        if (utilities
            .GroupBy(c => c.Id)
            .Any(g => g.Count() > 1))
        {
            errorMessage =
                "Utilities must be unique.";

            return false;
        }

        if (deck.Structure == DeckStructure.Queue)
        {
            bool characterInFirstFour =
                deck.Cards
                    .Take(4)
                    .Any(c =>
                        c.Type == CardType.Character);

            if (!characterInFirstFour)
            {
                errorMessage =
                    "Queue decks must contain a Character within the first 4 cards.";

                return false;
            }
        }

        if (deck.Structure == DeckStructure.Stack)
        {
            bool characterInTopFour =
                deck.Cards
                    .Skip(
                        Math.Max(
                            0,
                            deck.Cards.Count - 4))
                    .Any(c =>
                        c.Type == CardType.Character);

            if (!characterInTopFour)
            {
                errorMessage =
                    "Stack decks must contain a Character within the top 4 cards.";

                return false;
            }
        }

        if (deck.Structure == DeckStructure.PriorityQueue)
        {
            bool characterAtPriorityFive =
                deck.Cards.Any(c =>
                    c.Type == CardType.Character &&
                    c.Priority == 5);

            bool nonCharacterAtPriorityFive =
                deck.Cards.Any(c =>
                    c.Type != CardType.Character &&
                    c.Priority == 5);

            if (!characterAtPriorityFive)
            {
                errorMessage =
                    "Priority Queue decks must have at least one Character at Priority 5.";

                return false;
            }

            if (nonCharacterAtPriorityFive)
            {
                errorMessage =
                    "Priority 5 is reserved for Character cards.";

                return false;
            }
        }

        errorMessage = string.Empty;

        return true;
    }
    public static bool AddCard(
    Deck deck,
    Card card,
    int priority = 0)
{
    if (deck.Cards.Count >= 15)
    {
        return false;
    }

    Card clone =
        card.Clone();

    if (deck.Structure ==
        DeckStructure.PriorityQueue)
    {
        if (priority < 1 ||
            priority > 5)
        {
            return false;
        }

        if (clone.Type == CardType.Character)
        {
            if (priority != 5)
            {
                return false;
            }
        }
        else
        {
            if (priority == 5)
            {
                return false;
            }
        }

        clone.Priority =
            priority;
    }

    deck.Cards.Add(
        clone);

    return true;
}
public static bool RemoveCard(
    Deck deck,
    int index)
{
    if (index < 0 ||
        index >= deck.Cards.Count)
    {
        return false;
    }

    deck.Cards.RemoveAt(index);

    return true;
}
public static bool MoveCard(
    Deck deck,
    int fromIndex,
    int toIndex)
{
    if (fromIndex < 0 ||
        fromIndex >= deck.Cards.Count)
    {
        return false;
    }

    if (toIndex < 0 ||
        toIndex >= deck.Cards.Count)
    {
        return false;
    }

    if (fromIndex == toIndex)
    {
        return true;
    }

    Card card =
        deck.Cards[fromIndex];

    deck.Cards.RemoveAt(fromIndex);

    deck.Cards.Insert(
        toIndex,
        card);

    return true;
}

public static bool MoveCardUp(
    Deck deck,
    int index)
{
    if (index <= 0 ||
        index >= deck.Cards.Count)
    {
        return false;
    }

    return MoveCard(
        deck,
        index,
        index - 1);
}

public static bool MoveCardDown(
    Deck deck,
    int index)
{
    if (index < 0 ||
        index >= deck.Cards.Count - 1)
    {
        return false;
    }

    return MoveCard(
        deck,
        index,
        index + 1);
}
}