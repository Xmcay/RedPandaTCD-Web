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
}