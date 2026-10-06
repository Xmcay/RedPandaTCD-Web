namespace RedPandaTCD_Web.Game;

public static class UtilityManager
{
    public static bool PlayUtility(
        Player player,
        Player opponent,
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
            return false;

        if (handSlot.CardInSlot!.Type != CardType.Utility)
            return false;

        Card card = handSlot.CardInSlot;

        // Discard For Energy requires the player
        // to choose another card first.
        if (card.Name == "Discard For Energy")
        {
            return false;
        }

        if (player.Energy < card.Cost)
            return false;

        player.Energy -= card.Cost;

        handSlot.CardInSlot = null;
        player.DiscardPile.Add(card);

        player.UtilityActionsRemaining--;

        switch (card.Name)
        {
            case "Energy Potion":
            {
                int before = player.Energy;

                player.Energy =
                    Math.Min(
                        player.MaxEnergy,
                        player.Energy + 3);

                BattleLog.Write(
                    $"{player.Name} used Energy Potion " +
                    $"(+{player.Energy - before} Energy, " +
                    $"{player.Energy} Total)");

                break;
            }

            case "Energy Drain":
            {
                if (player.EnergyDrainedThisPhaseCount < 1)
                {
                    player.EnergyDrainedThisPhaseCount++;

                    opponent.Energy =
                        Math.Max(
                            1,
                            opponent.Energy - 2);

                    player.Energy =
                        Math.Min(
                            player.MaxEnergy,
                            player.Energy + 2);

                    BattleLog.Write(
                        $"{player.Name} used Energy Drain " +
                        $"({opponent.Name}: {opponent.Energy} Energy, " +
                        $"{player.Name}: {player.Energy} Energy)");
                }

                break;
            }

            case "Attack Amplifier":
            {
                player.TemporaryAttackBonus++;

                BattleLog.Write(
                    $"{player.Name} used Attack Amplifier " +
                    $"(+1 Attack Damage)");

                break;
            }

            case "Defensive Stance":
            {
                player.StatusEffects.Add(
                    "DeflectNextHit");

                BattleLog.Write(
                    $"{player.Name} used Defensive Stance " +
                    $"(Deflect Next Hit)");

                break;
            }

            case "Shield Booster":
            {
                player.Shield += 2;

                BattleLog.Write(
                    $"{player.Name} used Shield Booster " +
                    $"(+2 Shield, {player.Shield} Total)");

                break;
            }

            case "Fortify":
            {
                int attacks =
                    (player.AttackSlot1 != null ? 1 : 0) +
                    (player.AttackSlot2 != null ? 1 : 0);

                player.Shield += attacks;

                BattleLog.Write(
                    $"{player.Name} used Fortify " +
                    $"(+{attacks} Shield, {player.Shield} Total)");

                break;
            }

            case "Discount Coupon":
            {
                player.DiscountActive = true;

                BattleLog.Write(
                    $"{player.Name} used Discount Coupon " +
                    $"(Next Card Costs 1 Less)");

                break;
            }
        }

        return true;
    }

    public static bool DiscardForEnergy(
        Player player,
        int utilitySlotNumber,
        int targetSlotNumber)
    {
        if (utilitySlotNumber < 1 ||
            utilitySlotNumber > Player.MAX_HAND_SIZE ||
            targetSlotNumber < 1 ||
            targetSlotNumber > Player.MAX_HAND_SIZE)
        {
            return false;
        }

        if (utilitySlotNumber == targetSlotNumber)
        {
            return false;
        }

        HandSlot utilitySlot =
            player.HandSlots[utilitySlotNumber - 1];

        HandSlot targetSlot =
            player.HandSlots[targetSlotNumber - 1];

        if (utilitySlot.IsEmpty ||
            utilitySlot.CardInSlot == null ||
            targetSlot.IsEmpty ||
            targetSlot.CardInSlot == null)
        {
            return false;
        }

        Card utilityCard =
            utilitySlot.CardInSlot;

        Card discardedCard =
            targetSlot.CardInSlot;

        if (utilityCard.Name != "Discard For Energy" ||
            utilityCard.Type != CardType.Utility)
        {
            return false;
        }

        if (player.UtilityActionsRemaining <= 0)
        {
            return false;
        }

        // Remove both cards from the hand.
        utilitySlot.CardInSlot = null;
        targetSlot.CardInSlot = null;

        // Both cards go to the discard pile.
        player.DiscardPile.Add(utilityCard);
        player.DiscardPile.Add(discardedCard);

        player.UtilityActionsRemaining--;

        int before =
            player.Energy;

        player.Energy =
            Math.Min(
                player.MaxEnergy,
                player.Energy + discardedCard.Cost);

        int gained =
            player.Energy - before;

        BattleLog.Write(
            $"{player.Name} used Discard For Energy, " +
            $"discarding {discardedCard.Name} " +
            $"(+{gained} Energy, " +
            $"{player.Energy} Total)");

        return true;
    }

    public static bool DiscardAndDraw(
        Player player,
        int slotNumber)
    {
        if (slotNumber < 1 ||
            slotNumber > Player.MAX_HAND_SIZE)
        {
            return false;
        }

        if (player.HandSlots[slotNumber - 1].IsEmpty)
        {
            return false;
        }

        HandManager.DiscardCard(
            player,
            slotNumber);

        HandManager.DrawCard(player);

        BattleLog.Write(
            $"{player.Name} discarded a card and drew a replacement");

        return true;
    }

    public static List<Card>? PeekDeck(Player player)
    {
        if (player.Deck.Structure != DeckStructure.Queue &&
            player.Deck.Structure != DeckStructure.Stack)
        {
            return null;
        }

        List<Card> cards =
            player.Deck.RuntimeStorage!.GetCards();

        if (cards.Count == 0)
        {
            return null;
        }

        bool freePeek =
            CharacterManager.HasPassive(
                player,
                "FreePeek");

        if (!freePeek)
        {
            if (player.UtilityActionsRemaining <= 0)
            {
                return null;
            }

            player.UtilityActionsRemaining--;
        }

        List<Card> peekedCards =
            cards
                .Take(Math.Min(2, cards.Count))
                .ToList();

        BattleLog.Write(
            $"{player.Name} peeked at the next cards");

        return peekedCards;
    }

    public static List<Card>? ViewDrawPool(Player player)
    {
        if (player.Deck.Structure != DeckStructure.RandomList)
        {
            return null;
        }

        List<Card> cards =
            player.Deck.RuntimeStorage!.GetCards();

        if (cards.Count == 0)
        {
            return null;
        }

        bool freePeek =
            CharacterManager.HasPassive(
                player,
                "FreePeek");

        if (!freePeek)
        {
            if (player.UtilityActionsRemaining <= 0)
            {
                return null;
            }

            player.UtilityActionsRemaining--;
        }

        BattleLog.Write(
            $"{player.Name} viewed the Random draw pool");

        return cards;
    }

    public static bool StartReorderNodes(Player player)
    {
        if (player.Deck.Structure != DeckStructure.LinkedList)
        {
            return false;
        }

        if (player.IsReorderingNodes)
        {
            return true;
        }

        if (player.UtilityActionsRemaining <= 0)
        {
            return false;
        }

        if (player.Deck.RuntimeStorage == null ||
            player.Deck.RuntimeStorage.Count == 0)
        {
            return false;
        }

        player.UtilityActionsRemaining--;
        player.ReorderMovesThisPhase = 0;
        player.IsReorderingNodes = true;

        BattleLog.Write(
            $"{player.Name} started Reorder Nodes");

        return true;
    }

    public static bool ReorderNodes(
        Player player,
        int fromPosition,
        int toPosition)
    {
        if (!player.IsReorderingNodes)
        {
            return false;
        }

        if (player.Deck.Structure != DeckStructure.LinkedList)
        {
            return false;
        }

        List<Card> currentCards =
            player.Deck.RuntimeStorage!.GetCards();

        int visibleCount =
            Math.Min(6, currentCards.Count);

        if (fromPosition < 1 ||
            fromPosition > visibleCount ||
            toPosition < 1 ||
            toPosition > visibleCount)
        {
            return false;
        }

        int moveCost =
            player.ReorderMovesThisPhase + 1;

        if (player.Energy < moveCost)
        {
            return false;
        }

        string movedCard =
            currentCards[fromPosition - 1].Name;

        bool moved =
            DeckStructureManager.MoveNode(
                player,
                fromPosition - 1,
                toPosition - 1);

        if (!moved)
        {
            return false;
        }

        player.Energy -= moveCost;
        player.ReorderMovesThisPhase++;

        BattleLog.Write(
            $"Moved {movedCard} from Position {fromPosition} " +
            $"to Position {toPosition} (-{moveCost} Energy)");

        return true;
    }

    public static void FinishReorderNodes(Player player)
    {
        if (!player.IsReorderingNodes)
        {
            return;
        }

        player.IsReorderingNodes = false;

        BattleLog.Write(
            $"{player.Name} finished Reorder Nodes");
    }

    public static bool BotPlayUtility(
        Player player,
        Player opponent,
        int slotNumber)
    {
        return PlayUtility(
            player,
            opponent,
            slotNumber);
    }
}