namespace RedPandaTCD_Web.Game;

public static class EndPhaseManager
{
    public static void Resolve(
        Player player,
        int turnNumber)
    {
        player.CharacterNewlyDeployed = false;
        player.EntryDefenseUsed = false;
        player.AbilityUsedThisPhase = false;

        if (player.AbilityCooldownRemaining > 0)
        {
            player.AbilityCooldownRemaining--;
        }

player.UtilityActionsRemaining = 1;
player.ReorderMovesThisPhase = 0;
player.IsReorderingNodes = false;
player.EnergyDrainedThisPhaseCount = 0;
player.AttackSlot1UsedThisPhase = false;
player.AttackSlot2UsedThisPhase = false;
player.DiscountActive = false;

        if (player.IsRestingSlot1)
        {
            player.IsRestingSlot1 = false;
            player.AttackSlot1UsesRemaining = 2;

            BattleLog.Write(
                $"{player.AttackSlot1?.Name} finished Resting");
        }

        if (player.IsRestingSlot2)
        {
            player.IsRestingSlot2 = false;
            player.AttackSlot2UsesRemaining = 2;

            BattleLog.Write(
                $"{player.AttackSlot2?.Name} finished Resting");
        }

        player.Shield++;

        if (CharacterManager.HasPassive(
                player,
                "EndTurnShield"))
        {
            player.Shield++;
        }

        if (player.ActiveCharacter != null)
        {
            player.Shield =
                Math.Min(
                    player.ActiveCharacter.ShieldValue,
                    player.Shield);

            BattleLog.Write(
                $"{player.Name} now has {player.Shield} Shield");
        }
        else
        {
            player.Shield = 0;
        }

        HandManager.DrawUntilHandSize(player);

        int energyGain = 5;

        if (turnNumber >= 5 &&
            turnNumber <= 9)
        {
            energyGain = 3;
        }
        else if (turnNumber >= 10 &&
                 turnNumber <= 14)
        {
            energyGain = 1;
        }
        else if (turnNumber >= 15)
        {
            energyGain = -1;
        }

        player.Energy =
            Math.Min(
                player.MaxEnergy,
                player.Energy + energyGain);

        if (energyGain < 0)
        {
            player.Energy =
                Math.Max(
                    0,
                    player.Energy);
        }

        BattleLog.Write(
            $"{player.Name} recovered {energyGain} Energy and now has {player.Energy} Energy");

        player.HitDuringPreviousTurn =
            player.WasHitThisTurn;

        player.WasHitThisTurn = false;
    }
}