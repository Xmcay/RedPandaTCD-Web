namespace RedPandaTCD_Web.Game;

public static class GameSetupManager
{
    public static MatchState StartTrainingMatch(
        string playerName,
        Deck playerDeck,
        Deck botDeck)
    {
        Player player = CreatePlayer(
            playerName,
            false,
            playerDeck);

        Player bot = CreatePlayer(
            "Bot",
            true,
            botDeck);

        return BattleEngine.StartMatch(
            player,
            bot,
            GameMode.BotMatch);
    }

public static MatchState StartLearnToPlayMatch(
    string playerName,
    Deck tutorialDeck)
{
    Deck orderedPlayerDeck =
        CreateOrderedLearnToPlayDeck(
            tutorialDeck);

    Deck weakBotDeck =
        CreateWeakLearnToPlayBotDeck(
            tutorialDeck);

    Player player =
        CreatePlayer(
            playerName,
            false,
            orderedPlayerDeck);

    Player bot =
        CreatePlayer(
            "Tutorial Bot",
            true,
            weakBotDeck);

    return BattleEngine.StartMatch(
        player,
        bot,
        GameMode.BotMatch,
        forcePlayerOneStarts: true);
}

    public static MatchState StartDsaTutorialMatch(
        string playerName,
        TutorialType tutorial)
    {
        Deck playerDeck =
            GetDsaTutorialDeck(
                tutorial);

        /*
            The Bot receives a normal Queue deck.

            Only the player's deck needs to demonstrate
            the selected structure. Using Queue for the
            Bot avoids applying player-facing automatic
            Stack behavior to both sides.
        */
        Deck botDeck =
            StarterDeckFactory.CreateQueueStarter();

        Player player = CreatePlayer(
            playerName,
            false,
            playerDeck);

        Player bot = CreatePlayer(
            "Tutorial Bot",
            true,
            botDeck);

        return BattleEngine.StartMatch(
            player,
            bot,
            GameMode.BotMatch,
            forcePlayerOneStarts: true);
    }

    public static Deck GetDsaTutorialDeck(
        TutorialType tutorial)
    {
        return tutorial switch
        {
            TutorialType.Queue =>
                StarterDeckFactory.CreateQueueStarter(),

            TutorialType.PriorityQueue =>
                StarterDeckFactory.CreatePriorityStarter(),

            TutorialType.Stack =>
                StarterDeckFactory.CreateStackStarter(),

            TutorialType.RandomList =>
                StarterDeckFactory.CreateRandomStarter(),

            TutorialType.LinkedList =>
                StarterDeckFactory.CreateLinkedStarter(),

            _ =>
                throw new ArgumentException(
                    "The selected Tutorial does not have " +
                    "a DSA Battle deck.",
                    nameof(tutorial))
        };
    }

    public static MatchState StartPassAndPlay(
        string player1Name,
        Deck player1Deck,
        string player2Name,
        Deck player2Deck)
    {
        Player player1 = CreatePlayer(
            player1Name,
            false,
            player1Deck);

        Player player2 = CreatePlayer(
            player2Name,
            false,
            player2Deck);

        return BattleEngine.StartMatch(
            player1,
            player2,
            GameMode.PassAndPlay);
    }

    public static MatchState StartTournamentMatch(
        string playerName,
        TournamentState tournament)
    {
        if (tournament.LockedPlayerDeck == null)
        {
            throw new InvalidOperationException(
                "Tournament player deck has not been locked.");
        }

        Deck? opponentDeck =
            TournamentManager.GetOpponentDeck(
                tournament.CurrentRound);

        if (opponentDeck == null)
        {
            throw new InvalidOperationException(
                "Tournament opponent deck could not be created.");
        }

        Player player = CreatePlayer(
            playerName,
            false,
            tournament.LockedPlayerDeck);

        Player opponent = CreatePlayer(
            tournament.CurrentOpponentName,
            true,
            opponentDeck);

        return BattleEngine.StartMatch(
            player,
            opponent,
            GameMode.BotMatch);
    }

    private static Player CreatePlayer(
        string name,
        bool isBot,
        Deck deck)
    {
        return new Player
        {
            Name = name,
            IsBot = isBot,
            Deck = DeckCloner.Clone(
                deck)
        };
    }
    private static Deck CreateOrderedLearnToPlayDeck(
    Deck sourceDeck)
{
    Deck orderedDeck =
        DeckCloner.Clone(
            sourceDeck);

    List<Card> orderedCards =
        new();

    foreach (string requiredId in
             TutorialManager.LearnToPlayDeckOrder)
    {
        Card? card =
            orderedDeck.Cards.FirstOrDefault(
                candidate =>
                    candidate.Id == requiredId);

        if (card == null)
        {
            throw new InvalidOperationException(
                $"Learn to Play card {requiredId} " +
                "could not be found in the Tutorial deck.");
        }

        orderedCards.Add(
            card);
    }

    orderedDeck.Structure =
        DeckStructure.Queue;

    orderedDeck.Cards =
        orderedCards;

    return orderedDeck;
}
private static Deck CreateWeakLearnToPlayBotDeck(
    Deck sourceDeck)
{
    string[] botDeckOrder =
    {
        "UM1",
        "CN1",
        "AN1",
        "AN2",
        "AN1",
        "AN2",
        "CM2",
        "UN4",
        "UN1",
        "UN3",
        "UM2",
        "AM3"
    };

    List<Card> orderedCards =
        new();

    foreach (string requiredId in botDeckOrder)
    {
        /*
            Clone the source for every requested card.

            This is important for duplicate Attacks.
            Each Pounce and Chain Lightning must be a
            separate runtime Card instance.
        */
        Deck cardSource =
            DeckCloner.Clone(
                sourceDeck);

        Card? card =
            cardSource.Cards.FirstOrDefault(
                candidate =>
                    string.Equals(
                        candidate.Id,
                        requiredId,
                        StringComparison.OrdinalIgnoreCase));

        if (card == null)
        {
            throw new InvalidOperationException(
                $"Tutorial Bot card {requiredId} " +
                "could not be found.");
        }

        orderedCards.Add(
            card);
    }

    Deck botDeck =
        new()
        {
            Name =
                "Weak Tutorial Bot Deck",

            Structure =
                DeckStructure.Queue,

            Cards =
                orderedCards
        };

    if (!RedPandaTCD_Web.Game.DeckBuilder.ValidateDeck(
            botDeck,
            out string validationError))
    {
        throw new InvalidOperationException(
            "Weak Tutorial Bot deck is invalid: " +
            validationError);
    }

    return botDeck;
}
}