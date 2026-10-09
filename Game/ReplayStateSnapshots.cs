namespace RedPandaTCD_Web.Game;

/// <summary>
/// Immutable, decision-free snapshot of the match after a recorded event.
/// Replay playback restores this data; it never executes gameplay rules.
/// </summary>
public sealed class ReplayMatchSnapshot
{
    public int TurnNumber { get; set; }
    public MatchPhase CurrentPhase { get; set; }
    public int ActingPlayerIndex { get; set; }
    public bool PlayerOneStarts { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDraw { get; set; }
    public ReplayPlayerSnapshot Player1 { get; set; } = new();
    public ReplayPlayerSnapshot Player2 { get; set; } = new();
}

public sealed class ReplayPlayerSnapshot
{
    public string Name { get; set; } = "";
    public int Energy { get; set; }
    public int MaxEnergy { get; set; }
    public int Shield { get; set; }
    public int Wins { get; set; }
    public int Losses { get; set; }
    public bool IsBot { get; set; }
    public string DeckName { get; set; } = "";
    public DeckStructure DeckStructure { get; set; }
    public List<Card> DeckBlueprint { get; set; } = new();
    public List<Card> DeckRuntimeCards { get; set; } = new();
    public List<Card?> HandCards { get; set; } = new();
    public List<Card> DiscardPile { get; set; } = new();
    public Card? ActiveCharacter { get; set; }
    public int CurrentCharacterHp { get; set; }
    public Card? AttackSlot1 { get; set; }
    public Card? AttackSlot2 { get; set; }
    public bool IsRestingSlot1 { get; set; }
    public bool IsRestingSlot2 { get; set; }
    public bool IsFatiguedSlot1 { get; set; }
    public bool IsFatiguedSlot2 { get; set; }
    public bool AttackSlot1UsedThisPhase { get; set; }
    public bool AttackSlot2UsedThisPhase { get; set; }
    public int AttackSlot1UsesRemaining { get; set; }
    public int AttackSlot2UsesRemaining { get; set; }
    public int AttackSlot1TotalUses { get; set; }
    public int AttackSlot2TotalUses { get; set; }
    public bool CharacterNewlyDeployed { get; set; }
    public bool EntryDefenseUsed { get; set; }
    public bool CharacterDefeatedSinceLastPlacement { get; set; }
    public int TemporaryAttackBonus { get; set; }
    public bool DiscountActive { get; set; }
    public int EnergyDrainedThisPhaseCount { get; set; }
    public int ReorderMovesThisPhase { get; set; }
    public bool IsReorderingNodes { get; set; }
    public bool AbilityUsedThisPhase { get; set; }
    public int UtilityActionsRemaining { get; set; }
    public List<string> StatusEffects { get; set; } = new();
    public bool HitDuringPreviousTurn { get; set; }
    public bool WasHitThisTurn { get; set; }
    public bool HasSurrendered { get; set; }
    public int AbilityCooldownRemaining { get; set; }

    public static ReplayPlayerSnapshot Capture(Player player)
    {
        return new ReplayPlayerSnapshot
        {
            Name = player.Name,
            Energy = player.Energy,
            MaxEnergy = player.MaxEnergy,
            Shield = player.Shield,
            Wins = player.Wins,
            Losses = player.Losses,
            IsBot = player.IsBot,
            DeckName = player.Deck.Name,
            DeckStructure = player.Deck.Structure,
            DeckBlueprint = player.Deck.Cards.Select(card => card.Clone()).ToList(),
            DeckRuntimeCards = player.Deck.RuntimeStorage?.GetCards().Select(card => card.Clone()).ToList() ?? new(),
            HandCards = player.HandSlots.Select(slot => slot.CardInSlot?.Clone()).ToList(),
            DiscardPile = player.DiscardPile.Select(card => card.Clone()).ToList(),
            ActiveCharacter = player.ActiveCharacter?.Clone(),
            CurrentCharacterHp = player.CurrentCharacterHp,
            AttackSlot1 = player.AttackSlot1?.Clone(),
            AttackSlot2 = player.AttackSlot2?.Clone(),
            IsRestingSlot1 = player.IsRestingSlot1,
            IsRestingSlot2 = player.IsRestingSlot2,
            IsFatiguedSlot1 = player.IsFatiguedSlot1,
            IsFatiguedSlot2 = player.IsFatiguedSlot2,
            AttackSlot1UsedThisPhase = player.AttackSlot1UsedThisPhase,
            AttackSlot2UsedThisPhase = player.AttackSlot2UsedThisPhase,
            AttackSlot1UsesRemaining = player.AttackSlot1UsesRemaining,
            AttackSlot2UsesRemaining = player.AttackSlot2UsesRemaining,
            AttackSlot1TotalUses = player.AttackSlot1TotalUses,
            AttackSlot2TotalUses = player.AttackSlot2TotalUses,
            CharacterNewlyDeployed = player.CharacterNewlyDeployed,
            EntryDefenseUsed = player.EntryDefenseUsed,
            CharacterDefeatedSinceLastPlacement = player.CharacterDefeatedSinceLastPlacement,
            TemporaryAttackBonus = player.TemporaryAttackBonus,
            DiscountActive = player.DiscountActive,
            EnergyDrainedThisPhaseCount = player.EnergyDrainedThisPhaseCount,
            ReorderMovesThisPhase = player.ReorderMovesThisPhase,
            IsReorderingNodes = player.IsReorderingNodes,
            AbilityUsedThisPhase = player.AbilityUsedThisPhase,
            UtilityActionsRemaining = player.UtilityActionsRemaining,
            StatusEffects = player.StatusEffects.ToList(),
            HitDuringPreviousTurn = player.HitDuringPreviousTurn,
            WasHitThisTurn = player.WasHitThisTurn,
            HasSurrendered = player.HasSurrendered,
            AbilityCooldownRemaining = player.AbilityCooldownRemaining
        };
    }

    public void Restore(Player player)
    {
        player.Name = Name;
        player.Energy = Energy;
        player.MaxEnergy = MaxEnergy;
        player.Shield = Shield;
        player.Wins = Wins;
        player.Losses = Losses;
        player.IsBot = IsBot;

        player.Deck = new Deck
        {
            Name = DeckName,
            Structure = DeckStructure,
            Cards = DeckBlueprint.Select(card => card.Clone()).ToList()
        };
        player.Deck.SetReplayRuntimeCards(DeckRuntimeCards.Select(card => card.Clone()));

        for (int i = 0; i < Player.MAX_HAND_SIZE; i++)
        {
            player.HandSlots[i].CardInSlot =
                i < HandCards.Count ? HandCards[i]?.Clone() : null;
        }

        player.DiscardPile = DiscardPile.Select(card => card.Clone()).ToList();
        player.ActiveCharacter = ActiveCharacter?.Clone();
        player.CurrentCharacterHp = CurrentCharacterHp;
        player.AttackSlot1 = AttackSlot1?.Clone();
        player.AttackSlot2 = AttackSlot2?.Clone();
        player.IsRestingSlot1 = IsRestingSlot1;
        player.IsRestingSlot2 = IsRestingSlot2;
        player.IsFatiguedSlot1 = IsFatiguedSlot1;
        player.IsFatiguedSlot2 = IsFatiguedSlot2;
        player.AttackSlot1UsedThisPhase = AttackSlot1UsedThisPhase;
        player.AttackSlot2UsedThisPhase = AttackSlot2UsedThisPhase;
        player.AttackSlot1UsesRemaining = AttackSlot1UsesRemaining;
        player.AttackSlot2UsesRemaining = AttackSlot2UsesRemaining;
        player.AttackSlot1TotalUses = AttackSlot1TotalUses;
        player.AttackSlot2TotalUses = AttackSlot2TotalUses;
        player.CharacterNewlyDeployed = CharacterNewlyDeployed;
        player.EntryDefenseUsed = EntryDefenseUsed;
        player.CharacterDefeatedSinceLastPlacement = CharacterDefeatedSinceLastPlacement;
        player.TemporaryAttackBonus = TemporaryAttackBonus;
        player.DiscountActive = DiscountActive;
        player.EnergyDrainedThisPhaseCount = EnergyDrainedThisPhaseCount;
        player.ReorderMovesThisPhase = ReorderMovesThisPhase;
        player.IsReorderingNodes = IsReorderingNodes;
        player.AbilityUsedThisPhase = AbilityUsedThisPhase;
        player.UtilityActionsRemaining = UtilityActionsRemaining;
        player.StatusEffects = StatusEffects.ToList();
        player.HitDuringPreviousTurn = HitDuringPreviousTurn;
        player.WasHitThisTurn = WasHitThisTurn;
        player.HasSurrendered = HasSurrendered;
        player.AbilityCooldownRemaining = AbilityCooldownRemaining;
    }
}
