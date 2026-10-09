namespace RedPandaTCD_Web.Game;

public static class CardFactory
{
    // ==========================
    // CHARACTERS
    // ==========================

    public static Card CreateSageRedPanda() => new Card
    {
        Id = "CM1",
        Name = "Sage Red Panda",
        Type = CardType.Character,
        Archetype = Archetype.Magic,
        Cost = 5,
        HpValue = 12,
        ShieldValue = 7,
        ActiveAbility = "ShieldGain",
        AbilityCooldown = 1,
        PassiveAbilities = new List<string>
        {
            "MultiHitResistance"
        },
        Description =
            "Passive: Reduce multi-hit damage by 1 per hit. " +
            "Active: Gain 1 Shield (Cost 2, CD 1)."
    };

    public static Card CreateSorcererRedPanda() => new Card
    {
        Id = "CM2",
        Name = "Sorcerer Red Panda",
        Type = CardType.Character,
        Archetype = Archetype.Magic,
        Cost = 5,
        HpValue = 12,
        ShieldValue = 5,
        ActiveAbility = "MagicBoost",
        AbilityCooldown = 1,
        PassiveAbilities = new List<string>
        {
            "ArcaneFocus"
        },
        Description =
            "Passive: Magic attacks gain +1 damage if no Physical attacks are deployed. " +
            "Active: Next attack gains +1 Damage (Cost 1, CD 1)."
    };

    public static Card CreateGuardianRedPanda() => new Card
    {
        Id = "CP1",
        Name = "Guardian Red Panda",
        Type = CardType.Character,
        Archetype = Archetype.Physical,
        Cost = 5,
        HpValue = 12,
        ShieldValue = 7,
        ActiveAbility = "FieldShield",
        AbilityCooldown = 2,
        PassiveAbilities = new List<string>
        {
            "ShieldRecovery"
        },
        Description =
            "Passive: Recover Shield equal to successful hits taken (max 2). " +
            "Active: Gain Shield equal to deployed Attacks (Cost 3, CD 2)."
    };

    public static Card CreateBerserkerRedPanda() => new Card
    {
        Id = "CP2",
        Name = "Berserker Red Panda",
        Type = CardType.Character,
        Archetype = Archetype.Physical,
        Cost = 5,
        HpValue = 12,
        ShieldValue = 5,
        ActiveAbility = "PowerSurge",
        AbilityCooldown = 2,
        PassiveAbilities = new List<string>
        {
            "Retaliation"
        },
        Description =
            "Passive: +1 Damage if hit during the previous turn. " +
            "Active: Next attack gains +2 Damage (Cost 3, CD 2)."
    };

    public static Card CreateWandererRedPanda() => new Card
    {
        Id = "CN1",
        Name = "Wanderer Red Panda",
        Type = CardType.Character,
        Archetype = Archetype.Neutral,
        Cost = 5,
        HpValue = 12,
        ShieldValue = 6,
        ActiveAbility = "AttackBoost",
        AbilityCooldown = 1,
        PassiveAbilities = new List<string>
        {
            "FreePeek"
        },
        Description =
            "Passive: Peek actions do not consume the free-peek limit. " +
            "Active: Next attack gains +1 Damage (Cost 2, CD 1)."
    };

    public static Card CreateNomadRedPanda() => new Card
    {
        Id = "CN2",
        Name = "Nomad Red Panda",
        Type = CardType.Character,
        Archetype = Archetype.Neutral,
        Cost = 5,
        HpValue = 12,
        ShieldValue = 6,
        ActiveAbility = "Heal",
        AbilityCooldown = 2,
        PassiveAbilities = new List<string>
        {
            "EndTurnShield"
        },
        Description =
            "Passive: Gain +1 additional Shield during End Phase. " +
            "Active: Restore 1 HP (Cost 2, CD 2)."
    };

    // ==========================
    // MAGIC ATTACKS
    // ==========================

    public static Card CreateManaPulses() => new Card
    {
        Id = "AM1",
        Name = "Mana Pulses",
        Type = CardType.Attack,
        Archetype = Archetype.Magic,
        Cost = 2,
        Damage = 1,
        Hits = 2
    };

    public static Card CreateArcaneBullets() => new Card
    {
        Id = "AM2",
        Name = "Arcane Bullets",
        Type = CardType.Attack,
        Archetype = Archetype.Magic,
        Cost = 2,
        Damage = 1,
        Hits = 2
    };

    public static Card CreateIceShards() => new Card
    {
        Id = "AM3",
        Name = "Ice Shards",
        Type = CardType.Attack,
        Archetype = Archetype.Magic,
        Cost = 4,
        Damage = 2,
        Hits = 2
    };

    public static Card CreateMysticSpike() => new Card
    {
        Id = "AM4",
        Name = "Mystic Spike",
        Type = CardType.Attack,
        Archetype = Archetype.Magic,
        Cost = 3,
        Damage = 3
    };

    public static Card CreateManaBurst() => new Card
    {
        Id = "AM5",
        Name = "Mana Burst",
        Type = CardType.Attack,
        Archetype = Archetype.Magic,
        Cost = 7,
        Damage = 3,
        Hits = 2
    };

    public static Card CreateFireball() => new Card
    {
        Id = "AM6",
        Name = "Fireball",
        Type = CardType.Attack,
        Archetype = Archetype.Magic,
        Cost = 5,
        Damage = 6
    };

    // ==========================
    // PHYSICAL ATTACKS
    // ==========================

    public static Card CreateSlash() => new Card
    {
        Id = "AP1",
        Name = "Slash",
        Type = CardType.Attack,
        Archetype = Archetype.Physical,
        Cost = 2,
        Damage = 2
    };

    public static Card CreateDoubleClaw() => new Card
    {
        Id = "AP2",
        Name = "Double Claw",
        Type = CardType.Attack,
        Archetype = Archetype.Physical,
        Cost = 2,
        Damage = 1,
        Hits = 2
    };

    public static Card CreateDoubleThrust() => new Card
    {
        Id = "AP3",
        Name = "Double Thrust",
        Type = CardType.Attack,
        Archetype = Archetype.Physical,
        Cost = 4,
        Damage = 2,
        Hits = 2,
        IsPiercing = true
    };

    public static Card CreatePiercingStrike() => new Card
    {
        Id = "AP4",
        Name = "Piercing Strike",
        Type = CardType.Attack,
        Archetype = Archetype.Physical,
        Cost = 4,
        Damage = 4,
        IsPiercing = true
    };

    public static Card CreateHeavyStrike() => new Card
    {
        Id = "AP5",
        Name = "Heavy Strike",
        Type = CardType.Attack,
        Archetype = Archetype.Physical,
        Cost = 5,
        Damage = 6
    };

    public static Card CreateRampage() => new Card
    {
        Id = "AP6",
        Name = "Rampage",
        Type = CardType.Attack,
        Archetype = Archetype.Physical,
        Cost = 7,
        Damage = 3,
        Hits = 2,
        InstantFatigue = true
    };

    // ==========================
    // NEUTRAL ATTACKS
    // ==========================

    public static Card CreatePounce() => new Card
    {
        Id = "AN1",
        Name = "Pounce",
        Type = CardType.Attack,
        Archetype = Archetype.Neutral,
        Cost = 2,
        Damage = 2
    };

    public static Card CreateChainLightning() => new Card
    {
        Id = "AN2",
        Name = "Chain Lightning",
        Type = CardType.Attack,
        Archetype = Archetype.Neutral,
        Cost = 2,
        Damage = 1,
        Hits = 2
    };

    public static Card CreateGuardBreak() => new Card
    {
        Id = "AN3",
        Name = "Guard Break",
        Type = CardType.Attack,
        Archetype = Archetype.Neutral,
        Cost = 6,
        Damage = 6,
        IsPiercing = true
    };

    public static Card CreateFrostNova() => new Card
    {
        Id = "AN4",
        Name = "Frost Nova",
        Type = CardType.Attack,
        Archetype = Archetype.Neutral,
        Cost = 5,
        Damage = 7,
        InstantFatigue = true
    };

    // ==========================
    // UTILITIES
    // ==========================

    public static Card CreateEnergyPotion() => new Card
    {
        Id = "UM1",
        Name = "Energy Potion",
        Type = CardType.Utility,
        Archetype = Archetype.Magic,
        Cost = 0
    };

    public static Card CreateEnergyDrain() => new Card
    {
        Id = "UM2",
        Name = "Energy Drain",
        Type = CardType.Utility,
        Archetype = Archetype.Magic,
        Cost = 2
    };

    public static Card CreateAttackAmplifier() => new Card
    {
        Id = "UP1",
        Name = "Attack Amplifier",
        Type = CardType.Utility,
        Archetype = Archetype.Physical,
        Cost = 1
    };

    public static Card CreateDefensiveStance() => new Card
    {
        Id = "UP2",
        Name = "Defensive Stance",
        Type = CardType.Utility,
        Archetype = Archetype.Physical,
        Cost = 2
    };

    public static Card CreateShieldBooster() => new Card
    {
        Id = "UN1",
        Name = "Shield Booster",
        Type = CardType.Utility,
        Archetype = Archetype.Neutral,
        Cost = 1
    };

    public static Card CreateFortify() => new Card
    {
        Id = "UN2",
        Name = "Fortify",
        Type = CardType.Utility,
        Archetype = Archetype.Neutral,
        Cost = 2
    };

    public static Card CreateDiscountCoupon() => new Card
    {
        Id = "UN3",
        Name = "Discount Coupon",
        Type = CardType.Utility,
        Archetype = Archetype.Neutral,
        Cost = 0
    };

    public static Card CreateDiscardForEnergy() => new Card
    {
        Id = "UN4",
        Name = "Discard For Energy",
        Type = CardType.Utility,
        Archetype = Archetype.Neutral,
        Cost = 0
    };

    // ==========================
    // COMPLETE CARD POOL
    // ==========================

    public static List<Card> GetAllAvailableCards()
    {
        return new List<Card>
        {
            CreateSageRedPanda(),
            CreateSorcererRedPanda(),
            CreateGuardianRedPanda(),
            CreateBerserkerRedPanda(),
            CreateWandererRedPanda(),
            CreateNomadRedPanda(),

            CreateManaPulses(),
            CreateArcaneBullets(),
            CreateIceShards(),
            CreateMysticSpike(),
            CreateManaBurst(),
            CreateFireball(),

            CreateSlash(),
            CreateDoubleClaw(),
            CreateDoubleThrust(),
            CreatePiercingStrike(),
            CreateHeavyStrike(),
            CreateRampage(),

            CreatePounce(),
            CreateChainLightning(),
            CreateGuardBreak(),
            CreateFrostNova(),

            CreateEnergyPotion(),
            CreateEnergyDrain(),
            CreateAttackAmplifier(),
            CreateDefensiveStance(),
            CreateShieldBooster(),
            CreateFortify(),
            CreateDiscountCoupon(),
            CreateDiscardForEnergy()
        };
    }
}