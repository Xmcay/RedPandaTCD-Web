namespace RedPandaTCD_Web.Game;

public static class AttackManager
{
    public static bool UseAttack(
        Player attacker,
        Player defender,
        int slotNumber)
    {
        if (slotNumber != 1 &&
            slotNumber != 2)
        {
            return false;
        }

        Card? attack =
            slotNumber == 1
            ? attacker.AttackSlot1
            : attacker.AttackSlot2;

        if (attack == null)
        {
            return false;
        }

        bool fatigued =
            slotNumber == 1
            ? attacker.IsFatiguedSlot1
            : attacker.IsFatiguedSlot2;

        bool resting =
            slotNumber == 1
            ? attacker.IsRestingSlot1
            : attacker.IsRestingSlot2;
        
        bool usedThisPhase =
    slotNumber == 1
        ? attacker.AttackSlot1UsedThisPhase
        : attacker.AttackSlot2UsedThisPhase;

if (usedThisPhase)
{
    return false;
}

        if (fatigued || resting)
        {
            return false;
        }

        int useCost =
            slotNumber == 1
            ? attacker.AttackSlot1TotalUses
            : attacker.AttackSlot2TotalUses;

        if (attacker.Energy < useCost)
        {
            return false;
        }

attacker.Energy -= useCost;

ExecuteAttack(attacker, defender, attack);

if (slotNumber == 1)
{
    attacker.AttackSlot1UsedThisPhase = true;
            attacker.AttackSlot1TotalUses++;
            attacker.AttackSlot1UsesRemaining--;

            if (attacker.AttackSlot1UsesRemaining > 0 &&
                !attacker.IsFatiguedSlot1)
            {
                BattleLog.Write(
                    $"{attack.Name} has " +
                    $"{attacker.AttackSlot1UsesRemaining} use(s) remaining");
            }

            if (attack.InstantFatigue ||
                attacker.AttackSlot1UsesRemaining <= 0)
            {
                attacker.IsFatiguedSlot1 = true;

                BattleLog.Write(
                    $"{attack.Name} became Fatigued!");
            }
        }
        else
        {
            attacker.AttackSlot2TotalUses++;
            attacker.AttackSlot2UsesRemaining--;
            attacker.AttackSlot2UsedThisPhase = true;

            if (attacker.AttackSlot2UsesRemaining > 0 &&
                !attacker.IsFatiguedSlot2)
            {
                BattleLog.Write(
                    $"{attack.Name} has " +
                    $"{attacker.AttackSlot2UsesRemaining} use(s) remaining");
            }

            if (attack.InstantFatigue ||
                attacker.AttackSlot2UsesRemaining <= 0)
            {
                attacker.IsFatiguedSlot2 = true;

                BattleLog.Write(
                    $"{attack.Name} became Fatigued!");
            }
        }

        return true;
    }
    public static bool UseCharacterAbility(Player player)
{
    if (player.ActiveCharacter == null)
    {
        return false;
    }

    if (player.CharacterNewlyDeployed)
    {
        return false;
    }

    if (player.AbilityUsedThisPhase)
    {
        return false;
    }

    if (!CharacterManager.UseAbility(player))
    {
        return false;
    }

    player.AbilityUsedThisPhase = true;

    return true;
}
    private static void ExecuteAttack(
    Player attacker,
    Player defender,
    Card attack)
{
    int damage =
        attack.Damage +
        attacker.TemporaryAttackBonus;

    attacker.TemporaryAttackBonus = 0;

    if (attacker.CharacterNewlyDeployed &&
        attacker.ActiveCharacter != null)
    {
        if (attacker.ActiveCharacter.Name ==
                "Sorcerer Red Panda" &&
            attack.Archetype == Archetype.Magic)
        {
            damage += 1;
        }

        if (attacker.ActiveCharacter.Name ==
                "Berserker Red Panda" &&
            attack.Archetype == Archetype.Physical)
        {
            damage += 1;
        }
    }

    // Arcane Focus
    if (CharacterManager.HasPassive(
            attacker,
            "ArcaneFocus"))
    {
        bool physicalExists =
            attacker.AttackSlot1?.Archetype ==
                Archetype.Physical ||
            attacker.AttackSlot2?.Archetype ==
                Archetype.Physical;

        if (!physicalExists &&
            attack.Archetype == Archetype.Magic)
        {
            damage++;
        }
    }

    // Retaliation
    if (CharacterManager.HasPassive(
            attacker,
            "Retaliation") &&
        attacker.HitDuringPreviousTurn)
    {
        damage++;
    }

    BattleLog.Write(
        $"{attacker.Name} attacked with {attack.Name} " +
        $"({damage} Damage x {attack.Hits} Hit(s))");

    int hitDamage = damage;
    int guardianShieldGain = 0;

    bool fortified =
        defender.CharacterNewlyDeployed &&
        !defender.EntryDefenseUsed &&
        defender.ActiveCharacter?.Name ==
            "Guardian Red Panda" &&
        defender.Shield > 0;

    for (int hit = 0;
     hit < attack.Hits;
     hit++)
{
    bool landed =
        ResolveHit(
            defender,
            hitDamage,
            attack.IsPiercing,
            attack.Hits,
            fortified);

    if (landed)
    {
        guardianShieldGain++;
    }

    if (defender.Energy <= 0)
    {
        break;
    }
}

if (fortified)
{
    defender.EntryDefenseUsed = true;

    BattleLog.Write(
        $"{defender.Name}'s Fortified effect was consumed");
}

// Shield Recovery
if (CharacterManager.HasPassive(
        defender,
        "ShieldRecovery"))
{
    defender.Shield +=
        Math.Min(
            2,
            guardianShieldGain);
}
}
private static bool ResolveHit(
    Player defender,
    int damage,
    bool piercing,
    int totalHits,
    bool fortified)
{
    defender.WasHitThisTurn = true;

    // Sage Entry Effect: Damage Floor
    if (defender.CharacterNewlyDeployed &&
        !defender.EntryDefenseUsed &&
        defender.ActiveCharacter?.Name ==
            "Sage Red Panda")
    {
        damage =
            Math.Min(
                damage,
                2);

        defender.EntryDefenseUsed = true;

        BattleLog.Write(
            $"{defender.Name}'s Damage Floor limited the hit to {damage} Damage");
    }

    // Multi-Hit Resistance
    if (CharacterManager.HasPassive(
            defender,
            "MultiHitResistance") &&
        totalHits > 1)
    {
        damage =
            Math.Max(
                1,
                damage - 1);
    }

    // Deflect Next Hit
    if (CharacterManager.HasStatusEffect(
            defender,
            "DeflectNextHit"))
    {
        CharacterManager.RemoveStatusEffect(
            defender,
            "DeflectNextHit");

        BattleLog.Write(
            $"{defender.Name} deflected the hit");

        return false;
    }

    if (piercing)
    {
        ResolvePiercingHit(
            defender,
            damage);

        return true;
    }

    // Shield
    if (defender.Shield > 0)
    {
        if (fortified)
        {
            int shieldDamage =
                Math.Min(
                    damage,
                    defender.Shield - 1);

            defender.Shield -=
                shieldDamage;

            BattleLog.Write(
                $"{defender.Name}'s Fortified Shield cannot fall below 1");

            return true;
        }

        if (defender.Shield >= damage)
        {
            defender.Shield -= damage;

            BattleLog.Write(
                $"{defender.Name}'s Shield absorbed {damage} Damage");
        }
        else
        {
            defender.Shield = 0;

            BattleLog.Write(
                $"{defender.Name}'s Shield was Broken");
        }

        return true;
    }

    DamageCharacterOrEnergy(
        defender,
        damage);

    return true;
}

private static void ResolvePiercingHit(
    Player defender,
    int damage)
{
    /*
        With no Shield remaining, there is
        nothing for Piercing to bypass.

        Deal the full hit normally instead of
        splitting it into standard and direct
        damage.
    */
    if (defender.Shield <= 0)
    {
        DamageCharacterOrEnergy(
            defender,
            damage);

        return;
    }

    /*
        When Shield exists, split the hit:

        - half interacts with Shield
        - the remainder bypasses Shield
    */
    int standardDamage =
        damage / 2;

    int piercingDamage =
        damage - standardDamage;

    if (defender.Shield >=
        standardDamage)
    {
        defender.Shield -=
            standardDamage;

        BattleLog.Write(
            $"{defender.Name}'s Shield absorbed " +
            $"{standardDamage} Damage");
    }
    else
    {
        defender.Shield = 0;

        BattleLog.Write(
            $"{defender.Name}'s Shield was Broken");
    }

    DamageCharacterOrEnergy(
        defender,
        piercingDamage);

    BattleLog.Write(
        $"Piercing dealt {piercingDamage} direct damage");
}

private static void DamageCharacterOrEnergy(
    Player defender,
    int damage)
{
    if (defender.ActiveCharacter == null)
    {
        defender.Energy =
            Math.Max(
                0,
                defender.Energy - damage);

        BattleLog.Write(
            $"{defender.Name} lost {damage} Energy " +
            $"({defender.Energy} Remaining)");

        return;
    }

    if (damage >= defender.CurrentCharacterHp)
    {
        int overflow =
            damage -
            defender.CurrentCharacterHp;

        BattleLog.Write(
            $"{defender.Name}'s {defender.ActiveCharacter.Name} was Defeated");

        defender.DiscardPile.Add(
            defender.ActiveCharacter);

        defender.ActiveCharacter = null;
        defender.CurrentCharacterHp = 0;
        defender.Shield = 0;

        defender.CharacterDefeatedSinceLastPlacement =
            true;

        if (overflow > 0)
        {
            defender.Energy =
                Math.Max(
                    0,
                    defender.Energy - overflow);

            BattleLog.Write(
                $"{defender.Name} took {overflow} Overflow Energy Damage " +
                $"({defender.Energy} Energy Remaining)");
        }

        return;
    }

    defender.CurrentCharacterHp -=
        damage;

    BattleLog.Write(
        $"{defender.ActiveCharacter.Name} took {damage} HP Damage " +
        $"({defender.CurrentCharacterHp} HP Remaining)");
}
public static bool BotUseAttack(
    Player attacker,
    Player defender,
    int slotNumber)
{
    return UseAttack(
        attacker,
        defender,
        slotNumber);
}
}