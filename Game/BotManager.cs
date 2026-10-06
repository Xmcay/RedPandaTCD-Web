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
        if (CharacterManager.UseAbility(attacker))
        {
            attacker.AbilityUsedThisPhase = true;
        }
    }

    AttackManager.BotUseAttack(
        attacker,
        defender,
        1);

    if (MatchManager.IsMatchOver(
            attacker,
            defender))
    {
        return;
    }

    AttackManager.BotUseAttack(
        attacker,
        defender,
        2);
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

        return UtilityManager.BotPlayUtility(
            player,
            opponent,
            slot.SlotNumber);
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

            if (!CharacterManager.UseAbility(attacker))
            {
                return false;
            }

            attacker.AbilityUsedThisPhase = true;
            return true;

        case 1:
            return AttackManager.BotUseAttack(
                attacker,
                defender,
                1);

        case 2:
            return AttackManager.BotUseAttack(
                attacker,
                defender,
                2);

        default:
            return false;
    }
}
}