namespace RedPandaTCD_Web.Game;

public static class HandManager
{
    public static void ClearHand(Player player)
    {
        foreach (var slot in player.HandSlots)
        {
            slot.CardInSlot = null;
        }
    }

    public static int CountCards(Player player)
    {
        return player.HandSlots.Count(s => !s.IsEmpty);
    }

    public static HandSlot? GetFirstEmptySlot(Player player)
    {
        return player.HandSlots.FirstOrDefault(s => s.IsEmpty);
    }

    public static void RecycleDiscardPile(
        Player player)
    {
        if (player.DiscardPile.Count == 0)
        {
            return;
        }

        int recycledCount =
            player.DiscardPile.Count;

        foreach (Card card in player.DiscardPile)
        {
            player.Deck.RuntimeStorage!.Add(card);
        }

        player.DiscardPile.Clear();

        BattleLog.Write(
            $"{player.Name} recycled their discard pile " +
            $"into their deck ({recycledCount} card(s))");
    }

    public static void DrawCard(Player player)
    {
        DrawCard(
            player,
            -1,
            0);
    }

    public static void DrawCard(
        Player player,
        int playerIndex,
        int turnNumber)
    {
        if (player == null)
        {
            return;
        }

        if (player.Deck == null)
        {
            return;
        }

        if (player.HandSlots == null)
        {
            return;
        }

        HandSlot? slot =
            GetFirstEmptySlot(player);

        if (slot == null)
        {
            return;
        }

        if (player.Deck.RuntimeStorage!.Count == 0)
        {
            RecycleDiscardPile(player);
        }

        if (player.Deck.RuntimeStorage!.Count == 0)
        {
            return;
        }

        Card? drawnCard =
            DeckStructureManager.DrawFromDeck(player);

        if (drawnCard == null)
        {
            return;
        }

        slot.CardInSlot = drawnCard;

        if (playerIndex >= 1 &&
            turnNumber >= 1)
        {
            BattleLog.WriteDraw(
                $"{player.Name} drew {drawnCard.Name}.",
                turnNumber,
                playerIndex,
                drawnCard.Name,
                drawnCard.Type,
                player.Deck.Structure);
        }
    }

    public static void DrawUntilHandSize(Player player)
    {
        DrawUntilHandSize(
            player,
            -1,
            0);
    }

    public static void DrawUntilHandSize(
        Player player,
        int playerIndex,
        int turnNumber)
    {
        int targetHandSize =
            DeckStructureManager.GetHandSize(player.Deck);

        while (CountCards(player) < targetHandSize)
        {
            DrawCard(
                player,
                playerIndex,
                turnNumber);
        }
    }

    public static Card? RemoveCard(
        Player player,
        int slotNumber)
    {
        if (slotNumber < 1 ||
            slotNumber > Player.MAX_HAND_SIZE)
        {
            return null;
        }

        HandSlot slot =
            player.HandSlots[slotNumber - 1];

        if (slot.IsEmpty)
        {
            return null;
        }

        Card card =
            slot.CardInSlot!;

        slot.CardInSlot = null;

        return card;
    }

    public static void DrawOpeningHand(
        Player player,
        int playerIndex)
    {
        const int openingTurnNumber = 1;

        if (player.Deck.Structure == DeckStructure.RandomList &&
            player.Deck.RuntimeStorage is RandomListDeckStorage randomStorage)
        {
            Card? character =
                randomStorage.DrawRandomCharacter();

            if (character != null)
            {
                HandSlot? slot =
                    GetFirstEmptySlot(player);

                if (slot != null)
                {
                    slot.CardInSlot = character;

                    BattleLog.WriteDraw(
                        $"{player.Name} drew {character.Name}.",
                        openingTurnNumber,
                        playerIndex,
                        character.Name,
                        character.Type,
                        player.Deck.Structure);
                }
            }
        }

        DrawUntilHandSize(
            player,
            playerIndex,
            openingTurnNumber);
    }

    public static void DiscardCard(
        Player player,
        int slotNumber)
    {
        Card? card =
            RemoveCard(player, slotNumber);

        if (card != null)
        {
            player.DiscardPile.Add(card);
        }
    }
}