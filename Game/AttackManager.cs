namespace RedPandaTCD_Web.Game;

public static class AttackManager
{
    // ==========================================================
    // ATTACK USE
    // ==========================================================

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

        int energyBefore =
            attacker.Energy;

        attacker.Energy -= useCost;

        BattleLog.WriteReplay(
            $"{attacker.Name} paid {useCost} Energy to use {attack.Name}.",
            BattleLog.CurrentTurnNumber,
            BattleLog.CurrentPlayerIndex,
            BattleLogEventType.EnergyChange,
            cardName: attack.Name,
            cardType: attack.Type,
            deckStructure: attacker.Deck.Structure,
            amount: -useCost,
            playerEnergy: attacker.Energy);

        ExecuteAttack(
            attacker,
            defender,
            attack);

        if (slotNumber == 1)
        {
            attacker.AttackSlot1UsedThisPhase = true;

            attacker.AttackSlot1TotalUses++;

            attacker.AttackSlot1UsesRemaining--;

            if (attacker.AttackSlot1UsesRemaining > 0 &&
                !attacker.IsFatiguedSlot1)
            {
                BattleLog.WriteReplay(
                    $"{attack.Name} has " +
                    $"{attacker.AttackSlot1UsesRemaining} use(s) remaining",
                    BattleLog.CurrentTurnNumber,
                    BattleLog.CurrentPlayerIndex,
                    BattleLogEventType.Attack,
                    cardName: attack.Name,
                    cardType: attack.Type,
                    deckStructure: attacker.Deck.Structure,
                    slotNumber: slotNumber,
                    amount: attacker.AttackSlot1UsesRemaining);
            }

            if (attack.InstantFatigue ||
                attacker.AttackSlot1UsesRemaining <= 0)
            {
                attacker.IsFatiguedSlot1 = true;

                BattleLog.WriteReplay(
                    $"{attack.Name} became Fatigued!",
                    BattleLog.CurrentTurnNumber,
                    BattleLog.CurrentPlayerIndex,
                    BattleLogEventType.Attack,
                    cardName: attack.Name,
                    cardType: attack.Type,
                    deckStructure: attacker.Deck.Structure,
                    slotNumber: slotNumber);
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
                BattleLog.WriteReplay(
                    $"{attack.Name} has " +
                    $"{attacker.AttackSlot2UsesRemaining} use(s) remaining",
                    BattleLog.CurrentTurnNumber,
                    BattleLog.CurrentPlayerIndex,
                    BattleLogEventType.Attack,
                    cardName: attack.Name,
                    cardType: attack.Type,
                    deckStructure: attacker.Deck.Structure,
                    slotNumber: slotNumber,
                    amount: attacker.AttackSlot2UsesRemaining);
            }

            if (attack.InstantFatigue ||
                attacker.AttackSlot2UsesRemaining <= 0)
            {
                attacker.IsFatiguedSlot2 = true;

                BattleLog.WriteReplay(
                    $"{attack.Name} became Fatigued!",
                    BattleLog.CurrentTurnNumber,
                    BattleLog.CurrentPlayerIndex,
                    BattleLogEventType.Attack,
                    cardName: attack.Name,
                    cardType: attack.Type,
                    deckStructure: attacker.Deck.Structure,
                    slotNumber: slotNumber);
            }
        }

        return true;
    }


    // ==========================================================
    // CHARACTER ABILITY
    // ==========================================================

    public static bool UseCharacterAbility(
        Player player)
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

        string characterName =
            player.ActiveCharacter.Name;

        if (!CharacterManager.UseAbility(player))
        {
            return false;
        }

        player.AbilityUsedThisPhase = true;

        BattleLog.WriteReplay(
            $"{player.Name} used {characterName}'s ability.",
            BattleLog.CurrentTurnNumber,
            BattleLog.CurrentPlayerIndex,
            BattleLogEventType.Ability,
            cardName: characterName,
            cardType: CardType.Character,
            deckStructure: player.Deck.Structure,
            characterHp: player.CurrentCharacterHp,
            playerEnergy: player.Energy,
            playerShield: player.Shield);

        return true;
    }


    // ==========================================================
    // ATTACK EXECUTION
    // ==========================================================

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

        // ======================================================
        // ARCANE FOCUS
        // ======================================================

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

        // ======================================================
        // RETALIATION
        // ======================================================

        if (CharacterManager.HasPassive(
                attacker,
                "Retaliation") &&
            attacker.HitDuringPreviousTurn)
        {
            damage++;
        }

        BattleLog.WriteReplay(
            $"{attacker.Name} attacked with {attack.Name} " +
            $"({damage} Damage x {attack.Hits} Hit(s))",
            BattleLog.CurrentTurnNumber,
            BattleLog.CurrentPlayerIndex,
            BattleLogEventType.Attack,
            cardName: attack.Name,
            cardType: attack.Type,
            deckStructure: attacker.Deck.Structure,
            slotNumber:
                attacker.AttackSlot1 == attack
                    ? 1
                    : 2,
            amount: damage,
            targetPlayerIndex:
                BattleLog.GetOpponentPlayerIndex());

        int hitDamage =
            damage;

        int guardianShieldGain = 0;

        bool fortified =
            defender.CharacterNewlyDeployed &&
            !defender.EntryDefenseUsed &&
            defender.ActiveCharacter?.Name ==
                "Guardian Red Panda" &&
            defender.Shield > 0;

        for (
            int hit = 0;
            hit < attack.Hits;
            hit++)
        {
            bool landed =
                ResolveHit(
                    defender,
                    hitDamage,
                    attack.IsPiercing,
                    attack.Hits,
                    fortified,
                    attack);

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

            BattleLog.WriteReplay(
                $"{defender.Name}'s Fortified effect was consumed",
                BattleLog.CurrentTurnNumber,
                BattleLog.CurrentPlayerIndex,
                BattleLogEventType.Ability,
                targetPlayerIndex:
                    BattleLog.GetOpponentPlayerIndex());
        }

        // ======================================================
        // SHIELD RECOVERY
        // ======================================================

        if (CharacterManager.HasPassive(
                defender,
                "ShieldRecovery"))
        {
            int recovered =
                Math.Min(
                    2,
                    guardianShieldGain);

            defender.Shield +=
                recovered;

            if (recovered > 0)
            {
                BattleLog.WriteReplay(
                    $"{defender.Name} recovered {recovered} Shield",
                    BattleLog.CurrentTurnNumber,
                    BattleLog.CurrentPlayerIndex,
                    BattleLogEventType.EnergyChange,
                    targetPlayerIndex:
                        BattleLog.GetOpponentPlayerIndex(),
                    amount: recovered,
                    opponentShield: defender.Shield);
            }
        }
    }


    // ==========================================================
    // HIT RESOLUTION
    // ==========================================================

    private static bool ResolveHit(
        Player defender,
        int damage,
        bool piercing,
        int totalHits,
        bool fortified,
        Card attack)
    {
        defender.WasHitThisTurn = true;

        // ======================================================
        // SAGE DAMAGE FLOOR
        // ======================================================

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

            BattleLog.WriteReplay(
                $"{defender.Name}'s Damage Floor limited the hit to {damage} Damage",
                BattleLog.CurrentTurnNumber,
                BattleLog.CurrentPlayerIndex,
                BattleLogEventType.Ability,
                cardName: defender.ActiveCharacter.Name,
                cardType: CardType.Character,
                deckStructure: defender.Deck.Structure,
                targetPlayerIndex:
                    BattleLog.GetOpponentPlayerIndex(),
                amount: damage);
        }

        // ======================================================
        // MULTI-HIT RESISTANCE
        // ======================================================

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

        // ======================================================
        // DEFLECT NEXT HIT
        // ======================================================

        if (CharacterManager.HasStatusEffect(
                defender,
                "DeflectNextHit"))
        {
            CharacterManager.RemoveStatusEffect(
                defender,
                "DeflectNextHit");

            BattleLog.WriteReplay(
                $"{defender.Name} deflected the hit",
                BattleLog.CurrentTurnNumber,
                BattleLog.CurrentPlayerIndex,
                BattleLogEventType.Damage,
                targetPlayerIndex:
                    BattleLog.GetOpponentPlayerIndex(),
                amount: 0);

            return false;
        }

        // ======================================================
        // PIERCING
        // ======================================================

        if (piercing)
        {
            ResolvePiercingHit(
                defender,
                damage,
                attack);

            return true;
        }

        // ======================================================
        // SHIELD
        // ======================================================

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

                BattleLog.WriteReplay(
                    $"{defender.Name}'s Fortified Shield cannot fall below 1",
                    BattleLog.CurrentTurnNumber,
                    BattleLog.CurrentPlayerIndex,
                    BattleLogEventType.Damage,
                    targetPlayerIndex:
                        BattleLog.GetOpponentPlayerIndex(),
                    amount: shieldDamage,
                    opponentShield: defender.Shield);

                return true;
            }

            if (defender.Shield >= damage)
            {
                defender.Shield -=
                    damage;

                BattleLog.WriteReplay(
                    $"{defender.Name}'s Shield absorbed {damage} Damage",
                    BattleLog.CurrentTurnNumber,
                    BattleLog.CurrentPlayerIndex,
                    BattleLogEventType.Damage,
                    targetPlayerIndex:
                        BattleLog.GetOpponentPlayerIndex(),
                    amount: damage,
                    opponentShield: defender.Shield);
            }
            else
            {
                int absorbed =
                    defender.Shield;

                defender.Shield = 0;

                BattleLog.WriteReplay(
                    $"{defender.Name}'s Shield was Broken",
                    BattleLog.CurrentTurnNumber,
                    BattleLog.CurrentPlayerIndex,
                    BattleLogEventType.Damage,
                    targetPlayerIndex:
                        BattleLog.GetOpponentPlayerIndex(),
                    amount: absorbed,
                    opponentShield: 0);
            }

            return true;
        }

        DamageCharacterOrEnergy(
            defender,
            damage,
            attack);

        return true;
    }


    // ==========================================================
    // PIERCING HIT
    // ==========================================================

    private static void ResolvePiercingHit(
        Player defender,
        int damage,
        Card attack)
    {
        if (defender.Shield <= 0)
        {
            DamageCharacterOrEnergy(
                defender,
                damage,
                attack);

            return;
        }

        int standardDamage =
            damage / 2;

        int piercingDamage =
            damage - standardDamage;

        if (defender.Shield >= standardDamage)
        {
            defender.Shield -=
                standardDamage;

            BattleLog.WriteReplay(
                $"{defender.Name}'s Shield absorbed " +
                $"{standardDamage} Damage",
                BattleLog.CurrentTurnNumber,
                BattleLog.CurrentPlayerIndex,
                BattleLogEventType.Damage,
                targetPlayerIndex:
                    BattleLog.GetOpponentPlayerIndex(),
                amount: standardDamage,
                opponentShield: defender.Shield);
        }
        else
        {
            int absorbed =
                defender.Shield;

            defender.Shield = 0;

            BattleLog.WriteReplay(
                $"{defender.Name}'s Shield was Broken",
                BattleLog.CurrentTurnNumber,
                BattleLog.CurrentPlayerIndex,
                BattleLogEventType.Damage,
                targetPlayerIndex:
                    BattleLog.GetOpponentPlayerIndex(),
                amount: absorbed,
                opponentShield: 0);
        }

        DamageCharacterOrEnergy(
            defender,
            piercingDamage,
            attack);

        BattleLog.WriteReplay(
            $"Piercing dealt {piercingDamage} direct damage",
            BattleLog.CurrentTurnNumber,
            BattleLog.CurrentPlayerIndex,
            BattleLogEventType.Damage,
            cardName: attack.Name,
            cardType: attack.Type,
            deckStructure: BattleLog.GetCurrentDeckStructure(),
            targetPlayerIndex:
                BattleLog.GetOpponentPlayerIndex(),
            amount: piercingDamage);
    }


    // ==========================================================
    // DAMAGE TO CHARACTER OR ENERGY
    // ==========================================================

    private static void DamageCharacterOrEnergy(
        Player defender,
        int damage,
        Card attack)
    {
        if (defender.ActiveCharacter == null)
        {
            defender.Energy =
                Math.Max(
                    0,
                    defender.Energy - damage);

            BattleLog.WriteReplay(
                $"{defender.Name} lost {damage} Energy " +
                $"({defender.Energy} Remaining)",
                BattleLog.CurrentTurnNumber,
                BattleLog.CurrentPlayerIndex,
                BattleLogEventType.EnergyChange,
                cardName: attack.Name,
                cardType: attack.Type,
                deckStructure: BattleLog.GetCurrentDeckStructure(),
                targetPlayerIndex:
                    BattleLog.GetOpponentPlayerIndex(),
                amount: -damage,
                opponentEnergy: defender.Energy);

            return;
        }

        if (damage >= defender.CurrentCharacterHp)
        {
            int overflow =
                damage -
                defender.CurrentCharacterHp;

            string defeatedCharacter =
                defender.ActiveCharacter.Name;

            BattleLog.WriteReplay(
                $"{defender.Name}'s {defeatedCharacter} was Defeated",
                BattleLog.CurrentTurnNumber,
                BattleLog.CurrentPlayerIndex,
                BattleLogEventType.CharacterDefeated,
                cardName: defeatedCharacter,
                cardType: CardType.Character,
                deckStructure: defender.Deck.Structure,
                targetPlayerIndex:
                    BattleLog.GetOpponentPlayerIndex(),
                amount: damage,
                characterHp: 0);

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

                BattleLog.WriteReplay(
                    $"{defender.Name} took {overflow} Overflow Energy Damage " +
                    $"({defender.Energy} Energy Remaining)",
                    BattleLog.CurrentTurnNumber,
                    BattleLog.CurrentPlayerIndex,
                    BattleLogEventType.EnergyChange,
                    targetPlayerIndex:
                        BattleLog.GetOpponentPlayerIndex(),
                    amount: -overflow,
                    opponentEnergy: defender.Energy);
            }

            return;
        }

        defender.CurrentCharacterHp -=
            damage;

        BattleLog.WriteReplay(
            $"{defender.ActiveCharacter.Name} took {damage} HP Damage " +
            $"({defender.CurrentCharacterHp} HP Remaining)",
            BattleLog.CurrentTurnNumber,
            BattleLog.CurrentPlayerIndex,
            BattleLogEventType.Damage,
            cardName: attack.Name,
            cardType: attack.Type,
            deckStructure: BattleLog.GetCurrentDeckStructure(),
            targetPlayerIndex:
                BattleLog.GetOpponentPlayerIndex(),
            amount: damage,
            characterHp: defender.CurrentCharacterHp);
    }


    // ==========================================================
    // BOT
    // ==========================================================

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