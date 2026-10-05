namespace RedPandaTCD_Web.Game;

public enum TutorialType
{
    None,
    LearnToPlay,
    Queue,
    PriorityQueue,
    Stack,
    RandomList,
    LinkedList
}

public class TutorialState
{
    // True while any Tutorial is currently running.
    public bool IsActive { get; set; }

    // Which Tutorial is currently being played.
    public TutorialType CurrentTutorial { get; set; } =
        TutorialType.None;

    // Current scripted step inside the Tutorial.
    // TutorialManager will define what each step means.
    public int CurrentStep { get; set; }

    // True once the current Tutorial has reached its ending.
    public bool IsCompleted { get; set; }

    // Used by guided Battles so normal Battle behavior
    // can distinguish a Tutorial match from Training,
    // Tournament, or Pass-and-Play.
    public bool IsBattleTutorial { get; set; }

    // Allows TutorialManager to temporarily prevent
    // normal player actions until the required tutorial
    // instruction has been completed.
    public bool InputRestricted { get; set; }

    // Optional action identifier used by the Tutorial
    // controller when a step requires one specific action.
    public string RequiredAction { get; set; } = "";

    // Keeps track of whether the current instruction
    // has been acknowledged by the player.
    public bool InstructionAcknowledged { get; set; }

    // These are session-level completion records.
    // They allow the Tutorial menu to show progress.
    public bool LearnToPlayCompleted { get; set; }

    public bool QueueCompleted { get; set; }

    public bool PriorityQueueCompleted { get; set; }

    public bool StackCompleted { get; set; }

    public bool RandomListCompleted { get; set; }

    public bool LinkedListCompleted { get; set; }
    
    public bool DeckBuilderCompleted { get; set; }

public bool TournamentCompleted { get; set; }

public bool SimulatorCompleted { get; set; }

    // The deck created during Learn to Play.
// Battle receives a fresh clone of this deck.
public Deck? TutorialDeck { get; set; }


    public void Start(
        TutorialType tutorial)
    {
        TutorialDeck = null;

        IsActive = true;

        CurrentTutorial =
            tutorial;

        CurrentStep = 0;

        IsCompleted = false;

        IsBattleTutorial =
            tutorial != TutorialType.None;

        InputRestricted = true;

        RequiredAction = "";

        InstructionAcknowledged = false;
    }


    public void AdvanceStep()
    {
        if (!IsActive ||
            IsCompleted)
        {
            return;
        }

        CurrentStep++;

        RequiredAction = "";

        InstructionAcknowledged = false;
    }


    public void CompleteCurrentTutorial()
    {
        switch (CurrentTutorial)
        {
            case TutorialType.LearnToPlay:
                LearnToPlayCompleted = true;
                break;

            case TutorialType.Queue:
                QueueCompleted = true;
                break;

            case TutorialType.PriorityQueue:
                PriorityQueueCompleted = true;
                break;

            case TutorialType.Stack:
                StackCompleted = true;
                break;

            case TutorialType.RandomList:
                RandomListCompleted = true;
                break;

            case TutorialType.LinkedList:
                LinkedListCompleted = true;
                break;
        }

        IsCompleted = true;

        InputRestricted = false;

        RequiredAction = "";

        InstructionAcknowledged = false;
    }


    public void EndTutorial()
    {
        IsActive = false;

        CurrentTutorial =
            TutorialType.None;

        CurrentStep = 0;

        IsCompleted = false;

        IsBattleTutorial = false;

        InputRestricted = false;

        RequiredAction = "";

        InstructionAcknowledged = false;

        TutorialDeck = null;
    }


    public void ResetAllProgress()
    {
        EndTutorial();

        LearnToPlayCompleted = false;

        QueueCompleted = false;

        PriorityQueueCompleted = false;

        StackCompleted = false;

        RandomListCompleted = false;

        LinkedListCompleted = false;

        DeckBuilderCompleted =
    false;

TournamentCompleted =
    false;

SimulatorCompleted =
    false;
    }
}