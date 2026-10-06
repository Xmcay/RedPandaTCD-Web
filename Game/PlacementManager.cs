namespace RedPandaTCD_Web.Game;

public static class PlacementManager
{
    public static bool TryDeployAttack(
        Player player,
        int handSlotNumber,
        int fieldSlot)
    {
        if (handSlotNumber < 1 ||
            handSlotNumber > Player.MAX_HAND_SIZE)
        {
            return false;
        }

        if (fieldSlot < 1 ||
            fieldSlot > 2)
        {
            return false;
        }

        HandSlot handSlot =
            player.HandSlots[handSlotNumber - 1];

        if (handSlot.IsEmpty)
        {
            return false;
        }

        if (handSlot.CardInSlot!.Type != CardType.Attack)
        {
            return false;
        }

        Card card =
            handSlot.CardInSlot;

        int cost =
            card.Cost;

        if (player.DiscountActive)
        {
            cost =
                Math.Max(
                    0,
                    cost - 1);
        }

        if (player.Energy < cost)
        {
            return false;
        }

        if (player.DiscountActive)
        {
            player.DiscountActive =
                false;
        }

        player.Energy -= cost;

        if (fieldSlot == 1)
        {
            if (player.AttackSlot1 != null)
            {
                player.DiscardPile.Add(
                    player.AttackSlot1);
            }

            player.AttackSlot1 =
                card;

            player.IsFatiguedSlot1 =
                false;

            player.IsRestingSlot1 =
                false;

            player.AttackSlot1UsesRemaining =
                2;

            player.AttackSlot1TotalUses =
                0;
        }
        else
        {
            if (player.AttackSlot2 != null)
            {
                player.DiscardPile.Add(
                    player.AttackSlot2);
            }

            player.AttackSlot2 =
                card;

            player.IsFatiguedSlot2 =
                false;

            player.IsRestingSlot2 =
                false;

            player.AttackSlot2UsesRemaining =
                2;

            player.AttackSlot2TotalUses =
                0;
        }

        handSlot.CardInSlot =
            null;

        BattleLog.Write(
            $"{player.Name} placed {card.Name} in Attack Slot {fieldSlot} " +
            $"(-{cost} Energy)");

        return true;
    }

    public static bool TryDeployCharacter(
        Player player,
        int slotNumber)
    {
        if (slotNumber < 1 ||
            slotNumber > Player.MAX_HAND_SIZE)
        {
            return false;
        }

        HandSlot handSlot =
            player.HandSlots[slotNumber - 1];

        if (handSlot.IsEmpty)
        {
            return false;
        }

        if (handSlot.CardInSlot!.Type != CardType.Character)
        {
            return false;
        }

        Card card =
            handSlot.CardInSlot;

        int cost;

        if (player.CharacterDefeatedSinceLastPlacement)
        {
            cost = 0;
        }
        else if (player.ActiveCharacter != null)
        {
            cost = 4;
        }
        else
        {
            cost = card.Cost;
        }

        if (player.Energy < cost)
        {
            return false;
        }

        player.Energy -= cost;

        if (player.ActiveCharacter != null)
        {
            player.DiscardPile.Add(
                player.ActiveCharacter);
        }

        player.ActiveCharacter = card;
        player.AbilityCooldownRemaining = 0;

        player.CurrentCharacterHp =
            card.HpValue;

        player.Shield =
            card.ShieldValue;

        player.CharacterNewlyDeployed = true;

        player.CharacterDefeatedSinceLastPlacement =
            false;

        handSlot.CardInSlot = null;

        CharacterManager.ApplyEntryEffect(player);

        BattleLog.Write(
            $"{player.Name} deployed {card.Name} " +
            $"(HP:{card.HpValue} SH:{card.ShieldValue}) " +
            $"(-{cost} Energy)");

        return true;
    }

    public static bool ResolveWandererEntry(
        Player player,
        int chosenSlot)
    {
        if (player.ActiveCharacter?.Name != "Wanderer Red Panda")
        {
            return false;
        }

        if (player.IsBot)
        {
            return false;
        }

        if (player.AttackSlot1 == null &&
            player.AttackSlot2 == null)
        {
            BattleLog.Write(
                $"{player.Name}'s Wanderer had no deployed Attack to adapt");

            return true;
        }

        if (player.AttackSlot1 != null &&
            player.AttackSlot2 == null)
        {
            chosenSlot = 1;
        }
        else if (player.AttackSlot1 == null &&
                 player.AttackSlot2 != null)
        {
            chosenSlot = 2;
        }
        else
        {
            if (chosenSlot != 1 &&
                chosenSlot != 2)
            {
                return false;
            }
        }

        CharacterManager.ReduceAttackBuildup(
            player,
            chosenSlot,
            2);

        Card? chosenAttack =
            chosenSlot == 1
                ? player.AttackSlot1
                : player.AttackSlot2;

        if (chosenAttack == null)
        {
            return false;
        }

        BattleLog.Write(
            $"{player.Name}'s Wanderer reduced " +
            $"{chosenAttack.Name}'s buildup cost by 2");

        return true;
    }

    public static void AutoStackPlacement(
        Player player)
    {
        if (player.Deck.Structure != DeckStructure.Stack)
        {
            return;
        }

        if (player.ActiveCharacter == null)
        {
            foreach (HandSlot slot in player.HandSlots)
            {
                if (TryDeployCharacter(
                        player,
                        slot.SlotNumber))
                {
                    break;
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
                TryDeployAttack(
                    player,
                    slot.SlotNumber,
                    1);
            }

            if (player.AttackSlot2 == null)
            {
                TryDeployAttack(
                    player,
                    slot.SlotNumber,
                    2);
            }
        }
    }

    public static bool ReplaceFatiguedAttack(
        Player player,
        int slotNumber)
    {
        if (slotNumber == 1)
        {
            if (!player.IsFatiguedSlot1 ||
                player.AttackSlot1 == null)
            {
                return false;
            }

            string attackName =
                player.AttackSlot1.Name;

            player.DiscardPile.Add(
                player.AttackSlot1);

            player.AttackSlot1 = null;
            player.IsFatiguedSlot1 = false;
            player.IsRestingSlot1 = false;
            player.AttackSlot1UsesRemaining = 2;
            player.AttackSlot1TotalUses = 0;

            BattleLog.Write(
                $"{attackName} was replaced");

            return true;
        }

        if (slotNumber == 2)
        {
            if (!player.IsFatiguedSlot2 ||
                player.AttackSlot2 == null)
            {
                return false;
            }

            string attackName =
                player.AttackSlot2.Name;

            player.DiscardPile.Add(
                player.AttackSlot2);

            player.AttackSlot2 = null;
            player.IsFatiguedSlot2 = false;
            player.IsRestingSlot2 = false;
            player.AttackSlot2UsesRemaining = 2;
            player.AttackSlot2TotalUses = 0;

            BattleLog.Write(
                $"{attackName} was replaced");

            return true;
        }

        return false;
    }

    public static bool RestFatiguedAttack(
        Player player,
        int slotNumber)
    {
        if (!DeckStructureManager.CanRest(
                player.Deck))
        {
            return false;
        }

        if (slotNumber == 1)
        {
            if (!player.IsFatiguedSlot1 ||
                player.AttackSlot1 == null)
            {
                return false;
            }

            player.IsFatiguedSlot1 = false;
            player.IsRestingSlot1 = true;

            BattleLog.Write(
                $"{player.AttackSlot1.Name} is Resting");

            return true;
        }

        if (slotNumber == 2)
        {
            if (!player.IsFatiguedSlot2 ||
                player.AttackSlot2 == null)
            {
                return false;
            }

            player.IsFatiguedSlot2 = false;
            player.IsRestingSlot2 = true;

            BattleLog.Write(
                $"{player.AttackSlot2.Name} is Resting");

            return true;
        }

        return false;
    }

    private static HandSlot? FindCheaperAttackInHand(
        Player player,
        int buildupCost)
    {
        HandSlot? bestSlot = null;
        int lowestCost = int.MaxValue;

        foreach (HandSlot slot in player.HandSlots)
        {
            if (slot.IsEmpty)
            {
                continue;
            }

            Card card =
                slot.CardInSlot!;

            if (card.Type != CardType.Attack)
            {
                continue;
            }

            int deployCost =
                card.Cost;

            if (player.DiscountActive)
            {
                deployCost =
                    Math.Max(
                        0,
                        deployCost - 1);
            }

            if (deployCost > player.Energy)
            {
                continue;
            }

            if (deployCost >= buildupCost)
            {
                continue;
            }

            if (deployCost < lowestCost)
            {
                lowestCost =
                    deployCost;

                bestSlot =
                    slot;
            }
        }

        return bestSlot;
    }

    public static bool EmergencyRestFatiguedAttack(
        Player player,
        int slotNumber)
    {
        if (player.Deck.Structure !=
            DeckStructure.PriorityQueue)
        {
            return false;
        }

        if (slotNumber == 1)
        {
            if (!player.IsFatiguedSlot1 ||
                player.AttackSlot1 == null)
            {
                return false;
            }

            player.IsFatiguedSlot1 = false;
            player.IsRestingSlot1 = true;

            BattleLog.Write(
                $"{player.AttackSlot1.Name} entered Emergency Rest");

            return true;
        }

        if (slotNumber == 2)
        {
            if (!player.IsFatiguedSlot2 ||
                player.AttackSlot2 == null)
            {
                return false;
            }

            player.IsFatiguedSlot2 = false;
            player.IsRestingSlot2 = true;

            BattleLog.Write(
                $"{player.AttackSlot2.Name} entered Emergency Rest");

            return true;
        }

        return false;
    }

    public static void BotResolveFatigue(
        Player player)
    {
        if (player.IsFatiguedSlot1)
        {
            BotResolveFatiguedSlot(
                player,
                1);
        }

        if (player.IsFatiguedSlot2)
        {
            BotResolveFatiguedSlot(
                player,
                2);
        }
    }

    public static bool BotResolveFatiguedSlot(
        Player player,
        int slotNumber)
    {
        bool isFatigued =
            slotNumber == 1
                ? player.IsFatiguedSlot1
                : player.IsFatiguedSlot2;

        if (!isFatigued)
        {
            return false;
        }

        int buildupCost =
            slotNumber == 1
                ? player.AttackSlot1TotalUses
                : player.AttackSlot2TotalUses;

        bool canRest =
            DeckStructureManager.CanRest(
                player.Deck);

        HandSlot? replacement =
            FindCheaperAttackInHand(
                player,
                buildupCost);

        if (replacement != null)
        {
            bool removed =
                ReplaceFatiguedAttack(
                    player,
                    slotNumber);

            if (!removed)
            {
                return false;
            }

            BotDeployAttack(
                player,
                replacement.SlotNumber,
                slotNumber);

            return true;
        }

        if (canRest)
        {
            return RestFatiguedAttack(
                player,
                slotNumber);
        }

        return EmergencyRestFatiguedAttack(
            player,
            slotNumber);
    }

    public static bool BotDeployCharacter(
        Player player,
        int slotNumber)
    {
        return TryDeployCharacter(
            player,
            slotNumber);
    }

    public static bool BotDeployAttack(
        Player player,
        int handSlot,
        int fieldSlot)
    {
        return TryDeployAttack(
            player,
            handSlot,
            fieldSlot);
    }
}