namespace RedPandaTCD_Web.Game;

public static class CharacterManager
{
    public static void ApplyEntryEffect(Player player)
    {
        if (player.ActiveCharacter == null)
            return;

        player.EntryDefenseUsed = false;

        switch (player.ActiveCharacter.Name)
        {
            case "Sage Red Panda":
                BattleLog.Write(
                    $"{player.Name} gained Damage Floor this turn");
                break;

            case "Sorcerer Red Panda":
                BattleLog.Write(
                    $"{player.Name}'s Magic attacks gain +1 Damage this turn");
                break;

            case "Guardian Red Panda":
                BattleLog.Write(
                    $"{player.Name} became Fortified this turn");
                break;

            case "Berserker Red Panda":
                BattleLog.Write(
                    $"{player.Name}'s Physical attacks gain +1 Damage this turn");
                break;

            case "Nomad Red Panda":
                ReduceAttackBuildup(
                    player,
                    1,
                    1);

                ReduceAttackBuildup(
                    player,
                    2,
                    1);

                BattleLog.Write(
                    $"{player.Name} reduced both Attack buildup costs by 1");
                break;

            case "Wanderer Red Panda":
                if (player.IsBot)
                {
                    int slotToReduce = 0;

                    if (player.AttackSlot1 != null &&
                        player.AttackSlot2 != null)
                    {
                        slotToReduce =
                            player.AttackSlot1TotalUses >=
                            player.AttackSlot2TotalUses
                            ? 1
                            : 2;
                    }
                    else if (player.AttackSlot1 != null)
                    {
                        slotToReduce = 1;
                    }
                    else if (player.AttackSlot2 != null)
                    {
                        slotToReduce = 2;
                    }

                    if (slotToReduce != 0)
                    {
                        ReduceAttackBuildup(
                            player,
                            slotToReduce,
                            2);
                    }
                }
                break;
        }
    }

    public static void ReduceAttackBuildup(
        Player player,
        int slotNumber,
        int amount)
    {
        if (slotNumber == 1)
        {
            if (player.AttackSlot1 == null)
                return;

            player.AttackSlot1TotalUses =
                Math.Max(
                    0,
                    player.AttackSlot1TotalUses - amount);
        }
        else if (slotNumber == 2)
        {
            if (player.AttackSlot2 == null)
                return;

            player.AttackSlot2TotalUses =
                Math.Max(
                    0,
                    player.AttackSlot2TotalUses - amount);
        }
    }

    // Active Abilities
    public static bool UseAbility(Player player)
    {
        if (player.ActiveCharacter == null)
            return false;

        // This manager is the single authority for ability legality,
        // regardless of whether a human or bot requested the ability.
        if (player.CharacterNewlyDeployed ||
            player.AbilityUsedThisPhase ||
            player.AbilityCooldownRemaining > 0)
            return false;

        bool usedAbility = false;
        switch (player.ActiveCharacter.ActiveAbility)
        {
            case "ShieldGain":
                if (player.Energy >= 2)
                {
                    player.Energy -= 2;
                    player.Shield++;

                    BattleLog.Write(
                        $"{player.Name}'s {player.ActiveCharacter.Name} gained +1 Shield " +
                        $"({player.Shield} Total)");

                    player.AbilityCooldownRemaining =
                        player.ActiveCharacter.AbilityCooldown;

                    player.AbilityUsedThisPhase = true;
                    usedAbility = true;
                    return usedAbility;
                }
                break;

            case "MagicBoost":
                if (player.Energy >= 1)
                {
                    player.Energy--;
                    player.TemporaryAttackBonus++;

                    BattleLog.Write(
                        $"{player.Name}'s {player.ActiveCharacter.Name} gained +1 Attack Damage");

                    player.AbilityCooldownRemaining =
                        player.ActiveCharacter.AbilityCooldown;

                    player.AbilityUsedThisPhase = true;
                    usedAbility = true;
                    return usedAbility;
                }
                break;

            case "FieldShield":
                if (player.Energy >= 3)
                {
                    player.Energy -= 3;

                    int attacks =
                        (player.AttackSlot1 != null ? 1 : 0) +
                        (player.AttackSlot2 != null ? 1 : 0);

                    player.Shield += attacks;

                    BattleLog.Write(
                        $"{player.Name}'s {player.ActiveCharacter.Name} gained +{attacks} Shield " +
                        $"({player.Shield} Total)");

                    player.AbilityCooldownRemaining =
                        player.ActiveCharacter.AbilityCooldown;

                    player.AbilityUsedThisPhase = true;
                    usedAbility = true;
                    return usedAbility;
                }
                break;

            case "PowerSurge":
                if (player.Energy >= 3)
                {
                    player.Energy -= 3;
                    player.TemporaryAttackBonus += 2;

                    BattleLog.Write(
                        $"{player.Name}'s {player.ActiveCharacter.Name} gained +2 Attack Damage");

                    player.AbilityCooldownRemaining =
                        player.ActiveCharacter.AbilityCooldown;

                    player.AbilityUsedThisPhase = true;
                    usedAbility = true;
                    return usedAbility;
                }
                break;

            case "AttackBoost":
                if (player.Energy >= 2)
                {
                    player.Energy -= 2;
                    player.TemporaryAttackBonus++;

                    BattleLog.Write(
                        $"{player.Name}'s {player.ActiveCharacter.Name} gained +1 Attack Damage");

                    player.AbilityCooldownRemaining =
                        player.ActiveCharacter.AbilityCooldown;

                    player.AbilityUsedThisPhase = true;
                    usedAbility = true;
                    return usedAbility;
                }
                break;

            case "Heal":
                if (player.Energy >= 2)
                {
                    player.Energy -= 2;

                    int healedTo =
                        Math.Min(
                            player.ActiveCharacter.HpValue,
                            player.CurrentCharacterHp + 1);

                    player.CurrentCharacterHp = healedTo;

                    BattleLog.Write(
                        $"{player.Name}'s {player.ActiveCharacter.Name} healed 1 HP ({healedTo} HP)");

                    player.AbilityCooldownRemaining =
                        player.ActiveCharacter.AbilityCooldown;

                    player.AbilityUsedThisPhase = true;
                    usedAbility = true;
                    return usedAbility;
                }
                break;
        }

        return false;
    }

    // Passives
    public static bool HasPassive(
        Player player,
        string passive)
    {
        return player.ActiveCharacter != null &&
               player.ActiveCharacter.PassiveAbilities.Contains(passive);
    }

    // Status Effects
    public static bool HasStatusEffect(
        Player player,
        string effect)
    {
        return player.StatusEffects.Contains(effect);
    }

    public static void RemoveStatusEffect(
        Player player,
        string effect)
    {
        player.StatusEffects.Remove(effect);
    }
}
