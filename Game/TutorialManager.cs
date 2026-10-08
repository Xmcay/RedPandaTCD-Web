namespace RedPandaTCD_Web.Game;
public enum TutorialAction
{
    None,
    Acknowledge,
    SelectUtility,
    PlayUtility,
    DiscardAndDraw,
    PeekDeck,
    ViewDrawPool,
    StartReorderNodes,
    MoveReorderNode,
    FinishReorderNodes,
    DeployCharacter,
    DeployAttack,
    ReplaceAttack,
    RestAttack,
    ReplaceCharacter,
    ResolveWandererEntry,
    ObserveAutomaticPlacement,
    UseAttack,
    UseCharacterAbility,
    ContinuePhase,
    PrepareRecycle,
    ObserveRecycle,
    Surrender,
    ObserveBoard,
    ReturnToTutorial,
    FinishTutorial
}
public sealed class TutorialStep
{
    public int Number { get; init; }
    public string Title { get; init; } = "";
    public string Message { get; init; } = "";
    public TutorialAction RequiredAction { get; init; }
    public string HighlightTarget { get; init; } = "";
    public bool RestrictInput { get; init; }
    public bool IsInformational =>
        RequiredAction ==
        TutorialAction.Acknowledge;
}
public static class TutorialManager
{
    /*
        LEARN TO PLAY DECK
        These are the 12 cards the player must use
        for the Learn to Play Tutorial deck.
    */
    public static readonly IReadOnlyList<string>
        LearnToPlayDeckCardIds =
        new List<string>
        {
            // Characters
            "CM2",
            "CN1",
            "AM3",
            "AM6",
            "AP4",
            "AN1",
            "AN2",
"UM1",
"UN1",
"UN4",
"UM2",
"UN3"
        };
public static readonly IReadOnlyList<string>
    LearnToPlayDeckOrder =
    new List<string>
    {
        "UM1",
        "CM2",
        "AM6",
        "AP4",
        "AM3",
        "AN1",
        "CN1",
        "UN4",
        "UN1",
        "AN2",
        "UM2",
        "UN3"
    };

private static readonly IReadOnlyList<TutorialStep>
    LearnToPlaySteps =
    new List<TutorialStep>
    {
        new()
        {
            Number = 0,
            Title = "Welcome to Battle",
            Message =
                "This is the battlefield. Reduce your opponent's Energy " +
                "to 0 to win. You'll learn by playing a real match.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "battle-board",
            RestrictInput =
                true
        },
        new()
        {
            Number = 1,
            Title = "Opponent Field",
            Message =
                "Your opponent's Character and deployed Attacks appear here.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "enemy-field",
            RestrictInput =
                true
        },
        new()
        {
            Number = 2,
            Title = "Your Field",
            Message =
                "Your Character and deployed Attacks appear here.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-field",
            RestrictInput =
                true
        },
        new()
        {
            Number = 3,
            Title = "Energy",
            Message =
                "Cards and abilities can cost Energy. " +
                "Reaching 0 can end the battle.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-energy",
            RestrictInput =
                true
        },
        new()
        {
            Number = 4,
            Title = "Your Hand",
            Message =
                "Cards you draw appear here. Your hand holds cards " +
                "you can play, deploy, or discard.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-hand",
            RestrictInput =
                true
        },
        new()
        {
            Number = 5,
            Title = "Your Deck",
            Message =
                "Your deck's data structure affects how cards are stored " +
                "and drawn. The Data Structure Tutorials explain each one.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 6,
            Title = "Initiative",
            Message =
                "You have Initiative on Turn 1. Initiative acts first " +
                "in each phase and swaps each turn.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "initiative",
            RestrictInput =
                true
        },
        new()
        {
            Number = 7,
            Title = "The Battle Turn",
            Message =
                "Each turn goes Utility → Placement → Attack → End. " +
                "Both players act in each phase, with Initiative first.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "phase-bar",
            RestrictInput =
                true
        },

        // LESSON 2 — YOUR FIRST TURN

        new()
        {
            Number = 8,
            Title = "Utility Cards",
            Message =
                "Utility cards provide special effects. " +
                "Select Energy Potion to inspect it.",
            RequiredAction =
                TutorialAction.SelectUtility,
            HighlightTarget =
                "tutorial-utility",
            RestrictInput =
                true
        },
        new()
        {
            Number = 9,
            Title = "Utility Effect",
            Message =
                "Energy Potion restores Energy. Read its effect, then play it.",
            RequiredAction =
                TutorialAction.PlayUtility,
            HighlightTarget =
                "utility-action",
            RestrictInput =
                true
        },
        new()
        {
            Number = 10,
            Title = "Battle Log",
            Message =
                "The Battle Log records what happens during battle. " +
                "Press Continue when you're ready.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "battle-log",
            RestrictInput =
                true
        },
        new()
        {
            Number = 11,
            Title = "Placement Phase",
            Message =
                "Placement prepares your field. " +
                "Deploy Characters and Attacks here.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "phase-placement",
            RestrictInput =
                true
        },
        new()
        {
            Number = 12,
            Title = "Deploy Sorcerer",
            Message =
                "Deploy Sorcerer Red Panda to your Character slot.",
            RequiredAction =
                TutorialAction.DeployCharacter,
            HighlightTarget =
                "sorcerer",
            RestrictInput =
                true
        },
        new()
        {
            Number = 13,
            Title = "Character Cards",
            Message =
                "Characters have Entry, Passive, and Active abilities. " +
                "Each works differently during battle.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 14,
            Title = "Magic Surge",
            Message =
                "Sorcerer's Entry is active this turn. " +
                "Your Magic Attacks gain +1 Damage.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 15,
            Title = "Deploy Fireball",
            Message =
                "Deploy Fireball. It's Magic, so it receives Sorcerer's bonuses.",
            RequiredAction =
                TutorialAction.DeployAttack,
            HighlightTarget =
                "fireball",
            RestrictInput =
                true
        },
        new()
        {
            Number = 16,
            Title = "Stacked Bonuses",
            Message =
                "Sorcerer now gives Magic Attacks +2: " +
                "+1 from Magic Surge and +1 from Arcane Focus.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 17,
            Title = "Conditional Passives",
            Message =
                "Deploy Piercing Strike. It's Physical, so Arcane Focus " +
                "turns off. Magic Surge's +1 remains.",
            RequiredAction =
                TutorialAction.DeployAttack,
            HighlightTarget =
                "piercing-attack",
            RestrictInput =
                true
        },
        new()
        {
            Number = 18,
            Title = "Field Ready",
            Message =
                "Your field is ready. Continue to finish your Placement action.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 19,
            Title = "Attack Phase",
            Message =
                "Your deployed Attacks can now be used. " +
                "Start with Piercing Strike.",
            RequiredAction =
                TutorialAction.UseAttack,
            HighlightTarget =
                "piercing-attack",
            RestrictInput =
                true
        },
        new()
        {
            Number = 20,
            Title = "Piercing",
            Message =
                "Piercing damaged the Character through Shield. " +
                "Compare the result with the Battle Log.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "enemy-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 21,
            Title = "Fireball",
            Message =
                "Now use Fireball. Normal hits damage Shield before HP.",
            RequiredAction =
                TutorialAction.UseAttack,
            HighlightTarget =
                "fireball",
            RestrictInput =
                true
        },
        new()
        {
            Number = 22,
            Title = "Before End",
            Message =
                "Your attacks are done. Continue to finish your Attack action.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 23,
            Title = "Defense",
            Message =
                "Your opponent attacked. Compare your Character's Shield " +
                "and HP with the Battle Log.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 24,
            Title = "Effect Duration",
            Message =
                "Magic Surge expired. Piercing Strike still disables " +
                "Arcane Focus, so Sorcerer gives no bonus right now.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },

        // LESSON 3 — COMBAT & FIELD MANAGEMENT

        new()
        {
            Number = 25,
            Title = "Discard & Draw",
            Message =
                "Utility actions aren't limited to Utility cards. " +
                "Discard the highlighted card and draw a replacement.",
            RequiredAction =
                TutorialAction.DiscardAndDraw,
            HighlightTarget =
                "discard-card",
            RestrictInput =
                true
        },
        new()
        {
            Number = 26,
            Title = "Card Movement",
            Message =
                "The discarded card moved to your Discard Pile, " +
                "and another card became available.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "discard-pile",
            RestrictInput =
                true
        },
        new()
        {
            Number = 27,
            Title = "Finish Utility",
            Message =
                "Your Utility action is done. Continue to finish Utility.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 28,
            Title = "Replacing Attacks",
            Message =
                "Replace Piercing Strike with Ice Shards, " +
                "your Multi-Hit Attack.",
            RequiredAction =
                TutorialAction.ReplaceAttack,
            HighlightTarget =
                "piercing-slot",
            RestrictInput =
                true
        },
        new()
        {
            Number = 29,
            Title = "Arcane Focus Returns",
            Message =
                "Both Attacks are Magic again. " +
                "Arcane Focus gives them +1 Damage.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 30,
            Title = "Finish Placement",
            Message =
                "Your Placement changes are done. Continue to finish Placement.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 31,
            Title = "Active Abilities",
            Message =
                "Active abilities are activated manually and may cost Energy. " +
                "Use Sorcerer's Active now.",
            RequiredAction =
                TutorialAction.UseCharacterAbility,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 32,
            Title = "Fireball Again",
            Message =
                "Use Fireball. It receives +1 from Arcane Focus " +
                "and +1 from Sorcerer's Active.",
            RequiredAction =
                TutorialAction.UseAttack,
            HighlightTarget =
                "fireball",
            RestrictInput =
                true
        },
        new()
        {
            Number = 33,
            Title = "Fireball Fatigue",
            Message =
                "Fireball reached its use limit and is Fatigued. " +
                "You'll resolve it during Placement.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "fireball",
            RestrictInput =
                true
        },
        new()
        {
            Number = 34,
            Title = "Multi-Hit",
            Message =
                "Use Ice Shards. Multi-Hit Attacks strike more than once.",
            RequiredAction =
                TutorialAction.UseAttack,
            HighlightTarget =
                "multi-hit-attack",
            RestrictInput =
                true
        },
        new()
        {
            Number = 35,
            Title = "Multi-Hit Resolution",
            Message =
                "Each hit resolves separately. " +
                "Compare the hits with the opponent's Shield and HP.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "battle-log",
            RestrictInput =
                true
        },
        new()
        {
            Number = 36,
            Title = "Finish Attack",
            Message =
                "Your attacks are done. Continue to finish your Attack action.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 37,
            Title = "End Phase Recovery",
            Message =
                "Shield and other effects can change during End. " +
                "Check the battlefield and Battle Log.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 38,
            Title = "Active Cooldown",
            Message =
                "Sorcerer's Active is now on cooldown. " +
                "Some abilities can't be used every time you act.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },

        // LESSON 4 — DECKS & ADVANCED PLACEMENT

        new()
        {
            Number = 39,
            Title = "Peek",
            Message =
                "For your Utility action, click your deck and use Peek.",
            RequiredAction =
                TutorialAction.PeekDeck,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 40,
            Title = "Queue Structure",
            Message =
                "Your deck is a Queue: cards leave in First-In, First-Out order.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 41,
            Title = "Finish Utility",
            Message =
                "Peek is complete. Continue to finish Utility.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 42,
            Title = "Resolving Fatigue",
            Message =
                "Fireball is still Fatigued. Rest it before leaving Placement.",
            RequiredAction =
                TutorialAction.RestAttack,
            HighlightTarget =
                "fireball",
            RestrictInput =
                true
        },
        new()
        {
            Number = 43,
            Title = "Replacing Characters",
            Message =
                "Characters can be replaced during Placement. " +
                "Select your Character slot.",
            RequiredAction =
                TutorialAction.ReplaceCharacter,
            HighlightTarget =
                "player-character",
            RestrictInput =
                true
        },
        new()
        {
            Number = 44,
            Title = "Deploy Wanderer",
            Message =
                "Deploy Wanderer Red Panda.",
            RequiredAction =
                TutorialAction.DeployCharacter,
            HighlightTarget =
                "wanderer",
            RestrictInput =
                true
        },
        new()
        {
            Number = 45,
            Title = "Wanderer's Entry",
            Message =
                "Wanderer's Entry reduces one deployed Attack's buildup " +
                "cost by 2. Select an Attack.",
            RequiredAction =
                TutorialAction.ResolveWandererEntry,
            HighlightTarget =
                "wanderer-entry",
            RestrictInput =
                true
        },
        new()
        {
            Number = 46,
            Title = "Inspect Wanderer",
            Message =
                "Inspect Wanderer on the field. " +
                "Each Character has its own Entry, Passive, and Active abilities.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "wanderer",
            RestrictInput =
                true
        },
        new()
        {
            Number = 47,
            Title = "Complete Placement",
            Message =
                "Wanderer's Entry is resolved. Continue to finish Placement.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 48,
            Title = "Ice Shards",
            Message =
                "Use Ice Shards during this Attack action.",
            RequiredAction =
                TutorialAction.UseAttack,
            HighlightTarget =
                "multi-hit-attack",
            RestrictInput =
                true
        },
        new()
        {
            Number = 49,
            Title = "Battle Training Complete",
            Message =
                "You've learned the core battle flow, deployment, abilities, " +
                "attacks, Fatigue, Utility actions, and the Battle Log.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "battle-board",
            RestrictInput =
                true
        },

        // LESSON 5 — YOU'RE READY

        new()
        {
            Number = 50,
            Title = "Ending a Match",
            Message =
                "Battles can end in Victory, Defeat, or a Draw.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "battle-board",
            RestrictInput =
                true
        },
        new()
        {
            Number = 51,
            Title = "Surrender",
            Message =
                "You can also end a match by surrendering. " +
                "Surrender now to continue the tutorial.",
            RequiredAction =
                TutorialAction.Surrender,
            HighlightTarget =
                "surrender-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 52,
            Title = "Observe Board",
            Message =
                "Observe Board lets you inspect the final battlefield " +
                "and Battle Log. Choose it now.",
            RequiredAction =
                TutorialAction.ObserveBoard,
            HighlightTarget =
                "observe-board-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 53,
            Title = "Learn To Battle Tutorial Complete",
            Message =
                "You've completed the Battle Tutorial. " +
                "Return to the Tutorial menu when you're ready.",
            RequiredAction =
                TutorialAction.ReturnToTutorial,
            HighlightTarget =
                "return-to-tutorial",
            RestrictInput =
                true
        }
    };

private static readonly IReadOnlyList<TutorialStep>
    QueueSteps =
    new List<TutorialStep>
    {
        new()
        {
            Number = 0,
            Title = "Queue Deck",
            Message =
                "A Queue follows First-In, First-Out order. " +
                "The card waiting at the front is retrieved first.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 1,
            Title = "Queue Peek",
            Message =
                "Click the Queue deck and use Peek. Peek inspects " +
                "up to the next two cards without drawing them.",
            RequiredAction =
                TutorialAction.PeekDeck,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 2,
            Title = "FIFO Order",
            Message =
                "The first card shown is nearest the front of the Queue. " +
                "Cards behind it wait until earlier cards leave.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 3,
            Title = "Advance the Turn",
            Message =
                "Continue through Utility and enter Placement.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 4,
            Title = "Queue Placement",
            Message =
                "The cards in your hand were retrieved from the Queue " +
                "in FIFO order. Deploy up to two Attack cards now. " +
                "The open hand slots will make the End Phase draw visible. " +
                "Place at least one Attack card before continuing.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-hand",
            RestrictInput =
                true
        },
        new()
        {
            Number = 5,
            Title = "Complete Placement",
            Message =
                "Continue into the Attack Phase.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 6,
            Title = "Complete Attack",
            Message =
                "Continue into the End Phase.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 7,
            Title = "Prepare Recycling",
            Message =
                "At the end of the cycle, discarded cards can return " +
                "to the deck when more cards are needed.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "discard-pile",
            RestrictInput =
                true
        },
        new()
        {
            Number = 8,
            Title = "Prepare Queue Recycling",
            Message =
                "The recycling demonstration is being prepared.",
            RequiredAction =
                TutorialAction.PrepareRecycle,
            HighlightTarget =
                "discard-pile",
            RestrictInput =
                true
        },
        new()
        {
            Number = 9,
            Title = "Run the Recycling Demonstration",
            Message =
                "Press Continue to run the tutorial's Queue recycling demonstration.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 10,
            Title = "Queue Recycling",
            Message =
                "Recycled cards join the rear of the Queue. Existing cards " +
                "at the front still leave before the newly recycled cards.",
            RequiredAction =
                TutorialAction.ObserveRecycle,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 11,
            Title = "Queue Tutorial Complete",
            Message =
                "You have observed FIFO retrieval, Peek, and Queue recycling.",
            RequiredAction =
                TutorialAction.FinishTutorial,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        }
    };
private static readonly IReadOnlyList<TutorialStep>
    PriorityQueueSteps =
    new List<TutorialStep>
    {
        new()
        {
            Number = 0,
            Title = "Priority Queue Deck",
            Message =
                "A Priority Queue retrieves cards according to priority " +
                "instead of insertion order. In this game, higher numeric " +
                "priority is retrieved before lower priority.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 1,
            Title = "Automatic Retrieval",
            Message =
                "Priority Queue retrieval is automatic. Unlike Queue, Stack, " +
                "Random List, and Linked List, clicking this deck provides " +
                "no normal Utility-phase deck action.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 2,
            Title = "Continue to Placement",
            Message =
                "Continue and observe the cards supplied by priority order.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 3,
            Title = "Priority Placement",
            Message =
                "The hand reflects Priority Queue retrieval. Deploy up to " +
                "two Attack cards now so the End Phase can visibly retrieve " +
                "cards by priority. Place at least one Attack card before " +
                "continuing.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-hand",
            RestrictInput =
                true
        },
        new()
        {
            Number = 4,
            Title = "Complete Placement",
            Message =
                "Continue into the Attack Phase.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 5,
            Title = "Complete Attack",
            Message =
                "Continue into the End Phase.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 6,
            Title = "Prepare Recycling",
            Message =
                "When discarded cards return, each card is inserted again " +
                "using its priority.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "discard-pile",
            RestrictInput =
                true
        },
        new()
        {
            Number = 7,
            Title = "Prepare Priority Recycling",
            Message =
                "The recycling demonstration is being prepared.",
            RequiredAction =
                TutorialAction.PrepareRecycle,
            HighlightTarget =
                "discard-pile",
            RestrictInput =
                true
        },
        new()
        {
            Number = 8,
            Title = "Run the Recycling Demonstration",
            Message =
                "Press Continue to run the tutorial's Priority Queue recycling demonstration.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 9,
            Title = "Priority Queue Recycling",
            Message =
                "Recycled cards do not simply join the front or rear. " +
                "They return according to priority, so a high-priority " +
                "recycled card can be retrieved before older cards.",
            RequiredAction =
                TutorialAction.ObserveRecycle,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 10,
            Title = "Priority Queue Tutorial Complete",
            Message =
                "You have observed priority retrieval and " +
                "priority-based recycling.",
            RequiredAction =
                TutorialAction.FinishTutorial,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        }
    };
private static readonly IReadOnlyList<TutorialStep>
    StackSteps =
    new List<TutorialStep>
    {
        new()
        {
            Number = 0,
            Title = "Stack Deck",
            Message =
                "A Stack follows Last-In, First-Out order. " +
                "The card most recently placed on top leaves first.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 1,
            Title = "Stack Peek",
            Message =
                "Click the Stack and use Peek to inspect up to " +
                "the top two cards.",
            RequiredAction =
                TutorialAction.PeekDeck,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 2,
            Title = "LIFO Order",
            Message =
                "The top card is retrieved first. Cards underneath it " +
                "remain blocked until the cards above leave.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 3,
            Title = "Enter Placement",
            Message =
                "Continue into Placement. Stack decks automatically " +
                "deploy eligible Characters and Attacks.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 4,
            Title = "Automatic Stack Placement",
            Message =
                "Observe the Stack deck's automatic Placement.",
            RequiredAction =
                TutorialAction.ObserveAutomaticPlacement,
            HighlightTarget =
                "player-field",
            RestrictInput =
                true
        },
        new()
        {
            Number = 5,
            Title = "Automatic Placement Complete",
            Message =
                "Eligible cards were deployed automatically. This reduces " +
                "manual control but follows the Stack's direct play style. " +
                "At least one Attack must be deployed before continuing.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-field",
            RestrictInput =
                true
        },
        new()
        {
            Number = 6,
            Title = "Complete Placement",
            Message =
                "Continue into the Attack Phase.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 7,
            Title = "Complete Attack",
            Message =
                "Continue into the End Phase.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 8,
            Title = "Prepare Recycling",
            Message =
                "Recycled cards are pushed back onto the Stack.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "discard-pile",
            RestrictInput =
                true
        },
        new()
        {
            Number = 9,
            Title = "Prepare Stack Recycling",
            Message =
                "The recycling demonstration is being prepared.",
            RequiredAction =
                TutorialAction.PrepareRecycle,
            HighlightTarget =
                "discard-pile",
            RestrictInput =
                true
        },
        new()
        {
            Number = 10,
            Title = "Run the Recycling Demonstration",
            Message =
                "Press Continue to run the tutorial's Stack recycling demonstration.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 11,
            Title = "Stack Recycling",
            Message =
                "Each recycled card is pushed onto the top. Because Stack " +
                "uses LIFO, the last recycled card becomes the first available.",
            RequiredAction =
                TutorialAction.ObserveRecycle,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 12,
            Title = "Stack Tutorial Complete",
            Message =
                "You have observed LIFO retrieval, Peek, automatic Placement, " +
                "and Stack recycling.",
            RequiredAction =
                TutorialAction.FinishTutorial,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        }
    };
private static readonly IReadOnlyList<TutorialStep>
    RandomListSteps =
    new List<TutorialStep>
    {
        new()
        {
            Number = 0,
            Title = "Random List Deck",
            Message =
                "A Random List does not guarantee a fixed retrieval order. " +
                "Each draw selects from the cards currently available " +
                "in the draw pool.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 1,
            Title = "View Draw Pool",
            Message =
                "Click the Random List deck and choose View Draw Pool. " +
                "The pool shows which cards may be selected by a future draw.",
            RequiredAction =
                TutorialAction.ViewDrawPool,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 2,
            Title = "Random Retrieval",
            Message =
                "Viewing the pool does not reveal the exact next card. " +
                "The next retrieval is selected randomly from the pool.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 3,
            Title = "Continue to Placement",
            Message =
                "Continue and observe the hand produced by Random List draws.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 4,
            Title = "Random List Placement",
            Message =
                "These cards came from the available draw pool. Deploy up to " +
                "two Attack cards now. The open hand slots will later show " +
                "random retrieval after recycling. Place at least one Attack " +
                "card before continuing.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-hand",
            RestrictInput =
                true
        },
        new()
        {
            Number = 5,
            Title = "Complete Placement",
            Message =
                "Continue into the Attack Phase.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 6,
            Title = "Complete Attack",
            Message =
                "Continue into the End Phase.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 7,
            Title = "Prepare Recycling",
            Message =
                "Recycled cards return to the Random List draw pool. " +
                "Once returned, each recycled card can be selected again.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "discard-pile",
            RestrictInput =
                true
        },
        new()
        {
            Number = 8,
            Title = "Prepare Random List Recycling",
            Message =
                "The recycling demonstration is being prepared.",
            RequiredAction =
                TutorialAction.PrepareRecycle,
            HighlightTarget =
                "discard-pile",
            RestrictInput =
                true
        },
        new()
        {
            Number = 9,
            Title = "Run the Recycling Demonstration",
            Message =
                "Press Continue to run the tutorial's Random List recycling demonstration.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 10,
            Title = "Random List Recycling",
            Message =
                "The recycled cards are available in the draw pool again. " +
                "Their previous discard order does not determine " +
                "which recycled card will be drawn first.",
            RequiredAction =
                TutorialAction.ObserveRecycle,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 11,
            Title = "Random List Tutorial Complete",
            Message =
                "You have observed the draw pool, random retrieval, " +
                "and Random List recycling.",
            RequiredAction =
                TutorialAction.FinishTutorial,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        }
    };
private static readonly IReadOnlyList<TutorialStep>
    LinkedListSteps =
    new List<TutorialStep>
    {
        new()
        {
            Number = 0,
            Title = "Linked List Deck",
            Message =
                "A Linked List stores cards as connected nodes. " +
                "The head node is retrieved first, and each node " +
                "points toward the next card in the sequence.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 1,
            Title = "Reorder Nodes",
            Message =
                "Click the Linked List deck and choose Reorder Nodes.",
            RequiredAction =
                TutorialAction.StartReorderNodes,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 2,
            Title = "Move a Node",
            Message =
                "Move one card node to a different position. " +
                "Changing node order changes the future retrieval order.",
            RequiredAction =
                TutorialAction.MoveReorderNode,
            HighlightTarget =
                "linked-list-nodes",
            RestrictInput =
                true
        },
        new()
        {
            Number = 3,
            Title = "Finish Reordering",
            Message =
                "Finish the node-reordering action to keep the new order.",
            RequiredAction =
                TutorialAction.FinishReorderNodes,
            HighlightTarget =
                "linked-list-nodes",
            RestrictInput =
                true
        },
        new()
        {
            Number = 4,
            Title = "Linked Retrieval Order",
            Message =
                "The first node is now the head. Retrieval begins at the head " +
                "and follows the links through the reordered sequence.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 5,
            Title = "Continue to Placement",
            Message =
                "Continue and observe the cards supplied by the linked order.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 6,
            Title = "Linked List Placement",
            Message =
                "The hand reflects the linked-node order. Deploy up to two " +
                "Attack cards now so the later End Phase draw can visibly " +
                "retrieve cards from the rebuilt Linked List. Place at least " +
                "one Attack card before continuing.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "player-hand",
            RestrictInput =
                true
        },
        new()
        {
            Number = 7,
            Title = "Complete Placement",
            Message =
                "Continue into the Attack Phase.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 8,
            Title = "Complete Attack",
            Message =
                "Continue into the End Phase.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 9,
            Title = "Prepare Recycling",
            Message =
                "Recycled cards are appended as new nodes at the tail " +
                "of the Linked List.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "discard-pile",
            RestrictInput =
                true
        },
        new()
        {
            Number = 10,
            Title = "Prepare Linked List Recycling",
            Message =
                "The recycling demonstration is being prepared.",
            RequiredAction =
                TutorialAction.PrepareRecycle,
            HighlightTarget =
                "discard-pile",
            RestrictInput =
                true
        },
        new()
        {
            Number = 11,
            Title = "Run the Recycling Demonstration",
            Message =
                "Press Continue to run the tutorial's Linked List recycling demonstration.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "continue-button",
            RestrictInput =
                true
        },
        new()
        {
            Number = 12,
            Title = "Linked List Recycling",
            Message =
                "Each recycled card becomes a node at the tail. " +
                "Existing nodes near the head remain earlier " +
                "in the retrieval path.",
            RequiredAction =
                TutorialAction.ObserveRecycle,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        },
        new()
        {
            Number = 13,
            Title = "Linked List Tutorial Complete",
            Message =
                "You have observed linked retrieval, node reordering, " +
                "and Linked List recycling.",
            RequiredAction =
                TutorialAction.FinishTutorial,
            HighlightTarget =
                "player-deck",
            RestrictInput =
                true
        }
    };
    public static void StartTutorial(
    TutorialState state,
    TutorialType tutorial)
{
    if (tutorial == TutorialType.None)
    {
        return;
    }
    state.Start(
        tutorial);
    ApplyCurrentStep(
        state);
}
    public static void StartLearnToPlay(
    TutorialState state)
{
    StartTutorial(
        state,
        TutorialType.LearnToPlay);
}
public static int GetStepCount(
    TutorialState state)
{
    if (!state.IsActive ||
        state.CurrentTutorial ==
            TutorialType.None)
    {
        return 0;
    }
    return GetSteps(
        state.CurrentTutorial).Count;
}
public static bool IsFinalStep(
    TutorialState state)
{
    TutorialStep? step =
        GetCurrentStep(
            state);
    if (step == null)
    {
        return false;
    }
    int count =
        GetStepCount(
            state);
    return count > 0 &&
           step.Number ==
               count - 1;
}
public static int GetStepCount(
    TutorialType tutorial)
{
    return GetSteps(
        tutorial).Count;
}
    public static TutorialStep? GetCurrentStep(
        TutorialState state)
    {
        if (!state.IsActive ||
            state.CurrentTutorial ==
                TutorialType.None)
        {
            return null;
        }
        IReadOnlyList<TutorialStep> steps =
            GetSteps(
                state.CurrentTutorial);
        if (state.CurrentStep < 0 ||
            state.CurrentStep >= steps.Count)
        {
            return null;
        }
        return steps[
            state.CurrentStep];
    }
    public static string GetInstructionTitle(
        TutorialState state)
    {
        return GetCurrentStep(state)?.Title ??
               "";
    }
    public static string GetInstructionMessage(
        TutorialState state)
    {
        return GetCurrentStep(state)?.Message ??
               "";
    }
    public static string GetHighlightTarget(
        TutorialState state)
    {
        return GetCurrentStep(state)?.HighlightTarget ??
               "";
    }
    public static TutorialAction GetRequiredAction(
        TutorialState state)
    {
        return GetCurrentStep(state)?.RequiredAction ??
               TutorialAction.None;
    }
    public static bool RequiresAction(
        TutorialState state,
        TutorialAction action)
    {
        TutorialStep? step =
            GetCurrentStep(
                state);
        if (step == null)
        {
            return false;
        }
        return step.RequiredAction ==
               action;
    }
    /*
        Called when the player performs an actual action
        in Battle.
        Returns true only when that action was the action
        required by the current Tutorial step.
    */
    public static bool NotifyAction(
        TutorialState state,
        TutorialAction action)
    {
        if (!state.IsActive ||
            state.IsCompleted)
        {
            return false;
        }
        TutorialStep? step =
            GetCurrentStep(
                state);
        if (step == null ||
            step.RequiredAction != action)
        {
            return false;
        }
        Advance(
            state);
        return true;
    }
    public static bool AcknowledgeInstruction(
        TutorialState state)
    {
        return NotifyAction(
            state,
            TutorialAction.Acknowledge);
    }
    public static bool CanPerformAction(
        TutorialState state,
        TutorialAction action)
    {
        /*
            Outside a Tutorial the game remains completely
            unrestricted.
        */
        if (!state.IsActive)
        {
            return true;
        }
        TutorialStep? step =
            GetCurrentStep(
                state);
        if (step == null)
        {
            return true;
        }
        if (!step.RestrictInput)
        {
            return true;
        }
        return step.RequiredAction ==
               action;
    }
    public static bool IsInputRestricted(
        TutorialState state)
    {
        TutorialStep? step =
            GetCurrentStep(
                state);
        return step?.RestrictInput ??
               false;
    }
    public static bool ShouldHighlight(
        TutorialState state,
        string target)
    {
        if (!state.IsActive ||
            string.IsNullOrWhiteSpace(target))
        {
            return false;
        }
        string currentTarget =
            GetHighlightTarget(
                state);
        return string.Equals(
            currentTarget,
            target,
            StringComparison.OrdinalIgnoreCase);
    }
    public static void CompleteTutorial(
        TutorialState state)
    {
        if (!state.IsActive)
        {
            return;
        }
        state.CompleteCurrentTutorial();
    }
    public static void ExitTutorial(
        TutorialState state)
    {
        state.EndTutorial();
    }
    private static void Advance(
        TutorialState state)
    {
        IReadOnlyList<TutorialStep> steps =
            GetSteps(
                state.CurrentTutorial);
        int nextStep =
            state.CurrentStep + 1;
        if (nextStep >= steps.Count)
        {
            CompleteTutorial(
                state);
            return;
        }
        state.AdvanceStep();
        ApplyCurrentStep(
            state);
    }
    private static void ApplyCurrentStep(
        TutorialState state)
    {
        TutorialStep? step =
            GetCurrentStep(
                state);
        if (step == null)
        {
            state.RequiredAction = "";
            state.InputRestricted = false;
            return;
        }
        state.RequiredAction =
            step.RequiredAction.ToString();
        state.InputRestricted =
            step.RestrictInput;
        state.InstructionAcknowledged =
            false;
    }
    public static bool IsLearnToPlayDeckCard(
        Card card)
    {
        return LearnToPlayDeckCardIds.Contains(
            card.Id);
    }
    public static bool IsLearnToPlayDeckComplete(
        Deck deck)
    {
        if (deck.Cards.Count !=
            LearnToPlayDeckCardIds.Count)
        {
            return false;
        }
        return LearnToPlayDeckCardIds.All(
            requiredId =>
                deck.Cards.Any(card =>
                    card.Id == requiredId));
    }
    public static string GetLearnToPlayDeckProgress(
        Deck deck)
    {
        int collected =
            LearnToPlayDeckCardIds.Count(
                requiredId =>
                    deck.Cards.Any(card =>
                        card.Id == requiredId));
        return
            $"{collected} / " +
            $"{LearnToPlayDeckCardIds.Count} required cards";
    }
private static IReadOnlyList<TutorialStep> GetSteps(
    TutorialType tutorial)
{
    return tutorial switch
    {
        TutorialType.LearnToPlay =>
            LearnToPlaySteps,
        TutorialType.Queue =>
            QueueSteps,
        TutorialType.PriorityQueue =>
            PriorityQueueSteps,
        TutorialType.Stack =>
            StackSteps,
        TutorialType.RandomList =>
            RandomListSteps,
        TutorialType.LinkedList =>
            LinkedListSteps,
        _ =>
            Array.Empty<TutorialStep>()
    };
}
    public static bool IsLearnToPlayDeckOrderCorrect(
    Deck deck)
{
    if (deck.Cards.Count !=
        LearnToPlayDeckOrder.Count)
    {
        return false;
    }
    for (int i = 0;
         i < LearnToPlayDeckOrder.Count;
         i++)
    {
        if (deck.Cards[i].Id !=
            LearnToPlayDeckOrder[i])
        {
            return false;
        }
    }
    return true;
}
public static string GetLearnToPlayDeckOrderHint(
    Deck deck)
{
    if (deck.Cards.Count !=
        LearnToPlayDeckOrder.Count)
    {
        return
            "Add all required cards before arranging the Queue.";
    }
    for (int i = 0;
         i < LearnToPlayDeckOrder.Count;
         i++)
    {
        string requiredId =
            LearnToPlayDeckOrder[i];
        Card? requiredCard =
            deck.Cards.FirstOrDefault(card =>
                card.Id == requiredId);
        if (requiredCard == null)
        {
            continue;
        }
        if (deck.Cards[i].Id != requiredId)
        {
            return
                $"Position {i + 1} should be " +
                $"{requiredCard.Name}.";
        }
    }
    return
        "Queue order is correct.";
}
}
