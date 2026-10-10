namespace RedPandaTCD_Web.Game;

public static class BotManager
{
    public static void RunUtility(
    Player player,
    Player opponent)
{
    foreach (HandSlot slot in player.HandSlots)
    {
        if (slot.IsEmpty)
        {
            continue;
        }

        if (slot.CardInSlot!.Type != CardType.Utility)
        {
            continue;
        }

        Card utilityCard = slot.CardInSlot;

        if (utilityCard.Name == "Discard For Energy")
        {
            foreach (HandSlot targetSlot in player.HandSlots)
            {
                if (targetSlot.IsEmpty)
                {
                    continue;
                }

                if (targetSlot.SlotNumber == slot.SlotNumber)
                {
                    continue;
                }

                if (UtilityManager.DiscardForEnergy(
                        player,
                        slot.SlotNumber,
                        targetSlot.SlotNumber))
                {
                    return;
                }
            }

            continue;
        }

        if (UtilityManager.BotPlayUtility(
                player,
                opponent,
                slot.SlotNumber))
        {
            return;
        }
    }
}
    public static void RunPlacement(Player player)
{
    PlacementManager.BotResolveFatigue(player);

    if (player.ActiveCharacter == null)
    {
        foreach (HandSlot slot in player.HandSlots)
        {
            if (slot.IsEmpty)
            {
                continue;
            }

            if (slot.CardInSlot!.Type == CardType.Character)
            {
                if (PlacementManager.BotDeployCharacter(
                        player,
                        slot.SlotNumber))
                {
                    break;
                }
            }
        }
    }

    foreach (HandSlot slot in player.HandSlots)
    {
        if (slot.IsEmpty)
        {
            continue;
        }

        if (slot.CardInSlot!.Type != CardType.Attack)
        {
            continue;
        }

        if (player.AttackSlot1 == null)
        {
            PlacementManager.BotDeployAttack(
                player,
                slot.SlotNumber,
                1);

            continue;
        }

        if (player.AttackSlot2 == null)
        {
            PlacementManager.BotDeployAttack(
                player,
                slot.SlotNumber,
                2);
        }
    }
}
public static void RunAttack(
    Player attacker,
    Player defender)
{
    if (attacker.ActiveCharacter != null &&
        !attacker.CharacterNewlyDeployed &&
        !attacker.AbilityUsedThisPhase)
    {
        // Abilities can change the value of the attacks, so resolve the
        // existing legal ability action before evaluating the current board.
        AttackManager.UseCharacterAbility(attacker);
    }

    // Re-evaluate after each attack: the first attack may break a shield,
    // defeat a character, consume a one-hit effect, or end the match.
    for (int action = 0; action < 2; action++)
    {
        if (MatchManager.IsMatchOver(attacker, defender))
            return;

        int first = GetAttackValue(attacker, defender, 1) >=
                    GetAttackValue(attacker, defender, 2) ? 1 : 2;
        int second = first == 1 ? 2 : 1;
        int chosen = CanUseAttack(attacker, first) ? first : second;

        if (!CanUseAttack(attacker, chosen))
            return;

        // Do not pay energy for an attack that produces no worthwhile result.
        // A confirmed lethal or character defeat is always allowed through.
        if (GetAttackValue(attacker, defender, chosen) <= 0)
            return;

        AttackManager.BotUseAttack(attacker, defender, chosen);
    }
}

private static bool CanUseAttack(Player player, int slotNumber)
{
    Card? attack = slotNumber == 1 ? player.AttackSlot1 : player.AttackSlot2;
    if (attack == null)
        return false;

    bool fatigued = slotNumber == 1 ? player.IsFatiguedSlot1 : player.IsFatiguedSlot2;
    bool resting = slotNumber == 1 ? player.IsRestingSlot1 : player.IsRestingSlot2;
    bool used = slotNumber == 1 ? player.AttackSlot1UsedThisPhase : player.AttackSlot2UsedThisPhase;
    int cost = slotNumber == 1 ? player.AttackSlot1TotalUses : player.AttackSlot2TotalUses;

    return !fatigued && !resting && !used && player.Energy >= cost;
}

// Estimates the result of one attack against the CURRENT board. This is a
// decision heuristic only; AttackManager remains the authority that resolves it.
private static double GetAttackValue(Player attacker, Player defender, int slotNumber)
{
    if (!CanUseAttack(attacker, slotNumber))
        return double.NegativeInfinity;

    Card attack = (slotNumber == 1 ? attacker.AttackSlot1 : attacker.AttackSlot2)!;
    int useCost = slotNumber == 1 ? attacker.AttackSlot1TotalUses : attacker.AttackSlot2TotalUses;
    // Paying the cost would end our own game, so this is not a useful trade.
    if (attacker.Energy - useCost <= 0)
        return -10000;

    int damage = AttackManager.GetEffectiveDamage(attacker, attack);
    int shield = defender.Shield;
    int hp = defender.CurrentCharacterHp;
    int energy = defender.Energy;
    bool hasCharacter = defender.ActiveCharacter != null;
    bool sageEntry = defender.CharacterNewlyDeployed && !defender.EntryDefenseUsed &&
                     defender.ActiveCharacter?.Name == "Sage Red Panda";
    bool deflect = CharacterManager.HasStatusEffect(defender, "DeflectNextHit");
    int landedHits = 0;
    int hpDamage = 0;
    int energyDamage = 0;
    bool characterKilled = false;
    bool lethal = false;

    for (int hit = 0; hit < Math.Max(1, attack.Hits); hit++)
    {
        int hitDamage = damage;
        if (sageEntry)
        {
            hitDamage = Math.Min(hitDamage, 2);
            sageEntry = false;
        }
        if (CharacterManager.HasPassive(defender, "MultiHitResistance") && attack.Hits > 1)
            hitDamage = Math.Max(1, hitDamage - 1);

        if (deflect)
        {
            deflect = false;
            continue;
        }

        landedHits++;
        if (attack.IsPiercing)
        {
            int shieldDamage = hitDamage / 2;
            shield = Math.Max(0, shield - shieldDamage);
            int directDamage = hitDamage - shieldDamage;
            if (hasCharacter)
            {
                if (directDamage >= hp)
                {
                    int overflow = directDamage - hp;
                    hp = 0;
                    hasCharacter = false;
                    characterKilled = true;
                    shield = 0;
                    energyDamage += overflow;
                }
                else hp -= directDamage;
            }
            else energyDamage += directDamage;
        }
        else if (shield > 0)
        {
            bool fortified = defender.CharacterNewlyDeployed && !defender.EntryDefenseUsed &&
                             defender.ActiveCharacter?.Name == "Guardian Red Panda";
            if (fortified)
                shield -= Math.Min(hitDamage, Math.Max(0, shield - 1));
            else
                shield = Math.Max(0, shield - hitDamage);
        }
        else if (hasCharacter)
        {
            if (hitDamage >= hp)
            {
                int overflow = hitDamage - hp;
                hp = 0;
                hasCharacter = false;
                characterKilled = true;
                shield = 0;
                energyDamage += overflow;
            }
            else hp -= hitDamage;
        }
        else energyDamage += hitDamage;

        if (energy - energyDamage <= 0)
        {
            lethal = true;
            break;
        }
    }

    if (CharacterManager.HasPassive(defender, "ShieldRecovery"))
        shield += Math.Min(2, landedHits);

    if (lethal)
        return 10000 + energyDamage;

    double value = hpDamage * 3.0 + energyDamage * 1.8;
    int actualHpDamage = defender.ActiveCharacter == null
        ? 0
        : Math.Max(0, defender.CurrentCharacterHp - hp);
    value += actualHpDamage * 3.0;
    if (characterKilled)
        value += 25.0; // Removing the character also removes its passives.

    int netShieldLoss = Math.Max(0, defender.Shield - shield);
    value += netShieldLoss * 0.65;
    value -= useCost * (attacker.Energy <= 8 ? 2.0 : 0.8);

    // An attack that is entirely undone by shield recovery is usually not
    // worth spending energy on unless it defeated a character or was lethal.
    return value;
}

public static bool RunNextUtilityAction(
    Player player,
    Player opponent)
{
    foreach (HandSlot slot in player.HandSlots)
    {
        if (slot.IsEmpty)
        {
            continue;
        }

        if (slot.CardInSlot!.Type != CardType.Utility)
        {
            continue;
        }

        if (UtilityManager.BotPlayUtility(
                player,
                opponent,
                slot.SlotNumber))
        {
            return true;
        }

        // An unusable Utility card should not prevent the bot
        // from trying later Utility cards in its hand.
    }

    return false;
}
public static bool RunNextPlacementAction(
    Player player,
    int step)
{
    switch (step)
    {
        case 0:
            return PlacementManager.BotResolveFatiguedSlot(
                player,
                1);

        case 1:
            return PlacementManager.BotResolveFatiguedSlot(
                player,
                2);

        case 2:
            if (player.ActiveCharacter != null)
            {
                return false;
            }

            foreach (HandSlot slot in player.HandSlots)
            {
                if (slot.IsEmpty)
                {
                    continue;
                }

                if (slot.CardInSlot!.Type != CardType.Character)
                {
                    continue;
                }

                if (PlacementManager.BotDeployCharacter(
                        player,
                        slot.SlotNumber))
                {
                    return true;
                }
            }

            return false;

        case 3:
            if (player.AttackSlot1 != null)
            {
                return false;
            }

            foreach (HandSlot slot in player.HandSlots)
            {
                if (slot.IsEmpty)
                {
                    continue;
                }

                if (slot.CardInSlot!.Type != CardType.Attack)
                {
                    continue;
                }

                if (PlacementManager.BotDeployAttack(
                        player,
                        slot.SlotNumber,
                        1))
                {
                    return true;
                }
            }

            return false;

        case 4:
            if (player.AttackSlot2 != null)
            {
                return false;
            }

            foreach (HandSlot slot in player.HandSlots)
            {
                if (slot.IsEmpty)
                {
                    continue;
                }

                if (slot.CardInSlot!.Type != CardType.Attack)
                {
                    continue;
                }

                if (PlacementManager.BotDeployAttack(
                        player,
                        slot.SlotNumber,
                        2))
                {
                    return true;
                }
            }

            return false;

        default:
            return false;
    }
}
public static bool RunNextAttackAction(
    Player attacker,
    Player defender,
    int step)
{
    switch (step)
    {
        case 0:
            if (attacker.ActiveCharacter == null ||
                attacker.CharacterNewlyDeployed ||
                attacker.AbilityUsedThisPhase)
            {
                return false;
            }

            return AttackManager.UseCharacterAbility(attacker);

        case 1:
            if (GetAttackValue(attacker, defender, 1) <= 0)
                return false;
            return AttackManager.BotUseAttack(attacker, defender, 1);

        case 2:
            if (GetAttackValue(attacker, defender, 2) <= 0)
                return false;
            return AttackManager.BotUseAttack(attacker, defender, 2);

        default:
            return false;
    }
}
}
