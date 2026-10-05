namespace RedPandaTCD_Web.Game;

public static class UtilityInfo
{
    public static string GetDescription(
        Card card)
    {
        return card.Name switch
        {
            "Energy Potion" =>
                "Restore 3 Energy, up to your maximum Energy.",

            "Energy Drain" =>
                "Drain 2 Energy from the opponent and restore 2 of your Energy. The opponent cannot be reduced below 1 Energy by this effect.",

            "Attack Amplifier" =>
                "Your Attacks gain +1 Damage.",

            "Defensive Stance" =>
                "Deflect the next incoming hit.",

            "Shield Booster" =>
                "Gain 2 Shield.",

            "Fortify" =>
                "Gain 1 Shield for each Attack currently deployed.",

            "Discount Coupon" =>
                "Your next card placement costs 1 less Energy.",

            "Discard For Energy" =>
                "Restore 2 Energy.",

            _ =>
                "No effect description available."
        };
    }
}