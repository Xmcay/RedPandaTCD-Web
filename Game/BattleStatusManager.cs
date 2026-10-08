namespace RedPandaTCD_Web.Game;
public enum BattleStatusTone
{
    Neutral,
    Positive,
    Utility,
    Attack,
    Energy,
    Interactable,
    Warning,
    Danger
}
public sealed class BattleStatus
{
    public string Key { get; }
    public string Label { get; }
    public string? Detail { get; }
    public BattleStatusTone Tone { get; }
    public string CssClass =>
        $"status-{Tone.ToString().ToLowerInvariant()}";
    public bool HasDetail =>
        !string.IsNullOrWhiteSpace(Detail);
    public BattleStatus(
        string key,
        string label,
        BattleStatusTone tone,
        string? detail = null)
    {
        Key = key;
        Label = label;
        Tone = tone;
        Detail = detail;
    }
}
public static class BattleStatusManager
{
    public static IReadOnlyList<BattleStatus>
        GetPlayerStatuses(Player player)
    {
        List<BattleStatus> statuses = new();
        if (player.DiscountActive)
        {
            statuses.Add(
                new BattleStatus(
                    "discount",
                    "Discount",
                    BattleStatusTone.Positive,
                    "The next Attack placement costs 1 less Energy."));
        }
        if (player.TemporaryAttackBonus > 0)
        {
            statuses.Add(
                new BattleStatus(
                    "temporary-attack-bonus",
                    $"+{player.TemporaryAttackBonus} Attack",
                    BattleStatusTone.Attack,
                    "Temporary Attack damage bonus."));
        }
        if (player.UtilityActionsRemaining > 0)
        {
            statuses.Add(
                new BattleStatus(
                    "utility-actions",
                    player.UtilityActionsRemaining == 1
                        ? "1 Utility Action"
                        : $"{player.UtilityActionsRemaining} Utility Actions",
                    BattleStatusTone.Utility,
                    "Utility actions remaining during this phase."));
        }
        if (player.IsReorderingNodes)
        {
            statuses.Add(
                new BattleStatus(
                    "reordering-nodes",
                    "Reordering",
                    BattleStatusTone.Interactable,
                    player.ReorderMovesThisPhase == 1
                        ? "1 node move completed."
                        : $"{player.ReorderMovesThisPhase} node moves completed."));
        }
        if (player.EnergyDrainedThisPhaseCount > 0)
        {
            statuses.Add(
                new BattleStatus(
                    "energy-drained",
                    $"Energy Drained ×{player.EnergyDrainedThisPhaseCount}",
                    BattleStatusTone.Warning,
                    "Energy was drained during the current phase."));
        }
        if (player.HitDuringPreviousTurn)
        {
            statuses.Add(
                new BattleStatus(
                    "hit-previous-turn",
                    "Hit Last Turn",
                    BattleStatusTone.Warning,
                    "This may activate effects that react to being hit."));
        }
        AddStoredStatusEffects(
            statuses,
            player.StatusEffects);
        return statuses;
    }
    public static IReadOnlyList<BattleStatus>
        GetCharacterStatuses(Player owner)
    {
        List<BattleStatus> statuses = new();
        Card? character =
            owner.ActiveCharacter;
        if (character == null)
        {
            return statuses;
        }
        AddCharacterEntryStatus(
            statuses,
            owner,
            character);
        AddCharacterPassiveStatuses(
            statuses,
            owner,
            character);
        AddCharacterAbilityStatus(
            statuses,
            owner,
            character);
        if (owner.CharacterDefeatedSinceLastPlacement)
        {
            statuses.Add(
                new BattleStatus(
                    "free-character-replacement",
                    "Free Replacement",
                    BattleStatusTone.Interactable,
                    "The defeated Character can be replaced for free."));
        }
        AddStoredStatusEffects(
            statuses,
            owner.StatusEffects);
        return RemoveDuplicateStatuses(
            statuses);
    }
    private static void AddCharacterEntryStatus(
        List<BattleStatus> statuses,
        Player owner,
        Card character)
    {
        if (!owner.CharacterNewlyDeployed)
        {
            return;
        }
        switch (character.Name)
        {
            case "Sage Red Panda":
                statuses.Add(
                    new BattleStatus(
                        "damage-floor",
                        "Damage Floor",
                        BattleStatusTone.Positive,
                        owner.EntryDefenseUsed
                            ? "The Entry protection has already been consumed."
                            : "Incoming damage is limited by the Entry effect."));
                break;
            case "Sorcerer Red Panda":
                statuses.Add(
                    new BattleStatus(
                        "magic-surge",
                        "+1 Magic",
                        BattleStatusTone.Attack,
                        "Magic Attacks gain 1 damage during the deployment turn."));
                break;
            case "Guardian Red Panda":
                statuses.Add(
                    new BattleStatus(
                        "fortified",
                        "Fortified",
                        BattleStatusTone.Positive,
                        owner.EntryDefenseUsed
                            ? "The Entry protection has already been consumed."
                            : "The Character is protected by its Entry effect."));
                break;
            case "Berserker Red Panda":
                statuses.Add(
                    new BattleStatus(
                        "physical-surge",
                        "+1 Physical",
                        BattleStatusTone.Attack,
                        "Physical Attacks gain 1 damage during the deployment turn."));
                break;
            case "Nomad Red Panda":
                statuses.Add(
                    new BattleStatus(
                        "reduced-buildup",
                        "Reduced Buildup",
                        BattleStatusTone.Positive,
                        "Both deployed Attack buildup costs were reduced by 1."));
                break;
            case "Wanderer Red Panda":
                statuses.Add(
                    new BattleStatus(
                        "wandering-path",
                        "Wandering Path",
                        BattleStatusTone.Interactable,
                        "A deployed Attack can receive reduced buildup."));
                break;
        }
    }
    private static void AddCharacterPassiveStatuses(
        List<BattleStatus> statuses,
        Player owner,
        Card character)
    {
        if (CharacterManager.HasPassive(
                owner,
                "Retaliation") &&
            owner.HitDuringPreviousTurn)
        {
            statuses.Add(
                new BattleStatus(
                    "retaliation",
                    "+1 Attack",
                    BattleStatusTone.Attack,
                    "Retaliation is active because the player was hit last turn."));
        }
        if (CharacterManager.HasPassive(
                owner,
                "ArcaneFocus") &&
            !HasPhysicalAttackDeployed(owner))
        {
            statuses.Add(
                new BattleStatus(
                    "arcane-focus",
                    "Arcane Focus",
                    BattleStatusTone.Attack,
                    "Magic Attacks gain 1 damage while no Physical Attack is deployed."));
        }
        if (CharacterManager.HasPassive(
                owner,
                "FreePeek"))
        {
            statuses.Add(
                new BattleStatus(
                    "free-peek",
                    "Free Peek",
                    BattleStatusTone.Utility,
                    "The relevant deck-view action does not spend a Utility action."));
        }
        if (CharacterManager.HasPassive(
                owner,
                "EndTurnShield"))
        {
            statuses.Add(
                new BattleStatus(
                    "end-turn-shield",
                    "Shield Recovery",
                    BattleStatusTone.Positive,
                    "This Character grants additional Shield during End."));
        }
        foreach (string passive in
            character.PassiveAbilities)
        {
            if (string.IsNullOrWhiteSpace(passive))
            {
                continue;
            }
            if (IsKnownDisplayedPassive(passive))
            {
                continue;
            }
            statuses.Add(
                new BattleStatus(
                    $"passive-{NormalizeKey(passive)}",
                    FormatIdentifier(passive),
                    BattleStatusTone.Positive,
                    "Character passive effect."));
        }
    }
    private static void AddCharacterAbilityStatus(
        List<BattleStatus> statuses,
        Player owner,
        Card character)
    {
        if (string.IsNullOrWhiteSpace(
                character.ActiveAbility))
        {
            return;
        }
        if (owner.CharacterNewlyDeployed)
        {
            statuses.Add(
                new BattleStatus(
                    "ability-newly-deployed",
                    "Ability Locked",
                    BattleStatusTone.Warning,
                    "A newly deployed Character cannot use its Active ability this phase."));
            return;
        }
        if (owner.AbilityCooldownRemaining > 0)
        {
            statuses.Add(
                new BattleStatus(
                    "ability-cooldown",
                    $"Cooldown {owner.AbilityCooldownRemaining}",
                    BattleStatusTone.Warning,
                    owner.AbilityCooldownRemaining == 1
                        ? "The Active ability has 1 turn of cooldown remaining."
                        : $"The Active ability has {owner.AbilityCooldownRemaining} turns of cooldown remaining."));
            return;
        }
        if (owner.AbilityUsedThisPhase)
        {
            statuses.Add(
                new BattleStatus(
                    "ability-used",
                    "Ability Used",
                    BattleStatusTone.Neutral,
                    "The Active ability has already been used this phase."));
            return;
        }
        statuses.Add(
            new BattleStatus(
                "ability-ready",
                "Ability Ready",
                BattleStatusTone.Positive,
                "The Active ability is ready to use."));
    }
    public static IReadOnlyList<BattleStatus>
        GetAttackStatuses(
            Player owner,
            int slot)
    {
        List<BattleStatus> statuses = new();
        if (!IsValidAttackSlot(slot))
        {
            return statuses;
        }
        Card? attack =
            GetAttack(
                owner,
                slot);
        if (attack == null)
        {
            return statuses;
        }
        bool fatigued =
            IsFatigued(
                owner,
                slot);
        bool resting =
            IsResting(
                owner,
                slot);
        bool usedThisPhase =
            WasUsedThisPhase(
                owner,
                slot);
        int usesRemaining =
            GetUsesRemaining(
                owner,
                slot);
        int nextUseCost =
            GetNextUseCost(
                owner,
                slot);
        if (resting)
        {
            statuses.Add(
                new BattleStatus(
                    "resting",
                    "Resting",
                    BattleStatusTone.Positive,
                    "This Attack is recovering and will return with refreshed uses."));
        }
        else if (fatigued)
        {
            statuses.Add(
                new BattleStatus(
                    "fatigued",
                    "Fatigued",
                    BattleStatusTone.Danger,
                    "This Attack must be Rested or replaced during Placement."));
        }
        else
        {
            statuses.Add(
                new BattleStatus(
                    "uses-remaining",
                    usesRemaining == 1
                        ? "1 Use"
                        : $"{usesRemaining} Uses",
                    usesRemaining <= 1
                        ? BattleStatusTone.Warning
                        : BattleStatusTone.Neutral,
                    "Uses remaining before this Attack becomes Fatigued."));
        }
        if (usedThisPhase)
        {
            statuses.Add(
                new BattleStatus(
                    "used-this-phase",
                    "Used This Phase",
                    BattleStatusTone.Neutral,
                    "This Attack has already been used during the current Attack phase."));
        }
        if (!fatigued &&
            !resting)
        {
            statuses.Add(
                new BattleStatus(
                    "next-use-cost",
                    $"{nextUseCost} Energy",
                    owner.Energy >= nextUseCost
                        ? BattleStatusTone.Energy
                        : BattleStatusTone.Danger,
                    owner.Energy >= nextUseCost
                        ? "Energy required for the next use."
                        : "The player does not currently have enough Energy."));
        }
        if (attack.IsPiercing)
        {
            statuses.Add(
                new BattleStatus(
                    "piercing",
                    "Piercing",
                    BattleStatusTone.Attack,
                    "This Attack uses the game's Piercing damage rules."));
        }
        if (attack.Hits > 1)
        {
            statuses.Add(
                new BattleStatus(
                    "multi-hit",
                    $"{attack.Hits} Hits",
                    BattleStatusTone.Attack,
                    "This Attack resolves multiple individual hits."));
        }
        if (attack.InstantFatigue)
        {
            statuses.Add(
                new BattleStatus(
                    "instant-fatigue",
                    "Instant Fatigue",
                    BattleStatusTone.Danger,
                    "Using this Attack immediately makes it Fatigued."));
        }
        int effectiveDamage = AttackManager.GetEffectiveDamage(owner, attack);
        int liveDamageBonus = effectiveDamage - attack.Damage;

        if (liveDamageBonus != 0)
        {
            string sign = liveDamageBonus > 0 ? "+" : "";
            statuses.Add(
                new BattleStatus(
                    "temporary-attack-bonus",
                    $"{sign}{liveDamageBonus} Damage",
                    liveDamageBonus > 0 ? BattleStatusTone.Attack : BattleStatusTone.Warning,
                    "Live modifier already included in this Attack's displayed Damage."));
        }
        return RemoveDuplicateStatuses(
            statuses);
    }
    public static bool IsAttackAvailable(
        Player owner,
        int slot)
    {
        if (!IsValidAttackSlot(slot))
        {
            return false;
        }
        Card? attack =
            GetAttack(
                owner,
                slot);
        if (attack == null)
        {
            return false;
        }
        return
            !IsFatigued(owner, slot) &&
            !IsResting(owner, slot) &&
            !WasUsedThisPhase(owner, slot) &&
            owner.Energy >=
                GetNextUseCost(owner, slot);
    }
    public static bool IsCharacterAbilityAvailable(
        Player owner)
    {
        return
            owner.ActiveCharacter != null &&
            !string.IsNullOrWhiteSpace(
                owner.ActiveCharacter.ActiveAbility) &&
            !owner.CharacterNewlyDeployed &&
            !owner.AbilityUsedThisPhase &&
            owner.AbilityCooldownRemaining <= 0;
    }
    public static string GetToneCssClass(
        BattleStatusTone tone)
    {
        return
            $"status-{tone.ToString().ToLowerInvariant()}";
    }
    public static string GetToneCssClass(
        BattleStatus status)
    {
        return status.CssClass;
    }
    private static void AddStoredStatusEffects(
        List<BattleStatus> statuses,
        IEnumerable<string> effects)
    {
        foreach (string effect in effects)
        {
            if (string.IsNullOrWhiteSpace(effect))
            {
                continue;
            }
            string label =
                effect == "DeflectNextHit"
                    ? "Deflect"
                    : FormatIdentifier(effect);
            statuses.Add(
                new BattleStatus(
                    $"stored-{NormalizeKey(effect)}",
                    label,
                    GetStatusTone(effect),
                    GetStoredStatusDetail(effect)));
        }
    }
    private static IReadOnlyList<BattleStatus>
        RemoveDuplicateStatuses(
            IEnumerable<BattleStatus> statuses)
    {
        return statuses
            .GroupBy(status =>
                status.Key,
                StringComparer.OrdinalIgnoreCase)
            .Select(group =>
                group.First())
            .ToList();
    }
    private static bool IsValidAttackSlot(
        int slot)
    {
        return slot == 1 ||
               slot == 2;
    }
    private static Card? GetAttack(
        Player owner,
        int slot)
    {
        return slot switch
        {
            1 => owner.AttackSlot1,
            2 => owner.AttackSlot2,
            _ => null
        };
    }
    private static bool IsFatigued(
        Player owner,
        int slot)
    {
        return slot switch
        {
            1 => owner.IsFatiguedSlot1,
            2 => owner.IsFatiguedSlot2,
            _ => false
        };
    }
    private static bool IsResting(
        Player owner,
        int slot)
    {
        return slot switch
        {
            1 => owner.IsRestingSlot1,
            2 => owner.IsRestingSlot2,
            _ => false
        };
    }
    private static bool WasUsedThisPhase(
        Player owner,
        int slot)
    {
        return slot switch
        {
            1 =>
                owner.AttackSlot1UsedThisPhase,
            2 =>
                owner.AttackSlot2UsedThisPhase,
            _ =>
                false
        };
    }
    private static int GetUsesRemaining(
        Player owner,
        int slot)
    {
        return slot switch
        {
            1 =>
                owner.AttackSlot1UsesRemaining,
            2 =>
                owner.AttackSlot2UsesRemaining,
            _ =>
                0
        };
    }
    private static int GetNextUseCost(
        Player owner,
        int slot)
    {
        return slot switch
        {
            1 =>
                owner.AttackSlot1TotalUses,
            2 =>
                owner.AttackSlot2TotalUses,
            _ =>
                0
        };
    }
    private static bool HasPhysicalAttackDeployed(
        Player owner)
    {
        return
            owner.AttackSlot1?.Archetype ==
                Archetype.Physical ||
            owner.AttackSlot2?.Archetype ==
                Archetype.Physical;
    }
    private static bool IsKnownDisplayedPassive(
        string passive)
    {
        return passive is
            "Retaliation" or
            "ArcaneFocus" or
            "FreePeek" or
            "EndTurnShield";
    }
    private static BattleStatusTone GetStatusTone(
        string effect)
    {
        string normalized =
            effect
                .Trim()
                .ToLowerInvariant();
        if (normalized.Contains("fatigue") ||
            normalized.Contains("defeated") ||
            normalized.Contains("danger"))
        {
            return BattleStatusTone.Danger;
        }
        if (normalized.Contains("energy") ||
            normalized.Contains("drain"))
        {
            return BattleStatusTone.Energy;
        }
        if (normalized.Contains("attack") ||
            normalized.Contains("piercing") ||
            normalized.Contains("damage"))
        {
            return BattleStatusTone.Attack;
        }
        if (normalized.Contains("utility") ||
            normalized.Contains("peek") ||
            normalized.Contains("draw") ||
            normalized.Contains("reorder"))
        {
            return BattleStatusTone.Utility;
        }
        if (normalized.Contains("shield") ||
            normalized.Contains("heal") ||
            normalized.Contains("boost") ||
            normalized.Contains("discount") ||
            normalized.Contains("deflect"))
        {
            return BattleStatusTone.Positive;
        }
        return BattleStatusTone.Neutral;
    }
    private static string? GetStoredStatusDetail(
        string effect)
    {
        return effect switch
        {
            "DeflectNextHit" =>
                "The next eligible incoming hit is deflected.",
            _ =>
                null
        };
    }
    private static string NormalizeKey(
        string value)
    {
        return string.Join(
            "-",
            SplitIdentifier(value)
                .Select(part =>
                    part.ToLowerInvariant()));
    }
    private static string FormatIdentifier(
        string value)
    {
        return string.Join(
            " ",
            SplitIdentifier(value));
    }
    private static IReadOnlyList<string>
        SplitIdentifier(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Array.Empty<string>();
        }
        string separated =
            System.Text.RegularExpressions.Regex.Replace(
                value.Trim(),
                "([a-z0-9])([A-Z])",
                "$1 $2");
        return separated
            .Replace(
                "_",
                " ")
            .Replace(
                "-",
                " ")
            .Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);
    }
}
