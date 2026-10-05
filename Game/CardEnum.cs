namespace RedPandaTCD_Web.Game;

public enum CardType
{
    Character,
    Attack,
    Utility
}

public enum Archetype
{
    Magic,
    Physical,
    Neutral
}

public enum GameMode
{
    BotMatch,
    PassAndPlay,
    Tutorial
}

public enum DeckStructure
{
    Queue,
    Stack,
    PriorityQueue,
    RandomList,
    LinkedList
}
public enum MatchPhase
{
    Utility,
    Placement,
    Attack,
    End
}