namespace RedPandaTCD_Web.Game;

public interface IDeckStorage
{
    int Count { get; }
    Card? Draw();
    void Add(Card card);
    List<Card> GetCards();
}


// ==========================
// QUEUE
// ==========================

public class QueueDeckStorage : IDeckStorage
{
    private readonly Queue<Card> cards =
        new Queue<Card>();

    public int Count => cards.Count;

    public Card? Draw()
    {
        if(cards.Count == 0)
            return null;

        return cards.Dequeue();
    }

    public void Add(Card card)
    {
        if(card == null)
            return;

        cards.Enqueue(card);
    }

    public List<Card> GetCards()
    {
        return cards.ToList();
    }
}


// ==========================
// STACK
// ==========================

public class StackDeckStorage : IDeckStorage
{
    private readonly Stack<Card> cards =
        new Stack<Card>();

    public int Count => cards.Count;

    public Card? Draw()
    {
        if(cards.Count == 0)
            return null;

        return cards.Pop();
    }

    public void Add(Card card)
    {
        if(card == null)
            return;

        cards.Push(card);
    }

    public List<Card> GetCards()
    {
        return cards.ToList();
    }
}


// ==========================
// RANDOM LIST
// ==========================

public class RandomListDeckStorage : IDeckStorage
{
    private readonly List<Card> cards =
        new List<Card>();

    public int Count => cards.Count;

    public Card? Draw()
    {
        if(cards.Count == 0)
            return null;

        int randomIndex =
            GameRandom.Instance.Next(cards.Count);

        Card card =
            cards[randomIndex];

        cards.RemoveAt(randomIndex);

        return card;
    }

    public void Add(Card card)
    {
        if(card == null)
            return;

        cards.Add(card);
    }

    public List<Card> GetCards()
    {
        return cards.ToList();
    }

    public Card? DrawRandomCharacter()
    {
        List<int> characterIndexes =
            new List<int>();

        for(int i = 0; i < cards.Count; i++)
        {
            if(cards[i].Type == CardType.Character)
            {
                characterIndexes.Add(i);
            }
        }

        if(characterIndexes.Count == 0)
            return null;

        int randomCharacterIndex =
            characterIndexes[
                GameRandom.Instance.Next(
                    characterIndexes.Count)];

        Card character =
            cards[randomCharacterIndex];

        cards.RemoveAt(randomCharacterIndex);

        return character;
    }
}


// ==========================
// LINKED LIST
// ==========================

public class LinkedListDeckStorage : IDeckStorage
{
    private readonly LinkedList<Card> cards =
        new LinkedList<Card>();

    public int Count => cards.Count;

    public Card? Draw()
    {
        if(cards.First == null)
            return null;

        Card card = cards.First.Value;

        cards.RemoveFirst();

        return card;
    }

    public void Add(Card card)
    {
        if(card == null)
            return;

        cards.AddLast(card);
    }

    public List<Card> GetCards()
    {
        return cards.ToList();
    }

    public bool MoveNode(
        int fromIndex,
        int toIndex)
    {
        if(fromIndex < 0 ||
           fromIndex >= cards.Count)
        {
            return false;
        }

        if(toIndex < 0 ||
           toIndex >= cards.Count)
        {
            return false;
        }

        if(fromIndex == toIndex)
            return true;

        LinkedListNode<Card>? nodeToMove =
            GetNodeAt(fromIndex);

        if(nodeToMove == null)
            return false;

        cards.Remove(nodeToMove);

        if(toIndex == 0)
        {
            cards.AddFirst(nodeToMove);
        }
        else
        {
            LinkedListNode<Card>? targetNode =
                GetNodeAt(toIndex - 1);

            if(targetNode == null)
                return false;

            cards.AddAfter(
                targetNode,
                nodeToMove);
        }

        return true;
    }

    private LinkedListNode<Card>? GetNodeAt(
        int index)
    {
        LinkedListNode<Card>? current =
            cards.First;

        for(int i = 0;
            i < index && current != null;
            i++)
        {
            current = current.Next;
        }

        return current;
    }
}


// ==========================
// PRIORITY QUEUE
// ==========================

public class PriorityQueueDeckStorage : IDeckStorage
{
    private readonly PriorityQueue<
        Card,
        (int priority, int tieBreaker)> cards =
        new PriorityQueue<
            Card,
            (int priority, int tieBreaker)>();

    public int Count => cards.Count;

    public Card? Draw()
    {
        if(cards.Count == 0)
            return null;

        return cards.Dequeue();
    }

    public void Add(Card card)
    {
        if(card == null)
            return;

        int gamePriority =
            Math.Clamp(
                card.Priority,
                1,
                5);

        int tieBreaker =
            GameRandom.Instance.Next();

        cards.Enqueue(
            card,
            (-gamePriority, tieBreaker));
    }

    public List<Card> GetCards()
    {
        return cards.UnorderedItems
            .Select(item => item.Element)
            .ToList();
    }
}