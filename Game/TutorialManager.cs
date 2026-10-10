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
            Title = "Welcome to the Battlefield",
            Message =
                "Welcome to the battlefield! Your goal is to reduce your opponent's Energy " +
                "to 0. You'll learn the rules by playing through a real match.",
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
            Title = "Your Opponent's Field",
            Message =
                "Your opponent's Character and deployed Attacks appear here. Watch this " +
                "area to see their current setup.",
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
            Title = "Your Battlefield",
            Message =
                "Your Character and deployed Attacks appear here. This is where you'll " +
                "build your own field.",
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
            Title = "Managing Energy",
            Message =
                "Cards and abilities can cost Energy. Keep an eye on your supply—if your " +
                "Energy reaches 0, you can lose the battle.",
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
            Title = "Your Hand of Cards",
            Message =
                "Cards you draw appear in your hand. From here, you can play Utility cards, " +
                "deploy Characters or Attacks, and discard cards when an action allows it.",
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
            Title = "Understanding Your Deck",
            Message =
                "Your deck's data structure determines how cards are stored and drawn. The " +
                "Data Structure Tutorials explain how each structure works.",
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
            Title = "Who Acts First?",
            Message =
                "Initiative determines who acts first in each phase. The player with " +
                "Initiative goes first, and Initiative swaps when a new turn begins.",
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
            Title = "The Four Battle Phases",
            Message =
                "Each turn follows four phases: Utility, Placement, Attack, then End. Both " +
                "players act during each phase, with the Initiative holder acting first.",
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
            Title = "Using Utility Cards",
            Message =
                "Utility cards provide special effects. Select the highlighted Energy " +
                "Potion to inspect it.",
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
            Title = "Play Energy Potion",
            Message =
                "Energy Potion restores Energy. Check its effect, then play it using the " +
                "Utility action.",
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
            Title = "Reading the Battle Log",
            Message =
                "The Battle Log records important events, including card effects and " +
                "changes during battle. Review it, then press Continue.",
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
            Title = "The Placement Phase",
            Message =
                "During Placement, prepare your field by deploying Characters and Attacks. " +
                "You'll set up your first cards now.",
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
            Title = "Deploy Sorcerer Red Panda",
            Message =
                "Deploy Sorcerer Red Panda into your Character slot.",
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
            Title = "Character Abilities",
            Message =
                "Characters can have Entry, Passive, and Active abilities. Entry effects " +
                "trigger when a Character is deployed, Passives apply under their " +
                "conditions, and Actives are used deliberately.",
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
            Title = "Entry Ability: Magic Surge",
            Message =
                "Sorcerer Red Panda's Entry ability, Magic Surge, gives your Magic Attacks " +
                "+1 Damage during the turn it enters play.",
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
                "Deploy Fireball. It's a Magic Attack, so it benefits from Sorcerer's Magic " +
                "damage bonuses.",
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
            Title = "Stacking Ability Bonuses",
            Message =
                "Fireball now benefits from two effects: Magic Surge adds +1 Damage this " +
                "turn, and Arcane Focus adds another +1 while no Physical Attack is " +
                "deployed.",
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
            Title = "Conditional Passive Abilities",
            Message =
                "Deploy Piercing Strike. It's Physical, so deploying it disables Arcane " +
                "Focus. Magic Surge's +1 Damage still applies this turn.",
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
            Title = "Your Field Is Ready",
            Message =
                "Your Character and Attacks are in place. Press Continue to finish your " +
                "Placement action.",
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
            Title = "Your First Attack",
            Message =
                "It's the Attack phase. Use Piercing Strike first and watch how its " +
                "piercing effect interacts with Shield.",
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
            Title = "Piercing Through Shield",
            Message =
                "Piercing Strike can damage the Character through its Shield. Check the " +
                "battlefield and Battle Log to see the result.",
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
            Title = "Attacking a Shield",
            Message =
                "Now use Fireball. Unlike a piercing Attack, a normal hit damages Shield " +
                "before it damages HP.",
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
            Title = "Attack Phase Complete",
            Message =
                "Your Attack action is complete. Press Continue to move on; the End phase " +
                "resolves automatically when the phase sequence reaches it.",
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
            Title = "Understanding Defense",
            Message =
                "Your opponent has attacked. Compare your Character's Shield and HP with " +
                "the Battle Log to see how the incoming damage was handled.",
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
            Title = "Temporary Effects Expire",
            Message =
                "Magic Surge lasts only for the turn Sorcerer enters play, so it has " +
                "expired. Piercing Strike is still deployed, which keeps Arcane Focus " +
                "inactive.",
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
            Title = "Discard to Draw",
            Message =
                "You can use a Utility action even without playing a Utility card. Discard " +
                "the highlighted card to draw a replacement.",
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
            Title = "Where Discarded Cards Go",
            Message =
                "The discarded card has moved to your Discard Pile, and a replacement has " +
                "become available in your hand.",
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
            Title = "Complete Your Utility Phase",
            Message =
                "You've finished your Utility action. Press Continue to move to the next " +
                "phase.",
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
            Title = "Replace an Attack",
            Message =
                "Replace Piercing Strike with Ice Shards, a Magic Attack that hits twice.",
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
            Title = "Passive Bonuses Revisited",
            Message =
                "Both deployed Attacks are Magic now, so Arcane Focus is active again and " +
                "gives them +1 Damage.",
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
            Title = "Complete Your Placement Phase",
            Message =
                "Your Placement changes are complete. Press Continue to enter the Attack " +
                "phase.",
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
            Title = "Using Active Abilities",
            Message =
                "Active abilities are activated manually and may cost Energy or have a " +
                "cooldown. Use Sorcerer Red Panda's Active ability now.",
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
            Title = "Combine Ability Bonuses",
            Message =
                "Use Fireball again. It gains +1 Damage from Arcane Focus and +1 from " +
                "Sorcerer's Active ability.",
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
            Title = "Understanding Fatigue",
            Message =
                "Fireball has reached its use limit and is now Fatigued. You'll need to " +
                "resolve its Fatigue during Placement before using it again.",
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
            Title = "Multi-Hit Attacks: Ice Shards",
            Message =
                "Use Ice Shards. It has two hits, so one use resolves as two separate hits.",
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
            Title = "Resolving Multiple Hits",
            Message =
                "Each hit resolves separately. Check the Battle Log to compare how the hits " +
                "affect your opponent's Shield and HP.",
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
            Title = "Finish the Attack Phase",
            Message =
                "Your attacks are finished for now. Press Continue to complete your Attack " +
                "action.",
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
            Title = "End-of-Turn Recovery",
            Message =
                "The End phase is where end-of-turn effects resolve. Shield and " +
                "other turn-based effects are updated, cards may be drawn, and " +
                "Energy is recovered. Check the battlefield and Battle Log " +
                "to see the changes.",
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
           Title = "Ability Cooldowns",
            Message =
                "End-of-turn processing can also affect ability availability. " +
                "The Sorcerer's Active ability has a cooldown, which determines " +
                "when it can be used again. Some abilities need time before " +
                "they become available again.",
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
            Title = "Peek at Your Deck",
            Message =
                "Use your Utility action to inspect your deck. Click your deck and choose " +
                "Peek.",
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
            Title = "First In, First Out (FIFO)",
            Message =
                "Your deck uses a Queue: cards are drawn in First-In, First-Out order. The " +
                "card that entered the Queue earliest is retrieved first.",
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
            Title = "Complete Your Utility Phase",
            Message =
                "You've finished Peek. Press Continue to leave the Utility phase.",
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
            Title = "Rest a Fatigued Attack",
            Message =
                "Fireball is still Fatigued. Rest it during Placement to restore its uses " +
                "before trying it again.",
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
            Title = "Replace Your Character",
            Message =
                "You can replace your Character during Placement. Select your current " +
                "Character slot to begin.",
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
            Title = "Deploy Wanderer Red Panda",
            Message =
                "Deploy Wanderer Red Panda into your Character slot.",
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
            Title = "Entry Ability: Quick Setup",
            Message =
                "Wanderer's Entry ability reduces the buildup cost of one deployed Attack " +
                "by 2. Select the Attack you want to adjust.",
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
            Title = "Inspect Your Character",
            Message =
                "Inspect Wanderer on the field to review its abilities. Each Character has " +
                "its own Entry, Passive, and Active effects.",
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
            Title = "Complete Your Placement Phase",
            Message =
                "Wanderer's Entry effect has been resolved. Press Continue to finish " +
                "Placement.",
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
            Title = "Ice Shards: A Second Look",
            Message =
                "Use Ice Shards again. Notice how a multi-hit Attack resolves and how its " +
                "damage interacts with the opponent's remaining Shield and HP.",
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
            Title = "Battle Basics Complete",
            Message =
                "You've practiced the core battle flow, deploying cards, using abilities, " +
                "attacking, managing Fatigue, taking Utility actions, and reading the " +
                "Battle Log.",
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
            Title = "Victory, Defeat, and Draws",
            Message =
                "A match can end in Victory, Defeat, or a Draw, depending on how the battle " +
                "concludes.",
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
            Title = "Surrendering a Match",
            Message =
                "You can also end a match by surrendering. Surrender now to continue this " +
                "tutorial.",
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
            Title = "Review the Final Battlefield",
            Message =
                "Choose Observe Board to inspect the final battlefield and review the " +
                "Battle Log.",
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
            Title = "Tutorial Complete: Ready for Battle",
            Message =
                "You've completed the Battle Tutorial. Return to the Tutorial menu whenever " +
                "you're ready.",
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
            Title = "How a Queue Works",
            Message =
                "A Queue follows First-In, First-Out (FIFO) order: the card at the front is " +
                "the next one retrieved.",
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
            Title = "Peek at the Queue",
            Message =
                "Select the Queue deck and use Peek. It lets you inspect up to the next two " +
                "cards without drawing them.",
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
            Title = "Understanding FIFO Order",
            Message =
                "The card shown nearest the front of the Queue will be retrieved first. " +
                "Cards behind it must wait until the earlier cards leave.",
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
            Title = "Advance to Placement",
            Message =
                "Press Continue through the remaining Utility action and enter Placement.",
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
            Title = "Deploy from Your Queue",
            Message =
                "Your hand was filled from the Queue in FIFO order. Deploy at least one " +
                "Attack (up to two) to your field, then continue. The open hand slots will " +
                "help show the cards drawn during End.",
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
            Title = "Complete Your Placement Phase",
            Message =
                "Your Placement is done. Press Continue to enter the Attack phase.",
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
            Title = "Complete Your Attack Phase",
            Message =
                "Your Attack action is done. Press Continue to proceed to End.",
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
            Title = "When Cards Are Recycled",
            Message =
                "When the deck runs out of cards, discarded cards can be recycled so they " +
                "can be drawn again.",
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
                "The tutorial is setting up a Queue recycling demonstration. Follow the " +
                "next instruction to see where recycled cards go.",
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
                "Press Continue to run the Queue recycling demonstration.",
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
            Title = "Recycled Cards Join the Rear",
            Message =
                "Recycled cards are added to the back of the Queue. Cards already near the " +
                "front are still retrieved before the newly recycled cards.",
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
                "You've seen FIFO retrieval, Peek, and how discarded cards return to the " +
                "Queue.",
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
            Title = "How a Priority Queue Works",
            Message =
                "A Priority Queue retrieves cards by priority instead of arrival order. In " +
                "this game, a higher numeric priority is retrieved before a lower one.",
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
            Title = "Automatic Priority Retrieval",
            Message =
                "Priority Queue draws happen automatically. Unlike the other deck " +
                "structures, clicking this deck does not provide a normal Utility-phase " +
                "deck action.",
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
            Title = "Advance to Placement",
            Message =
                "Press Continue and watch which cards the Priority Queue supplies to your " +
                "hand.",
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
            Title = "Deploy by Priority",
            Message =
                "Your hand reflects Priority Queue retrieval. Deploy at least one Attack " +
                "(up to two), then continue. The open hand slots will help reveal the next " +
                "cards retrieved by priority during End.",
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
            Title = "Complete Your Placement Phase",
            Message =
                "Your Placement is done. Press Continue to enter the Attack phase.",
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
            Title = "Complete Your Attack Phase",
            Message =
                "Your Attack action is done. Press Continue to proceed to End.",
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
            Title = "Recycling and Priority",
            Message =
                "When discarded cards are recycled, each card is inserted according to its " +
                "priority rather than simply being placed at the front or back.",
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
            Title = "Prepare Priority Queue Recycling",
            Message =
                "The tutorial is setting up a Priority Queue recycling demonstration. " +
                "Follow the next instruction to see how priorities affect insertion.",
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
                "Press Continue to run the Priority Queue recycling demonstration.",
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
            Title = "Recycled Cards Return by Priority",
            Message =
                "Recycled cards return to their priority-based positions. A high-priority " +
                "card can be retrieved before cards that were already waiting.",
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
                "You've seen priority-based retrieval and how recycled cards are placed " +
                "according to priority.",
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
            Title = "How a Stack Works",
            Message =
                "A Stack follows Last-In, First-Out (LIFO) order: the card most recently " +
                "placed on top is retrieved first.",
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
            Title = "Peek at the Stack",
            Message =
                "Select the Stack deck and use Peek to inspect up to the top two cards " +
                "without drawing them.",
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
            Title = "Understanding LIFO Order",
            Message =
                "The top card is retrieved first. Cards underneath it remain unavailable " +
                "until the cards above them have been removed.",
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
            Title = "Advance to Placement",
            Message =
                "Press Continue to enter Placement. With a Stack deck, eligible Characters " +
                "and Attacks are deployed automatically.",
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
            Title = "Automatic Stack Deployment",
            Message =
                "Watch as the Stack automatically deploys eligible cards during Placement.",
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
            Title = "Understanding Automatic Placement",
            Message =
                "The eligible cards have been deployed automatically. This reduces manual " +
                "control but follows the Stack's direct-play behavior. Make sure at least " +
                "one Attack is deployed before continuing.",
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
            Title = "Complete Your Placement Phase",
            Message =
                "Placement is complete. Press Continue to enter the Attack phase.",
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
            Title = "Complete Your Attack Phase",
            Message =
                "Your Attack action is done. Press Continue to proceed to End.",
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
            Title = "When Cards Are Recycled",
            Message =
                "Discarded cards can return to the Stack. Each recycled card is pushed onto " +
                "the top.",
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
                "The tutorial is setting up a Stack recycling demonstration. The next step " +
                "shows how the top of the Stack changes.",
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
                "Press Continue to run the Stack recycling demonstration.",
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
            Title = "Recycling onto the Stack",
            Message =
                "Each recycled card is pushed onto the top. Because the Stack uses LIFO, " +
                "the last card pushed on top is the first one retrieved.",
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
                "You've seen LIFO retrieval, Peek, automatic Placement, and how recycled " +
                "cards return to the top of the Stack.",
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
            Title = "How Random Retrieval Works",
            Message =
                "A Random List draws from the cards currently available in its draw pool. " +
                "It does not guarantee a fixed retrieval order.",
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
            Title = "View the Draw Pool",
            Message =
                "Select the Random List deck and choose View Draw Pool to see which cards " +
                "are currently available to be drawn.",
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
            Title = "Understanding Random Draws",
            Message =
                "Viewing the pool shows which cards are available, not which card will be " +
                "drawn next. The next card is selected randomly from that pool.",
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
            Title = "Advance to Placement",
            Message =
                "Press Continue and observe the hand supplied by Random List draws.",
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
            Title = "Deploy from the Draw Pool",
            Message =
                "Your hand was drawn from the available pool. Deploy at least one Attack " +
                "(up to two), then continue. The open hand slots will help show random " +
                "draws after recycling.",
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
            Title = "Complete Your Placement Phase",
            Message =
                "Your Placement is done. Press Continue to enter the Attack phase.",
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
            Title = "Complete Your Attack Phase",
            Message =
                "Your Attack action is done. Press Continue to proceed to End.",
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
            Title = "Returning Cards to the Pool",
            Message =
                "Recycled cards return to the draw pool and become available for future " +
                "random draws.",
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
                "The tutorial is setting up a Random List recycling demonstration. The next " +
                "step shows the recycled cards returning to the pool.",
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
                "Press Continue to run the Random List recycling demonstration.",
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
            Title = "Recycling into the Draw Pool",
            Message =
                "The recycled cards are available in the draw pool again. Their previous " +
                "discard order does not determine which card is drawn first.",
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
                "You've seen the draw pool, random retrieval, and how recycled cards become " +
                "available for future draws.",
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
            Title = "How a Linked List Works",
            Message =
                "A Linked List stores cards as connected nodes. Retrieval starts at the " +
                "head and follows each node's link to the next card.",
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
            Title = "Reorder the Nodes",
            Message =
                "Select the Linked List deck and choose Reorder Nodes to change the order " +
                "of its card nodes.",
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
            Title = "Move a Card Node",
            Message =
                "Move one card node to a different position. This changes the sequence in " +
                "which cards will be retrieved.",
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
            Title = "Confirm the New Order",
            Message =
                "Finish the reorder action to keep the new node order.",
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
            Title = "Understanding Head-to-Tail Order",
            Message =
                "The first node is the head. Cards are retrieved from the head onward, " +
                "following the links through the sequence you arranged.",
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
            Title = "Advance to Placement",
            Message =
                "Press Continue and observe the cards supplied by the Linked List's current " +
                "order.",
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
            Title = "Deploy from Your Linked List",
            Message =
                "Your hand reflects the linked-node order. Deploy at least one Attack (up " +
                "to two), then continue. The open hand slots will help show cards drawn " +
                "later from the updated list.",
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
            Title = "Complete Your Placement Phase",
            Message =
                "Your Placement is done. Press Continue to enter the Attack phase.",
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
            Title = "Complete Your Attack Phase",
            Message =
                "Your Attack action is done. Press Continue to proceed to End.",
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
            Title = "Appending Recycled Nodes",
            Message =
                "Recycled cards are appended as new nodes at the tail of the Linked List, " +
                "after the nodes already there.",
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
                "The tutorial is setting up a Linked List recycling demonstration. The next " +
                "step shows recycled cards being appended to the tail.",
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
                "Press Continue to run the Linked List recycling demonstration.",
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
            Title = "Recycling to the Tail",
            Message =
                "Each recycled card becomes a node at the tail. Nodes closer to the head " +
                "remain earlier in the retrieval sequence.",
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
                "You've seen linked-list retrieval, node reordering, and how recycled cards " +
                "are appended to the tail.",
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
            $"Tutorial Progress: {LearnToPlayDeckCardIds.Count} required cards";
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
public static string GetLearnToPlayDeckInstruction(
    string cardId)
{
    return cardId switch
    {
        "UM1" =>
    "Your next card is Energy Potion.\n\nMagic • Utility\n\nUse the Utilities and Magic filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"CM2" =>
    "Your next card is Sorcerer Red Panda.\n\nMagic • Character\n\nUse the Characters and Magic filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"AM6" =>
    "Your next card is Fireball.\n\nMagic • Attack\n\nUse the Attacks and Magic filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"AP4" =>
    "Your next card is Piercing Strike.\n\nPhysical • Attack\n\nUse the Attacks and Physical filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"AM3" =>
    "Your next card is Ice Shards.\n\nMagic • Attack\n\nUse the Attacks and Magic filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"AN1" =>
    "Your next card is Pounce.\n\nNeutral • Attack\n\nUse the Attacks and Neutral filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"CN1" =>
    "Your next card is Wanderer Red Panda.\n\nNeutral • Character\n\nUse the Characters and Neutral filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"UN4" =>
    "Your next card is Discard For Energy.\n\nNeutral • Utility\n\nUse the Utilities and Neutral filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"UN1" =>
    "Your next card is Shield Booster.\n\nNeutral • Utility\n\nUse the Utilities and Neutral filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"AN2" =>
    "Your next card is Chain Lightning.\n\nNeutral • Attack\n\nUse the Attacks and Neutral filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"UM2" =>
    "Your next card is Energy Drain.\n\nMagic • Utility\n\nUse the Utilities and Magic filters below to find it quickly, then press + to add it to your Tutorial Deck.",

"UN3" =>
    "Your next card is Discount Coupon.\n\nNeutral • Utility\n\nUse the Utilities and Neutral filters below to find it quickly, then press + to add it to your Tutorial Deck.",

        _ =>
            "Find the highlighted Tutorial card."
    };
}
}
