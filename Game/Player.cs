namespace RedPandaTCD_Web.Game;

public class Player
{
    public const int MAX_HAND_SIZE = 5;

    public string Name { get; set; } = "";

    public int Energy { get; set; } = 20;

    public int MaxEnergy { get; set; } = 25;

    public int Shield { get; set; }

    public int Wins { get; set; }

    public int Losses { get; set; }

    public Deck Deck { get; set; } =
        new Deck();

    public HandSlot[] HandSlots { get; set; } =
        new HandSlot[MAX_HAND_SIZE];

    public List<Card> DiscardPile { get; set; } =
        new List<Card>();

    public Card? ActiveCharacter { get; set; }

    public int CurrentCharacterHp { get; set; }

    public Card? AttackSlot1 { get; set; }

    public Card? AttackSlot2 { get; set; }

    public bool IsRestingSlot1 { get; set; }

    public bool IsRestingSlot2 { get; set; }

    public bool IsFatiguedSlot1 { get; set; }

    public bool AttackSlot1UsedThisPhase { get; set; }
    
public bool AttackSlot2UsedThisPhase { get; set; }

    public bool IsFatiguedSlot2 { get; set; }

    public int AttackSlot1UsesRemaining { get; set; } = 2;

    public int AttackSlot2UsesRemaining { get; set; } = 2;

    public int AttackSlot1TotalUses { get; set; } = 0;

    public int AttackSlot2TotalUses { get; set; } = 0;

    public bool CharacterNewlyDeployed { get; set; }

    public bool EntryDefenseUsed { get; set; }

    public bool CharacterDefeatedSinceLastPlacement { get; set; }

    public int TemporaryAttackBonus { get; set; }

    public bool DiscountActive { get; set; }

    public int EnergyDrainedThisPhaseCount { get; set; }

    public int ReorderMovesThisPhase { get; set; }

    public bool IsReorderingNodes { get; set; }

    public bool AbilityUsedThisPhase { get; set; }

    public int UtilityActionsRemaining { get; set; } = 1;

    public List<string> StatusEffects { get; set; } =
        new List<string>();

    public bool HitDuringPreviousTurn { get; set; }

    public bool WasHitThisTurn { get; set; }

    public bool HasSurrendered { get; set; }

    public bool IsBot { get; set; }

    public int AbilityCooldownRemaining { get; set; }

    public Player()
    {
        for(int i = 0; i < MAX_HAND_SIZE; i++)
        {
            HandSlots[i] =
                new HandSlot(i + 1);
        }
    }
}