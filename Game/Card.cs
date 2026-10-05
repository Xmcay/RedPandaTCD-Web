namespace RedPandaTCD_Web.Game;

public class Card
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";

    public CardType Type { get; set; }
    public Archetype Archetype { get; set; }

    public int Cost { get; set; }

    public int Damage { get; set; }
    public int Hits { get; set; } = 1;

    public int ShieldValue { get; set; }
    public int HpValue { get; set; }

    public bool IsPiercing { get; set; }
    public bool InstantFatigue { get; set; }

    public string Description { get; set; } = "";
    public string ActiveAbility { get; set; } = "";

    public List<string> PassiveAbilities { get; set; } =
        new List<string>();

    public int Priority { get; set; } = 0;

    public int AbilityCooldown { get; set; } = 1;

    public Card Clone()
    {
        return new Card
        {
            Id = Id,
            Name = Name,
            Type = Type,
            Archetype = Archetype,
            Cost = Cost,
            Damage = Damage,
            Hits = Hits,
            ShieldValue = ShieldValue,
            HpValue = HpValue,
            IsPiercing = IsPiercing,
            InstantFatigue = InstantFatigue,
            Description = Description,
            ActiveAbility = ActiveAbility,
            PassiveAbilities =
                new List<string>(PassiveAbilities),
            Priority = Priority,
            AbilityCooldown = AbilityCooldown
        };
    }
}