namespace RedPandaTCD_Web.Game;

public static class CharacterInfo
{
    public static string GetEntryName(
        Card character)
    {
        return character.Name switch
        {
            "Sage Red Panda" =>
                "Damage Floor",

            "Sorcerer Red Panda" =>
                "Magic Surge",

            "Guardian Red Panda" =>
                "Fortified",

            "Berserker Red Panda" =>
                "Physical Surge",

            "Wanderer Red Panda" =>
                "Adapt",

            "Nomad Red Panda" =>
                "Quick Setup",

            _ =>
                "Entry Effect"
        };
    }

    public static string GetEntryDescription(
        Card character)
    {
        return character.Name switch
        {
            "Sage Red Panda" =>
                "The next incoming hit this turn is limited to 2 Damage.",

            "Sorcerer Red Panda" =>
                "Magic Attacks gain +1 Damage during the deployment turn.",

            "Guardian Red Panda" =>
                "Shield cannot fall below 1 during the first incoming Attack while Fortified is active.",

            "Berserker Red Panda" =>
                "Physical Attacks gain +1 Damage during the deployment turn.",

            "Wanderer Red Panda" =>
                "Choose one deployed Attack and reduce its buildup cost by 2.",

            "Nomad Red Panda" =>
                "Reduce the buildup cost of both deployed Attacks by 1.",

            _ =>
                "Applies when this Character enters the battlefield."
        };
    }

    public static string GetPassiveName(
        Card character)
    {
        string? passive =
            character.PassiveAbilities
                .FirstOrDefault();

        return passive switch
        {
            "MultiHitResistance" =>
                "Multi-Hit Resist",

            "ArcaneFocus" =>
                "Arcane Focus",

            "ShieldRecovery" =>
                "Shield Recovery",

            "Retaliation" =>
                "Retaliation",

            "FreePeek" =>
                "Free Peek",

            "EndTurnShield" =>
                "End Shield",

            _ =>
                passive ?? "None"
        };
    }

    public static string GetPassiveDescription(
        Card character)
    {
        string? passive =
            character.PassiveAbilities
                .FirstOrDefault();

        return passive switch
        {
            "MultiHitResistance" =>
                "Reduce each hit of a multi-hit Attack by 1 Damage, to a minimum of 1.",

            "ArcaneFocus" =>
                "Magic Attacks gain +1 Damage while no Physical Attack is deployed.",

            "ShieldRecovery" =>
                "Recover Shield after successful incoming hits, up to the effect's limit.",

            "Retaliation" =>
                "Attacks gain +1 Damage if this player was hit during the previous turn.",

            "FreePeek" =>
                "Peek-style deck actions do not consume a Utility Action.",

            "EndTurnShield" =>
                "Gain 1 additional Shield during End Phase.",

            _ =>
                "No passive effect."
        };
    }

    public static string GetActiveName(
        Card character)
    {
        return character.ActiveAbility switch
        {
            "ShieldGain" =>
                "Shield Gain",

            "MagicBoost" =>
                "Magic Boost",

            "FieldShield" =>
                "Field Shield",

            "PowerSurge" =>
                "Power Surge",

            "AttackBoost" =>
                "Attack Boost",

            "Heal" =>
                "Heal",

            _ =>
                character.ActiveAbility
        };
    }

    public static string GetActiveDescription(
        Card character)
    {
        return character.ActiveAbility switch
        {
            "ShieldGain" =>
                "Gain 1 Shield.",

            "MagicBoost" =>
                "The next Attack gains +1 Damage.",

            "FieldShield" =>
                "Gain Shield equal to the number of deployed Attacks.",

            "PowerSurge" =>
                "The next Attack gains +2 Damage.",

            "AttackBoost" =>
                "The next Attack gains +1 Damage.",

            "Heal" =>
                "Restore 1 HP, up to the Character's maximum HP.",

            _ =>
                "Use this Character's active ability."
        };
    }

    public static int GetActiveCost(
        Card character)
    {
        return character.ActiveAbility switch
        {
            "ShieldGain" => 2,
            "MagicBoost" => 1,
            "FieldShield" => 3,
            "PowerSurge" => 3,
            "AttackBoost" => 2,
            "Heal" => 2,

            _ => 0
        };
    }
}