namespace RedPandaTCD_Web.Game;

public enum TutorialAction
{
    None,
    Acknowledge,

    // Utility and deck actions.
    SelectUtility,
    PlayUtility,
    DiscardAndDraw,
    PeekDeck,
    ViewDrawPool,
    StartReorderNodes,
    MoveReorderNode,
    FinishReorderNodes,

    // Placement.
    DeployCharacter,
    DeployAttack,
    ReplaceAttack,
    RestAttack,
    ReplaceCharacter,
    ResolveWandererEntry,
    ObserveAutomaticPlacement,

    // Attack.
    UseAttack,
    UseCharacterAbility,

    // Battle flow.
    ContinuePhase,

    // DSA recycling demonstration.
    PrepareRecycle,
    ObserveRecycle,

    // Learn to Play ending.
    Surrender,
    ObserveBoard,
    ReturnToTutorial,

    // Short DSA tutorial ending.
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
            "CM2", // Sorcerer Red Panda
            "CN1", // Wanderer Red Panda

            // Attacks
            "AM3", // Ice Shards
            "AM6", // Fireball
            "AP4", // Piercing Strike
            "AN1", // Pounce
            "AN2", // Chain Lightning

            // Utilities
"UM1", // Energy Potion
"UN1", // Shield Booster
"UN4", // Discard For Energy
"UM2", // Energy Drain
"UN3" // Discount Coupon
        };


    /*
        LEARN TO PLAY QUEUE ORDER

        This is the exact Queue order required by
        the scripted Learn to Play battle.
    */
    public static readonly IReadOnlyList<string>
        LearnToPlayDeckOrder =
        new List<string>
        {
            "UM1", // Energy Potion
            "CM2", // Sorcerer Red Panda
            "AM6", // Fireball
            "AP4", // Piercing Strike
            "AM3", // Ice Shards
            "AN1", // Pounce
            "CN1", // Wanderer Red Panda
            "UN4", // Discard For Energy
            "UN1", // Shield Booster
            "AN2",  // Chain Lightning
            "UM2", // Energy Drain
            "UN3" // Discount Coupon
        };


    /*
        Learn to Play Red Panda TCD Tutorial

        The Tutorial layer explains what the player
        should learn.

        The real Battle engine still performs every
        actual gameplay action.
    */
private static readonly IReadOnlyList<TutorialStep>
    LearnToPlaySteps =
    new List<TutorialStep>
    {
        /*
            OPENING
        */

        new()
        {
            Number = 0,
            Title = "Welcome to Battle",
            Message =
                "This is the battlefield. " +
                "Your goal is to defeat your opponent by reducing " +
                "their Energy to 0. " +
                "You'll learn the battle system by playing an actual match.",
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
                "Energy is one of your most important resources. " +
                "Cards and abilities can cost Energy, and reaching 0 " +
                "can end the battle.",
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
                "Cards drawn from your deck become available in your hand. " +
                "Your hand contains the cards you can currently play, " +
                "deploy, or discard.",
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
                "Your deck uses a data structure to determine how cards " +
                "are stored and retrieved. For now, we'll focus on using " +
                "those cards. The Data Structure Tutorials explain how " +
                "each structure works.",
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
                "You won the coin flip, so you have Initiative on Turn 1. " +
                "The player with Initiative acts first during every phase " +
                "of the current turn. Initiative swaps each turn.",
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
                "Every turn follows four phases: Utility, Placement, " +
                "Attack, then End. During each phase, both players get " +
                "their action according to Initiative. The center tracker " +
                "shows the current phase, acting player, and Initiative.",
            RequiredAction =
                TutorialAction.Acknowledge,
            HighlightTarget =
                "phase-bar",
            RestrictInput =
                true
        },


        /*
            TURN 1 - UTILITY
        */

        new()
        {
            Number = 8,
            Title = "Utility Cards",
            Message =
                "During the Utility Phase, you can use Utility cards " +
                "for special effects. Select Energy Potion to inspect it.",
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
                "Energy Potion restores Energy. Read its effect, " +
                "then play the Utility card.",
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
        "The Battle Log records what happens as the battle progresses. " +
        "Use it to follow actions, effects, and combat resolution. " +
        "You can copy the log and paste it into the Simulator to analyze or replay the match." +
        "When you're ready, Continue to the Placement Phase.",
    RequiredAction =
        TutorialAction.ContinuePhase,
    HighlightTarget =
        "battle-log",
    RestrictInput =
        true
},


        /*
            TURN 1 - PLACEMENT
        */

        new()
        {
            Number = 11,
            Title = "Placement Phase",
            Message =
                "Placement is where you prepare your field. " +
                "You'll begin by deploying a Character and two Attacks.",
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
                "Characters have HP and Shield, but also three abilities. " +
                "Entry activates when the Character enters battle. " +
                "Passive works automatically while its conditions are met. " +
                "Active is an ability you deliberately activate.",
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
            Title = "Magic Surge Entry",
            Message =
                "Sorcerer's Entry ability, Magic Surge, is active for this turn. " +
                "Magic Surge gives your Magic Attacks +1 Damage. " +
                "The active bonus is included in Sorcerer's badge.",
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
                "Deploy Fireball now. Fireball is a Magic Attack, " +
                "so it benefits from Sorcerer's Magic damage bonuses.",
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
            Title = "Stacked Magic Bonuses",
            Message =
                "Sorcerer's badge now shows +2 MAGIC. " +
                "Magic Surge provides +1 because Sorcerer was deployed this turn. " +
                "Arcane Focus provides another +1 because no Physical Attack " +
                "is currently deployed.",
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
                "Now deploy Piercing Strike. Piercing Strike is Physical, " +
                "so Arcane Focus will become inactive. " +
                "Watch Sorcerer's badge drop from +2 MAGIC to +1 MAGIC. " +
                "The remaining +1 comes from Magic Surge.",
            RequiredAction =
                TutorialAction.DeployAttack,
            HighlightTarget =
                "piercing-attack",
            RestrictInput =
                true
        },


        /*
            TURN 1 - ATTACK
        */

       new()
{
    Number = 18,
    Title = "Field Ready",
    Message =
        "Your Character and both Attacks are now deployed. " +
        "Continue to complete Placement and enter the Attack Phase.",
    RequiredAction =
        TutorialAction.ContinuePhase,
    HighlightTarget =
        "phase-placement",
    RestrictInput =
        true
},

new()
{
    Number = 19,
    Title = "Attack Phase",
    Message =
        "Your deployed Attacks can now be used. " +
        "Attack order matters. Start with Piercing Strike.",
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
                "The opponent still had Shield, but their Character " +
                "was damaged. Piercing can deal direct damage through Shield. " +
                "Compare the opponent's HP and Shield with the Battle Log.",
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
    "Now use Fireball. Unlike Piercing Strike, Fireball " +
    "interacts with the opponent's defenses normally. " +
    "Shield acts as a hit barrier, so a normal hit consumes " +
    "Shield points until it breaks once the shield is " +
    "broken you can deal damage to the character" +
    "Damage does not overflow from shield to character so prioritize breaking it first",
    
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
    Title = "Complete the Attack Phase",
    Message =
        "Both of your deployed Attacks have been used. " +
        "Continue so your opponent can complete their Attack action " +
        "and the battle can enter the End Phase.",
    RequiredAction =
        TutorialAction.ContinuePhase,
    HighlightTarget =
        "phase-attack",
    RestrictInput =
        true
},

new()
{
    Number = 23,
    Title = "Defense",
    Message =
        "Your opponent has now attacked. Shield and Character HP " +
        "absorb incoming damage according to the Attack being resolved. " +
        "Compare your Character's values with the Battle Log.",
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
    Title = "End Phase",
    Message =
        "The battle is now in the End Phase. End-of-turn effects " +
        "resolve here before the next turn begins. Energy recovery " +
        "and other End Phase effects appear in the Battle Log.",
    RequiredAction =
        TutorialAction.Acknowledge,
    HighlightTarget =
        "phase-end",
    RestrictInput =
        true
},

new()
{
    Number = 25,
    Title = "Begin Turn 2",
    Message =
        "Continue to complete your End Phase. Your opponent will " +
        "complete theirs, then Turn 2 will begin.",
    RequiredAction =
        TutorialAction.ContinuePhase,
    HighlightTarget =
        "phase-end",
    RestrictInput =
        true
},

/*
    TURN 2 - UTILITY
*/

new()
{
    Number = 26,
    Title = "Initiative and Effect Duration",
    Message =
        "Initiative has swapped for Turn 2, so your opponent acted first. " +
        "Magic Surge has expired because Sorcerer's deployment turn ended. " +
        "Piercing Strike is still Physical, so Arcane Focus remains inactive " +
        "and Sorcerer currently shows no Magic damage bonus.",
    RequiredAction =
        TutorialAction.Acknowledge,
    HighlightTarget =
        "player-character",
    RestrictInput =
        true
},

new()
{
    Number = 27,
    Title = "Discard & Draw",
    Message =
        "Utility Phase isn't only for Utility cards. " +
        "If the cards in your hand aren't what you need, " +
        "you can use Discard & Draw. Perform one now.",
    RequiredAction =
        TutorialAction.DiscardAndDraw,
    HighlightTarget =
        "player-hand",
    RestrictInput =
        true
},

new()
{
    Number = 28,
    Title = "Card Movement",
    Message =
        "Your discarded card moved to the Discard Pile, and another " +
        "card became available. Cards move through your deck, hand, " +
        "field, and discard throughout battle.",
    RequiredAction =
        TutorialAction.Acknowledge,
    HighlightTarget =
        "discard-pile",
    RestrictInput =
        true
},

new()
{
    Number = 29,
    Title = "Continue to Placement",
    Message =
        "Your Utility action is complete. Continue to enter " +
        "the Placement Phase.",
    RequiredAction =
        TutorialAction.ContinuePhase,
    HighlightTarget =
        "phase-utility",
    RestrictInput =
        true
},

/*
    TURN 2 - PLACEMENT
*/

new()
{
    Number = 30,
    Title = "Replacing Attacks",
    Message =
        "Piercing Strike has done its job. Attacks don't have " +
        "to remain on the field for the whole battle. Replace it " +
        "with Ice Shards, your Multi-Hit Attack.",
    RequiredAction =
        TutorialAction.ReplaceAttack,
    HighlightTarget =
        "piercing-slot",
    RestrictInput =
        true
},

new()
{
    Number = 31,
    Title = "Arcane Focus Returns",
    Message =
        "Piercing Strike has been replaced by Ice Shards. " +
        "Your deployed Attacks are now both Magic, so Arcane Focus " +
        "has automatically become active again. " +
        "Sorcerer's badge now shows +1 MAGIC.",
    RequiredAction =
        TutorialAction.Acknowledge,
    HighlightTarget =
        "player-character",
    RestrictInput =
        true
},

new()
{
    Number = 32,
    Title = "Continue to Attack",
    Message =
        "Your Placement changes are complete. Continue to enter " +
        "the Attack Phase.",
    RequiredAction =
        TutorialAction.ContinuePhase,
    HighlightTarget =
        "phase-placement",
    RestrictInput =
        true
},

/*
    TURN 2 - ATTACK
*/

new()
{
    Number = 33,
    Title = "Active Abilities",
    Message =
        "Active abilities are different from Passives. " +
        "You choose when to activate them, and they can require Energy. " +
        "Use Sorcerer's Active ability now.",
    RequiredAction =
        TutorialAction.UseCharacterAbility,
    HighlightTarget =
        "player-character",
    RestrictInput =
        true
},

new()
{
    Number = 34,
    Title = "Fireball Again",
    Message =
        "Use Fireball again. Sorcerer's Arcane Focus and Active bonus " +
        "will be included in the real damage calculation.",
    RequiredAction =
        TutorialAction.UseAttack,
    HighlightTarget =
        "fireball",
    RestrictInput =
        true
},

new()
{
    Number = 35,
    Title = "Fatigue",
    Message =
        "Fireball has reached its use limit and is now Fatigued. " +
        "Fatigued Attacks must be dealt with during Placement. " +
        "Leave Fireball for now. You'll resolve it next turn.",
    RequiredAction =
        TutorialAction.Acknowledge,
    HighlightTarget =
        "fireball",
    RestrictInput =
        true
},

new()
{
    Number = 36,
    Title = "Multi-Hit",
    Message =
        "Use Ice Shards. Multi-Hit Attacks strike more than once, " +
        "and each hit resolves separately.",
    RequiredAction =
        TutorialAction.UseAttack,
    HighlightTarget =
        "multi-hit-attack",
    RestrictInput =
        true
},

new()
{
    Number = 37,
    Title = "Multi-Hit Resolution",
    Message =
        "Each hit resolved separately. Compare the individual hits " +
        "in the Battle Log with the opponent's Shield and HP.",
    RequiredAction =
        TutorialAction.Acknowledge,
    HighlightTarget =
        "battle-log",
    RestrictInput =
        true
},

new()
{
    Number = 38,
    Title = "Complete the Attack Phase",
    Message =
        "Your Turn 2 Attack actions are complete. Continue to enter " +
        "the End Phase.",
    RequiredAction =
        TutorialAction.ContinuePhase,
    HighlightTarget =
        "phase-attack",
    RestrictInput =
        true
},

/*
    TURN 2 - END
*/

new()
{
    Number = 39,
    Title = "End Phase Recovery",
    Message =
        "Shield and other effects can change during End Phase. " +
        "Compare the battlefield values with the End Phase entries " +
        "in the Battle Log.",
    RequiredAction =
        TutorialAction.Acknowledge,
    HighlightTarget =
        "player-character",
    RestrictInput =
        true
},

new()
{
    Number = 40,
    Title = "Active Cooldown",
    Message =
        "Sorcerer's Active was used and has entered cooldown. " +
        "Powerful abilities cannot necessarily be used every time " +
        "you act.",
    RequiredAction =
        TutorialAction.Acknowledge,
    HighlightTarget =
        "player-character",
    RestrictInput =
        true
},

new()
{
    Number = 41,
    Title = "Begin Turn 3",
    Message =
        "Continue to complete the End Phase and begin Turn 3. " +
        "Initiative will return to you.",
    RequiredAction =
        TutorialAction.ContinuePhase,
    HighlightTarget =
        "phase-end",
    RestrictInput =
        true
},

/*
    TURN 3 - UTILITY
*/

new()
{
    Number = 42,
    Title = "Peek",
    Message =
        "Click your deck and use Peek. Peek lets you inspect up to " +
        "the next two cards without drawing them. Different deck " +
        "structures provide different Utility actions when clicked.",
    RequiredAction =
        TutorialAction.PeekDeck,
    HighlightTarget =
        "player-deck",
    RestrictInput =
        true
},

new()
{
    Number = 43,
    Title = "Queue Structure",
    Message =
        "Your Tutorial deck uses a Queue. Cards leave a Queue " +
        "in First-In, First-Out order. The dedicated Queue Tutorial " +
        "explores this data structure in more detail.",
    RequiredAction =
        TutorialAction.Acknowledge,
    HighlightTarget =
        "player-deck",
    RestrictInput =
        true
},

new()
{
    Number = 44,
    Title = "Continue to Placement",
    Message =
        "Peek is complete. Continue to enter the Placement Phase " +
        "and resolve Fireball's Fatigue.",
    RequiredAction =
        TutorialAction.ContinuePhase,
    HighlightTarget =
        "phase-utility",
    RestrictInput =
        true
},

/*
    TURN 3 - PLACEMENT
*/

new()
{
    Number = 45,
    Title = "Resolving Fatigue",
    Message =
        "Fireball is still Fatigued. You cannot leave Placement while " +
        "a Fatigued Attack is unresolved. You've already replaced an " +
        "Attack, so this time Rest Fireball.",
    RequiredAction =
        TutorialAction.RestAttack,
    HighlightTarget =
        "fireball",
    RestrictInput =
        true
},

new()
{
    Number = 46,
    Title = "Replacing Characters",
    Message =
        "Characters can be replaced during Placement. " +
        "Replacing an active Character normally costs 4 Energy. " +
        "If your previous Character was defeated, however, deploying " +
        "the replacement Character is free. Select the Character slot.",
    RequiredAction =
        TutorialAction.ReplaceCharacter,
    HighlightTarget =
        "player-character",
    RestrictInput =
        true
},

new()
{
    Number = 47,
    Title = "Inspect Wanderer",
    Message =
        "Inspect Wanderer Red Panda in the Character picker. " +
        "Wanderer has a different Entry, Passive, and Active ability " +
        "from Sorcerer.",
    RequiredAction =
        TutorialAction.Acknowledge,
    HighlightTarget =
        "wanderer",
    RestrictInput =
        true
},

new()
{
    Number = 48,
    Title = "Deploy Wanderer",
    Message =
        "Deploy Wanderer Red Panda. If Sorcerer was defeated, this " +
        "replacement deployment is free. Otherwise, normal Character " +
        "replacement costs apply.",
    RequiredAction =
        TutorialAction.DeployCharacter,
    HighlightTarget =
        "wanderer",
    RestrictInput =
        true
},

new()
{
    Number = 49,
    Title = "Wanderer's Entry",
    Message =
        "Wanderer's Adapt Entry lets you choose one deployed Attack " +
        "and reduce its buildup cost by 2. Select one of the " +
        "highlighted Attacks now.",
    RequiredAction =
        TutorialAction.ResolveWandererEntry,
    HighlightTarget =
        "wanderer-entry",
    RestrictInput =
        true
},

new()
{
    Number = 50,
    Title = "Complete Placement",
    Message =
        "Wanderer's Entry has resolved. Continue to complete Placement " +
        "and enter the Attack Phase.",
    RequiredAction =
        TutorialAction.ContinuePhase,
    HighlightTarget =
        "phase-placement",
    RestrictInput =
        true
},

/*
    TURN 3 - ATTACK
*/

new()
{
    Number = 51,
    Title = "Wanderer's Passive",
    Message =
        "Wanderer's Free Peek Passive allows Queue, Stack, and " +
        "Random List deck inspection without consuming a Utility Action. " +
        "Now use Ice Shards during this Attack Phase.",
    RequiredAction =
        TutorialAction.UseAttack,
    HighlightTarget =
        "multi-hit-attack",
    RestrictInput =
        true
},

/*
    TUTORIAL ENDING
*/

new()
{
    Number = 52,
    Title = "Battle Training Complete",
    Message =
        "You've learned the core battle system: phase order, Initiative, " +
        "deployment, replacement, Character abilities, attacks, defenses, " +
        "Fatigue, Utility actions, and the Battle Log.",
    RequiredAction =
        TutorialAction.Acknowledge,
    HighlightTarget =
        "battle-board",
    RestrictInput =
        true
},

new()
{
    Number = 53,
    Title = "Ending a Match",
    Message =
        "A battle can end in Victory, Defeat, or a Draw. Victory means " +
        "your opponent was defeated. Defeat means you were defeated. " +
        "A Draw occurs when neither player wins.",
    RequiredAction =
        TutorialAction.Acknowledge,
    HighlightTarget =
        "battle-board",
    RestrictInput =
        true
},

new()
{
    Number = 54,
    Title = "Surrender",
    Message =
        "You can also end a match by surrendering. For this tutorial, " +
        "surrender now. The tutorial will not continue until you do.",
    RequiredAction =
        TutorialAction.Surrender,
    HighlightTarget =
        "surrender-button",
    RestrictInput =
        true
},

new()
{
    Number = 55,
    Title = "Observe Board",
    Message =
        "After a match ends, Observe Board lets you inspect the final " +
        "battlefield and Battle Log without continuing play. " +
        "Choose Observe Board now.",
    RequiredAction =
        TutorialAction.ObserveBoard,
    HighlightTarget =
        "observe-board-button",
    RestrictInput =
        true
},

new()
{
    Number = 56,
    Title = "Battle Tutorial Complete",
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
                "phase-bar",
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
                "phase-placement",
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
                "phase-attack",
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
            Title = "Resolve End Phase",
            Message =
                "Continue so the discarded cards can return to the Queue.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "phase-end",
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
                "phase-bar",
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
                "phase-placement",
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
                "phase-attack",
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
            Title = "Resolve End Phase",
            Message =
                "Continue to reinsert the discarded cards.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "phase-end",
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
                "phase-placement",
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
                "phase-placement",
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
                "phase-attack",
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
            Title = "Resolve End Phase",
            Message =
                "Continue to push the discarded cards back onto the Stack.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "phase-end",
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
                "phase-bar",
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
                "phase-placement",
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
                "phase-attack",
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
            Title = "Resolve End Phase",
            Message =
                "Continue to return the discarded cards to the draw pool.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "phase-end",
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
                "phase-bar",
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
                "phase-placement",
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
                "phase-attack",
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
            Title = "Resolve End Phase",
            Message =
                "Continue to append the discarded cards to the Linked List.",
            RequiredAction =
                TutorialAction.ContinuePhase,
            HighlightTarget =
                "phase-end",
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