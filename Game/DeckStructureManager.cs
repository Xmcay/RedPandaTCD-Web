namespace RedPandaTCD_Web.Game;

public static class DeckStructureManager
{
    public static bool ShouldShuffle(Deck deck)
    {
        return deck.Structure != DeckStructure.Queue &&
               deck.Structure != DeckStructure.Stack;
    }

    public static int GetHandSize(Deck deck)
    {
        if (deck.Structure == DeckStructure.RandomList)
        {
            return 5;
        }

        return 4;
    }

    public static bool UsesStackLogic(Deck deck)
    {
        return deck.Structure == DeckStructure.Stack;
    }

    public static bool UsesPriorityLogic(Deck deck)
    {
        return deck.Structure == DeckStructure.PriorityQueue;
    }

    public static bool UsesLinkedListLogic(Deck deck)
    {
        return deck.Structure == DeckStructure.LinkedList;
    }

    public static bool CanRest(Deck deck)
    {
        return deck.Structure != DeckStructure.PriorityQueue;
    }

    public static bool RequiresQueueValidation(Deck deck)
    {
        return deck.Structure == DeckStructure.Queue;
    }

    public static bool CharacterInFirstFour(Deck deck)
    {
        return deck.Cards
            .Take(4)
            .Any(c => c.Type == CardType.Character);
    }

    public static void PrepareDeckForMatch(Player player)
    {
        switch (player.Deck.Structure)
        {
            case DeckStructure.Queue:
                // Keep build order.
                break;

            case DeckStructure.Stack:
                // Keep build order.
                break;

            case DeckStructure.PriorityQueue:
                break;

            case DeckStructure.RandomList:
                break;

            case DeckStructure.LinkedList:
                ShuffleDeck(player);
                EnsureCharacterInFirstFour(player);
                break;
        }
    }

    private static void EnsureCharacterInFirstFour(Player player)
    {
        List<Card> cards = player.Deck.Cards;

        bool characterFound = cards
            .Take(4)
            .Any(c => c.Type == CardType.Character);

        if (characterFound)
            return;

        int characterIndex = cards.FindIndex(
            c => c.Type == CardType.Character);

        if (characterIndex == -1)
            return;

        Card character = cards[characterIndex];

        cards.RemoveAt(characterIndex);
        cards.Insert(0, character);
    }

    public static void ShuffleDeck(Player player)
    {
        if (!ShouldShuffle(player.Deck))
            return;

        player.Deck.Cards = player.Deck.Cards
            .OrderBy(c => GameRandom.Instance.Next())
            .ToList();
    }

    public static Card? DrawFromDeck(Player player)
    {
        if (player?.Deck?.RuntimeStorage == null)
            return null;

        if (player.Deck.RuntimeStorage.Count == 0)
            return null;

        return player.Deck.RuntimeStorage.Draw();
    }

    public static bool MoveNode(
        Player player,
        int fromIndex,
        int toIndex)
    {
        if (player?.Deck?.RuntimeStorage
            is not LinkedListDeckStorage linkedStorage)
        {
            return false;
        }

        return linkedStorage.MoveNode(
            fromIndex,
            toIndex);
    }
}