namespace RedPandaTCD_Web.Game;

public class ReplayMatchState
{
    private readonly Player baselinePlayer1;
    private readonly Player baselinePlayer2;
    private readonly bool playerOneStarts;

    public MatchState Match { get; }

    public List<BattleLogEntry> Events { get; }

    public int CurrentEventIndex { get; private set; }

    public bool IsPlaying { get; set; }

    public bool IsFinished =>
        CurrentEventIndex >= Events.Count;

    public BattleLogEntry? CurrentEvent =>
    CurrentEventIndex > 0 &&
    CurrentEventIndex <= Events.Count
        ? Events[CurrentEventIndex - 1]
        : null;

    public IReadOnlyList<BattleLogEntry> VisibleEvents =>
        Events
            .Take(CurrentEventIndex)
            .Where(entry => entry.ShowInLog)
            .ToList()
            .AsReadOnly();

    public Player Player1 => Match.Player1;

    public Player Player2 => Match.Player2;

    public int TurnNumber =>
        Match.TurnNumber;

    public MatchPhase CurrentPhase =>
        Match.CurrentPhase;

    public ReplayMatchState(
        Player player1,
        Player player2,
        bool playerOneStarts,
        IEnumerable<BattleLogEntry> events)
    {
        baselinePlayer1 =
            CreateReplayPlayer(player1);

        baselinePlayer2 =
            CreateReplayPlayer(player2);

        this.playerOneStarts =
            playerOneStarts;

        Match =
            new MatchState(
                CreateReplayPlayer(player1),
                CreateReplayPlayer(player2),
                playerOneStarts);

        Events =
            events.ToList();

        CurrentEventIndex = 0;
        IsPlaying = false;

        Reset();
    }

    public void Reset()
    {
        CopyPlayerState(
            baselinePlayer1,
            Match.Player1);

        CopyPlayerState(
            baselinePlayer2,
            Match.Player2);

        Match.TurnNumber = 1;

        Match.CurrentPhase =
            MatchPhase.Utility;

        Match.IsCompleted = false;
        Match.IsDraw = false;

        Match.PlayerOneStarts =
            playerOneStarts;

        Match.ActingPlayerIndex =
            Match.DoesPlayerOneActFirst()
                ? 1
                : 2;

        CurrentEventIndex = 0;
        IsPlaying = false;
    }

    public bool MoveNext()
    {
        if (IsFinished)
            return false;

        CurrentEventIndex++;

        RebuildState();

        return true;
    }

    public bool MovePrevious()
    {
        if (CurrentEventIndex <= 0)
            return false;

        CurrentEventIndex--;

        RebuildState();

        return true;
    }

    public void JumpToStart()
    {
        CurrentEventIndex = 0;
        IsPlaying = false;

        RebuildState();
    }

    public void JumpToEnd()
    {
        CurrentEventIndex =
            Events.Count;

        IsPlaying = false;

        RebuildState();
    }

    public void SetEventIndex(int index)
    {
        CurrentEventIndex =
            Math.Clamp(
                index,
                0,
                Events.Count);

        RebuildState();
    }

    public void RebuildToCurrentEvent()
    {
        RebuildState();
    }

    public void UpdateMatchContext(
        BattleLogEntry entry)
    {
        if (entry.TurnNumber > 0)
        {
            Match.TurnNumber =
                entry.TurnNumber;
        }

        if (entry.PlayerIndex == 1 ||
            entry.PlayerIndex == 2)
        {
            Match.ActingPlayerIndex =
                entry.PlayerIndex;
        }

        Match.CurrentPhase =
            entry.Phase;

        if (entry.EventType ==
            BattleLogEventType.MatchEnd)
        {
            Match.IsCompleted = true;
        }
    }

    private void RebuildState()
    {
        CopyPlayerState(
            baselinePlayer1,
            Match.Player1);

        CopyPlayerState(
            baselinePlayer2,
            Match.Player2);

        Match.TurnNumber = 1;

        Match.CurrentPhase =
            MatchPhase.Utility;

        Match.IsCompleted = false;
        Match.IsDraw = false;

        Match.PlayerOneStarts =
            playerOneStarts;

        Match.ActingPlayerIndex =
            Match.DoesPlayerOneActFirst()
                ? 1
                : 2;

        for (int i = 0;
             i < CurrentEventIndex;
             i++)
        {
            ApplyEvent(
                Events[i]);
        }
    }

    private void ApplyEvent(
        BattleLogEntry entry)
    {
        UpdateMatchContext(entry);

        Player? actor =
            GetPlayer(entry.PlayerIndex);

        Player? target = null;

        if (entry.TargetPlayerIndex.HasValue)
        {
            target =
                GetPlayer(
                    entry.TargetPlayerIndex.Value);
        }

        if (actor == null &&
            target == null)
        {
            return;
        }

        switch (entry.EventType)
        {
            case BattleLogEventType.Draw:
                if (actor != null)
                {
                    ApplyDraw(
                        actor,
                        entry);
                }

                break;

            case BattleLogEventType.Utility:
                if (actor != null)
                {
                    ApplyCardAction(
                        actor,
                        entry,
                        addToDiscard: true);
                }

                break;

            case BattleLogEventType.Placement:
                if (actor != null)
                {
                    ApplyPlacement(
                        actor,
                        entry);
                }

                break;

            case BattleLogEventType.Damage:
                ApplyStateSnapshot(
                    entry,
                    actor,
                    target);

                break;

            case BattleLogEventType.EnergyChange:
                ApplyStateSnapshot(
                    entry,
                    actor,
                    target);

                break;

            case BattleLogEventType.Ability:
                ApplyStateSnapshot(
                    entry,
                    actor,
                    target);

                break;

            case BattleLogEventType.CharacterDefeated:
                if (target != null)
                {
                    target.ActiveCharacter =
                        null;

                    target.CurrentCharacterHp =
                        0;
                }
                else if (actor != null)
                {
                    actor.ActiveCharacter =
                        null;

                    actor.CurrentCharacterHp =
                        0;
                }

                ApplyStateSnapshot(
                    entry,
                    actor,
                    target);

                break;

            case BattleLogEventType.MatchEnd:
                Match.IsCompleted = true;

                ApplyStateSnapshot(
                    entry,
                    actor,
                    target);

                break;

            default:
                ApplyStateSnapshot(
                    entry,
                    actor,
                    target);

                break;
        }
    }

    private void ApplyDraw(
        Player player,
        BattleLogEntry entry)
    {
        if (string.IsNullOrWhiteSpace(
            entry.CardName))
        {
            return;
        }

        Card? card =
            FindCard(
                player,
                entry.CardName);

        if (card == null)
        {
            return;
        }

        RemoveCardFromDeck(
            player,
            card);

        HandSlot? slot =
            player.HandSlots
                .FirstOrDefault(
                    value => value.IsEmpty);

        if (slot != null)
        {
            slot.CardInSlot =
                card.Clone();
        }
    }

    private void ApplyCardAction(
        Player player,
        BattleLogEntry entry,
        bool addToDiscard)
    {
        if (string.IsNullOrWhiteSpace(
            entry.CardName))
        {
            return;
        }

        Card? card =
            RemoveCardFromHand(
                player,
                entry.CardName);

        if (card == null)
        {
            return;
        }

        if (addToDiscard)
        {
            player.DiscardPile.Add(
                card);
        }

        ApplyStateSnapshot(
            entry,
            player,
            GetOpponent(player));
    }

   private void ApplyPlacement(
    Player player,
    BattleLogEntry entry)
{
    Console.WriteLine(
        $"PLACEMENT -> " +
        $"Player={entry.PlayerIndex} " +
        $"Card={entry.CardName} " +
        $"TargetSlot={entry.TargetSlotNumber}");

    if (string.IsNullOrWhiteSpace(
        entry.CardName))
    {
        ApplyStateSnapshot(
            entry,
            player,
            GetOpponent(player));

        return;
    }

    Card? card =
        RemoveCardFromHand(
            player,
            entry.CardName);

    if (card == null)
    {
        card =
            FindCard(
                player,
                entry.CardName);

        if (card == null)
        {
            Console.WriteLine(
                $"CARD NOT FOUND -> {entry.CardName}");

            ApplyStateSnapshot(
                entry,
                player,
                GetOpponent(player));

            return;
        }
    }

    if (card.Type ==
        CardType.Character)
    {
        player.ActiveCharacter =
            card.Clone();

        Console.WriteLine(
            $"CHARACTER SET -> " +
            $"{player.Name} " +
            $"{player.ActiveCharacter?.Name}");

        player.CurrentCharacterHp =
            entry.CharacterHp ??
            card.HpValue;

        player.CharacterNewlyDeployed =
            false;
    }
    else if (card.Type ==
             CardType.Attack)
    {
        int slot =
            entry.TargetSlotNumber ?? 1;

        if (slot == 2)
        {
            player.AttackSlot2 =
                card.Clone();

            Console.WriteLine(
                $"ATTACK2 SET -> " +
                $"{player.Name} " +
                $"{card.Name}");

            player.AttackSlot2UsesRemaining =
                2;

            player.AttackSlot2TotalUses =
                0;

            player.IsFatiguedSlot2 =
                false;

            player.IsRestingSlot2 =
                false;
        }
        else
        {
            player.AttackSlot1 =
                card.Clone();

            Console.WriteLine(
                $"ATTACK1 SET -> " +
                $"{player.Name} " +
                $"{card.Name}");

            player.AttackSlot1UsesRemaining =
                2;

            player.AttackSlot1TotalUses =
                0;

            player.IsFatiguedSlot1 =
                false;

            player.IsRestingSlot1 =
                false;
        }
    }

    ApplyStateSnapshot(
        entry,
        player,
        GetOpponent(player));
}

    private void ApplyStateSnapshot(
    BattleLogEntry entry,
    Player? actor,
    Player? target)
{
    Console.WriteLine(
    $"SNAPSHOT -> " +
    $"Actor={actor?.Name} " +
    $"Target={target?.Name} " +
    $"TargetIndex={entry.TargetPlayerIndex} " +
    $"Shield={entry.OpponentShield}");
    
    if (actor != null)
    {
        if (entry.PlayerEnergy.HasValue)
        {
            actor.Energy =
                entry.PlayerEnergy.Value;
        }

        if (entry.PlayerShield.HasValue)
        {
            actor.Shield =
                entry.PlayerShield.Value;
        }

        if (entry.CharacterHp.HasValue &&
            actor.ActiveCharacter != null)
        {
            actor.CurrentCharacterHp =
                entry.CharacterHp.Value;
        }
    }

    if (target != null)
    {
        if (entry.OpponentEnergy.HasValue)
        {
            target.Energy =
                entry.OpponentEnergy.Value;
        }

        if (entry.OpponentShield.HasValue)
        {
            target.Shield =
                entry.OpponentShield.Value;
        }
    }

    if (entry.TargetPlayerIndex == 1 &&
        entry.PlayerEnergy.HasValue)
    {
        Match.Player1.Energy =
            entry.PlayerEnergy.Value;
    }

    if (entry.TargetPlayerIndex == 2 &&
        entry.PlayerEnergy.HasValue)
    {
        Match.Player2.Energy =
            entry.PlayerEnergy.Value;
    }

    if (entry.TargetPlayerIndex == 1 &&
        entry.OpponentEnergy.HasValue)
    {
        Match.Player2.Energy =
            entry.OpponentEnergy.Value;
    }

    if (entry.TargetPlayerIndex == 2 &&
        entry.OpponentEnergy.HasValue)
    {
        Match.Player1.Energy =
            entry.OpponentEnergy.Value;
    }

    /*
     * Shield reconstruction.
     */

    if (entry.TargetPlayerIndex == 1 &&
        entry.PlayerShield.HasValue)
    {
        Match.Player1.Shield =
            entry.PlayerShield.Value;
    }

    if (entry.TargetPlayerIndex == 2 &&
        entry.PlayerShield.HasValue)
    {
        Match.Player2.Shield =
            entry.PlayerShield.Value;
    }

    if (entry.TargetPlayerIndex == 1 &&
        entry.OpponentShield.HasValue)
    {
        Match.Player2.Shield =
            entry.OpponentShield.Value;
    }

    if (entry.TargetPlayerIndex == 2 &&
        entry.OpponentShield.HasValue)
    {
        Match.Player1.Shield =
            entry.OpponentShield.Value;
    }

    if (entry.CharacterHp.HasValue)
    {
        Player? characterOwner =
            target ??
            actor;

        if (characterOwner != null &&
            characterOwner.ActiveCharacter != null)
        {
            characterOwner.CurrentCharacterHp =
                entry.CharacterHp.Value;

            if (characterOwner.CurrentCharacterHp <= 0)
            {
                characterOwner.ActiveCharacter =
                    null;

                characterOwner.CurrentCharacterHp =
                    0;
            }
        }
    }
}

    private Player? GetPlayer(
        int playerIndex)
    {
        return playerIndex switch
        {
            1 => Match.Player1,
            2 => Match.Player2,
            _ => null
        };
    }

    private Player GetOpponent(
        Player player)
    {
        return player == Match.Player1
            ? Match.Player2
            : Match.Player1;
    }

    private static Player CreateReplayPlayer(
        Player source)
    {
        Player player =
            new Player
            {
                Name = source.Name,
                Energy = source.Energy,
                MaxEnergy = source.MaxEnergy,
                Shield = source.Shield,
                IsBot = source.IsBot,
                Deck = DeckCloner.Clone(
                    source.Deck)
            };

        player.Deck.InitializeRuntimeStorage();

        return player;
    }

    private static void CopyPlayerState(
        Player source,
        Player destination)
    {
        destination.Name =
            source.Name;

        destination.Energy =
            source.Energy;

        destination.MaxEnergy =
            source.MaxEnergy;

        destination.Shield =
            source.Shield;

        destination.Wins =
            source.Wins;

        destination.Losses =
            source.Losses;

        destination.Deck =
            DeckCloner.Clone(
                source.Deck);

        destination.Deck.InitializeRuntimeStorage();

        HandManager.ClearHand(
            destination);

        destination.DiscardPile.Clear();

        destination.ActiveCharacter = null;
        destination.CurrentCharacterHp = 0;

        destination.AttackSlot1 = null;
        destination.AttackSlot2 = null;

        destination.IsRestingSlot1 = false;
        destination.IsRestingSlot2 = false;

        destination.IsFatiguedSlot1 = false;
        destination.IsFatiguedSlot2 = false;

        destination.AttackSlot1UsedThisPhase = false;
        destination.AttackSlot2UsedThisPhase = false;

        destination.AttackSlot1UsesRemaining = 2;
        destination.AttackSlot2UsesRemaining = 2;

        destination.AttackSlot1TotalUses = 0;
        destination.AttackSlot2TotalUses = 0;

        destination.CharacterNewlyDeployed = false;
        destination.EntryDefenseUsed = false;
        destination.CharacterDefeatedSinceLastPlacement = false;

        destination.TemporaryAttackBonus = 0;
        destination.DiscountActive = false;

        destination.EnergyDrainedThisPhaseCount = 0;
        destination.ReorderMovesThisPhase = 0;
        destination.IsReorderingNodes = false;

        destination.AbilityUsedThisPhase = false;
        destination.UtilityActionsRemaining = 1;

        destination.StatusEffects =
            new List<string>();

        destination.HitDuringPreviousTurn = false;
        destination.WasHitThisTurn = false;
        destination.HasSurrendered = false;

        destination.AbilityCooldownRemaining = 0;
    }

    private static Card? FindCard(
        Player player,
        string cardName)
    {
        Card? handCard =
            player.HandSlots
                .Select(slot => slot.CardInSlot)
                .FirstOrDefault(
                    card =>
                        card != null &&
                        string.Equals(
                            card.Name,
                            cardName,
                            StringComparison.OrdinalIgnoreCase));

        if (handCard != null)
        {
            return handCard;
        }

        Card? deckCard =
            player.Deck.RuntimeStorage?
                .GetCards()
                .FirstOrDefault(
                    card =>
                        string.Equals(
                            card.Name,
                            cardName,
                            StringComparison.OrdinalIgnoreCase));

        if (deckCard != null)
        {
            return deckCard;
        }

        return player.DiscardPile
            .FirstOrDefault(
                card =>
                    string.Equals(
                        card.Name,
                        cardName,
                        StringComparison.OrdinalIgnoreCase));
    }

    private static Card? RemoveCardFromHand(
        Player player,
        string cardName)
    {
        HandSlot? slot =
            player.HandSlots
                .FirstOrDefault(
                    value =>
                        value.CardInSlot != null &&
                        string.Equals(
                            value.CardInSlot.Name,
                            cardName,
                            StringComparison.OrdinalIgnoreCase));

        if (slot == null)
        {
            return null;
        }

        Card card =
            slot.CardInSlot!;

        slot.CardInSlot = null;

        return card;
    }

    private static void RemoveCardFromDeck(
        Player player,
        Card card)
    {
        List<Card> remaining =
            player.Deck.RuntimeStorage?
                .GetCards()
                .ToList()
            ?? new List<Card>();

        int index =
            remaining.FindIndex(
                value =>
                    string.Equals(
                        value.Id,
                        card.Id,
                        StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(
                        value.Name,
                        card.Name,
                        StringComparison.OrdinalIgnoreCase));

        if (index < 0)
        {
            return;
        }

        remaining.RemoveAt(index);

        Deck rebuiltDeck =
            new Deck
            {
                Name =
                    player.Deck.Name,

                Structure =
                    player.Deck.Structure,

                Cards =
                    remaining
                        .Select(value => value.Clone())
                        .ToList()
            };

        rebuiltDeck.InitializeRuntimeStorage();

        player.Deck =
            rebuiltDeck;
    }
}