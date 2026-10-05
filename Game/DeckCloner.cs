namespace RedPandaTCD_Web.Game;

public static class DeckCloner
{
    public static Deck Clone(Deck deck)
    {
        return new Deck
        {
            Name = deck.Name,
            Structure = deck.Structure,
            Cards = deck.Cards
                .Select(c => c.Clone())
                .ToList()
        };
    }
}