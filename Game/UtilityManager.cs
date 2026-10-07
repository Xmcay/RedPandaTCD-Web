namespace RedPandaTCD_Web.Game;

public static class UtilityManager
{
    // ==========================================================
    // USE UTILITY
    // ==========================================================

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
        {
            return false;
        }

        if (handSlot.CardInSlot!.Type != CardType.Utility)
        {
            return false;
        }

        Card card =
            handSlot.CardInSlot;

        // Discard For Energy requires
        // a second card selection.
        if (card.Name == "Discard For Energy")
        {
            return false;
        }

        if (player.Energy < card.Cost)
        {
            return false;
        }

        player.Energy -=
            card.Cost;

        handSlot.CardInSlot = null;

        player.DiscardPile.Add(
            card);

        player.UtilityActionsRemaining--;

        // ======================================================
        // ENERGY POTION
        // ======================================================

        switch (card.Name)
        {
            case "Energy Potion":
            {
                int before =
                    player.Energy;

                player.Energy =
                    Math.Min(
                        player.MaxEnergy,
                        player.Energy + 3);

                int gained =
                    player.Energy - before;

                BattleLog.WriteReplay(
                    $"{player.Name} used Energy Potion " +
                    $"(+{gained} Energy, " +
                    $"{player.Energy} Total)",
                    BattleLog.CurrentTurnNumber,
                    BattleLog.CurrentPlayerIndex,
                    BattleLogEventType.Utility,
                    card.Name,
                    card.Type,
                    player.Deck.Structure,
                    slotNumber: slotNumber,
                    amount: gained,
                    playerEnergy: player.Energy);

                break;
            }

            // ==================================================
            // ENERGY DRAIN
            // ==================================================

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

                    BattleLog.WriteReplay(
                        $"{player.Name} used Energy Drain " +
                        $"({opponent.Name}: {opponent.Energy} Energy, " +
                        $"{player.Name}: {player.Energy} Energy)",
                        BattleLog.CurrentTurnNumber,
                        BattleLog.CurrentPlayerIndex,
                        BattleLogEventType.Utility,
                        card.Name,
                        card.Type,
                        player.Deck.Structure,
                        targetPlayerIndex:
                            BattleLog.GetOpponentPlayerIndex(),
                        amount: 2,
                        slotNumber: slotNumber,
                        playerEnergy: player.Energy,
                        opponentEnergy: opponent.Energy);
                }

                break;
            }

            // ==================================================
            // ATTACK AMPLIFIER
            // ==================================================

            case "Attack Amplifier":
            {
                player.TemporaryAttackBonus++;

                BattleLog.WriteReplay(
                    $"{player.Name} used Attack Amplifier " +
                    $"(+1 Attack Damage)",
                    BattleLog.CurrentTurnNumber,
                    BattleLog.CurrentPlayerIndex,
                    BattleLogEventType.Utility,
                    card.Name,
                    card.Type,
                    player.Deck.Structure,
                    slotNumber: slotNumber,
                    amount: 1);

                break;
            }

            // ==================================================
            // DEFENSIVE STANCE
            // ==================================================

            case "Defensive Stance":
            {
                player.StatusEffects.Add(
                    "DeflectNextHit");

                BattleLog.WriteReplay(
                    $"{player.Name} used Defensive Stance " +
                    $"(Deflect Next Hit)",
                    BattleLog.CurrentTurnNumber,
                    BattleLog.CurrentPlayerIndex,
                    BattleLogEventType.Utility,
                    card.Name,
                    card.Type,
                    player.Deck.Structure,
                    slotNumber: slotNumber);

                break;
            }

            // ==================================================
            // SHIELD BOOSTER
            // ==================================================

            case "Shield Booster":
            {
                player.Shield +=
                    2;

                BattleLog.WriteReplay(
                    $"{player.Name} used Shield Booster " +
                    $"(+2 Shield, {player.Shield} Total)",
                    BattleLog.CurrentTurnNumber,
                    BattleLog.CurrentPlayerIndex,
                    BattleLogEventType.Utility,
                    card.Name,
                    card.Type,
                    player.Deck.Structure,
                    slotNumber: slotNumber,
                    amount: 2,
                    playerShield: player.Shield);

                break;
            }

            // ==================================================
            // FORTIFY
            // ==================================================

            case "Fortify":
            {
                int attacks =
                    (player.AttackSlot1 != null ? 1 : 0) +
                    (player.AttackSlot2 != null ? 1 : 0);

                player.Shield +=
                    attacks;

                BattleLog.WriteReplay(
                    $"{player.Name} used Fortify " +
                    $"(+{attacks} Shield, {player.Shield} Total)",
                    BattleLog.CurrentTurnNumber,
                    BattleLog.CurrentPlayerIndex,
                    BattleLogEventType.Utility,
                    card.Name,
                    card.Type,
                    player.Deck.Structure,
                    slotNumber: slotNumber,
                    amount: attacks,
                    playerShield: player.Shield);

                break;
            }

            // ==================================================
            // DISCOUNT COUPON
            // ==================================================

            case "Discount Coupon":
            {
                player.DiscountActive = true;

                BattleLog.WriteReplay(
                    $"{player.Name} used Discount Coupon " +
                    $"(Next Card Costs 1 Less)",
                    BattleLog.CurrentTurnNumber,
                    BattleLog.CurrentPlayerIndex,
                    BattleLogEventType.Utility,
                    card.Name,
                    card.Type,
                    player.Deck.Structure,
                    slotNumber: slotNumber);

                break;
            }
        }

        return true;
    }


    // ==========================================================
    // DISCARD FOR ENERGY
    // ==========================================================

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

        utilitySlot.CardInSlot = null;

        targetSlot.CardInSlot = null;

        player.DiscardPile.Add(
            utilityCard);

        player.DiscardPile.Add(
            discardedCard);

        player.UtilityActionsRemaining--;

        int before =
            player.Energy;

        player.Energy =
            Math.Min(
                player.MaxEnergy,
                player.Energy + discardedCard.Cost);

        int gained =
            player.Energy - before;

        BattleLog.WriteReplay(
            $"{player.Name} used Discard For Energy, " +
            $"discarding {discardedCard.Name} " +
            $"(+{gained} Energy, " +
            $"{player.Energy} Total)",
            BattleLog.CurrentTurnNumber,
            BattleLog.CurrentPlayerIndex,
            BattleLogEventType.Utility,
            utilityCard.Name,
            utilityCard.Type,
            player.Deck.Structure,
            targetCardName: discardedCard.Name,
            targetSlotNumber: targetSlotNumber,
            slotNumber: utilitySlotNumber,
            amount: gained,
            playerEnergy: player.Energy);

        return true;
    }


    // ==========================================================
    // DISCARD AND DRAW
    // ==========================================================

    public static bool DiscardAndDraw(
        Player player,
        int slotNumber,
        int playerIndex,
        int turnNumber)
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

        Card? discardedCard =
            player.HandSlots[slotNumber - 1].CardInSlot;

        HandManager.DiscardCard(
            player,
            slotNumber);

        HandManager.DrawCard(
            player,
            playerIndex,
            turnNumber);

        BattleLog.WriteReplay(
            $"{player.Name} discarded a card and drew a replacement",
            turnNumber,
            playerIndex,
            BattleLogEventType.Utility,
            targetCardName:
                discardedCard?.Name,
            deckStructure:
                player.Deck.Structure,
            slotNumber:
                slotNumber);

        return true;
    }


    // ==========================================================
    // PEEK DECK
    // ==========================================================

    public static List<Card>? PeekDeck(
        Player player)
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

        BattleLog.WriteReplay(
            $"{player.Name} peeked at the next cards",
            BattleLog.CurrentTurnNumber,
            BattleLog.CurrentPlayerIndex,
            BattleLogEventType.Utility,
            deckStructure:
                player.Deck.Structure,
            amount:
                peekedCards.Count);

        return peekedCards;
    }


    // ==========================================================
    // VIEW RANDOM DRAW POOL
    // ==========================================================

    public static List<Card>? ViewDrawPool(
        Player player)
    {
        if (player.Deck.Structure !=
            DeckStructure.RandomList)
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

        BattleLog.WriteReplay(
            $"{player.Name} viewed the Random draw pool",
            BattleLog.CurrentTurnNumber,
            BattleLog.CurrentPlayerIndex,
            BattleLogEventType.Utility,
            deckStructure:
                player.Deck.Structure,
            amount:
                cards.Count);

        return cards;
    }


    // ==========================================================
    // START REORDER NODES
    // ==========================================================

    public static bool StartReorderNodes(
        Player player)
    {
        if (player.Deck.Structure !=
            DeckStructure.LinkedList)
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

        BattleLog.WriteReplay(
            $"{player.Name} started Reorder Nodes",
            BattleLog.CurrentTurnNumber,
            BattleLog.CurrentPlayerIndex,
            BattleLogEventType.Utility,
            deckStructure:
                player.Deck.Structure);

        return true;
    }


    // ==========================================================
    // REORDER NODES
    // ==========================================================

    public static bool ReorderNodes(
        Player player,
        int fromPosition,
        int toPosition)
    {
        if (!player.IsReorderingNodes)
        {
            return false;
        }

        if (player.Deck.Structure !=
            DeckStructure.LinkedList)
        {
            return false;
        }

        List<Card> currentCards =
            player.Deck.RuntimeStorage!.GetCards();

        int visibleCount =
            Math.Min(
                6,
                currentCards.Count);

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

        player.Energy -=
            moveCost;

        player.ReorderMovesThisPhase++;

        BattleLog.WriteReplay(
            $"Moved {movedCard} from Position {fromPosition} " +
            $"to Position {toPosition} (-{moveCost} Energy)",
            BattleLog.CurrentTurnNumber,
            BattleLog.CurrentPlayerIndex,
            BattleLogEventType.Utility,
            targetCardName:
                movedCard,
            deckStructure:
                player.Deck.Structure,
            amount:
                -moveCost,
            playerEnergy:
                player.Energy);

        return true;
    }


    // ==========================================================
    // FINISH REORDER NODES
    // ==========================================================

    public static void FinishReorderNodes(
        Player player)
    {
        if (!player.IsReorderingNodes)
        {
            return;
        }

        player.IsReorderingNodes = false;

        BattleLog.WriteReplay(
            $"{player.Name} finished Reorder Nodes",
            BattleLog.CurrentTurnNumber,
            BattleLog.CurrentPlayerIndex,
            BattleLogEventType.Utility,
            deckStructure:
                player.Deck.Structure);
    }


    // ==========================================================
    // BOT
    // ==========================================================

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