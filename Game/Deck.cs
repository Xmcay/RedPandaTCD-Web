namespace RedPandaTCD_Web.Game;

public class Deck
{
    public string Name { get; set; } = "";

    public DeckStructure Structure { get; set; }

    // Deck blueprint.
    // Used by Deck Builder, starter decks,
    // cloning and validation.
    public List<Card> Cards { get; set; } =
        new List<Card>();

    // Actual data structure used during a match.
    public IDeckStorage? RuntimeStorage
    {
        get;
        private set;
    }

    public void InitializeRuntimeStorage()
    {
        switch(Structure)
        {
            case DeckStructure.Queue:
                RuntimeStorage =
                    new QueueDeckStorage();
                break;

            case DeckStructure.Stack:
                RuntimeStorage =
                    new StackDeckStorage();
                break;

            case DeckStructure.PriorityQueue:
                RuntimeStorage =
                    new PriorityQueueDeckStorage();
                break;

            case DeckStructure.RandomList:
                RuntimeStorage =
                    new RandomListDeckStorage();
                break;

            case DeckStructure.LinkedList:
                RuntimeStorage =
                    new LinkedListDeckStorage();
                break;

            default:
                throw new InvalidOperationException(
                    "Unknown deck structure.");
        }

        foreach(Card card in Cards)
        {
            RuntimeStorage.Add(card);
        }
    }
    /// <summary>
    /// Installs the exact recorded deck order for a replay viewer. Replay state
    /// is observational only, so it uses a passive list-backed storage rather
    /// than invoking live deck draw/randomization behavior.
    /// </summary>
    public void SetReplayRuntimeCards(IEnumerable<Card> cards)
    {
        RuntimeStorage = new ReplaySnapshotDeckStorage(cards);
    }

    private sealed class ReplaySnapshotDeckStorage : IDeckStorage
    {
        private readonly List<Card> cards;

        public ReplaySnapshotDeckStorage(IEnumerable<Card> cards)
        {
            this.cards = cards.Select(card => card.Clone()).ToList();
        }

        public int Count => cards.Count;

        public Card? Draw()
        {
            if (cards.Count == 0)
                return null;

            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        public void Add(Card card)
        {
            if (card != null)
                cards.Add(card);
        }

        public List<Card> GetCards() => cards.Select(card => card.Clone()).ToList();
    }

}
