namespace RedPandaTCD_Web.Game;

public static class CardLibraryManager
{
    public static List<Card> GetAllCards()
    {
        return CardFactory.GetAllAvailableCards();
    }

public static List<Card> LinearSearch(
    List<Card> cards,
    string searchText)
{
    List<Card> results =
        new List<Card>();

    if (string.IsNullOrWhiteSpace(searchText))
    {
        return results;
    }

    for (int i = 0;
         i < cards.Count;
         i++)
    {
        bool nameMatches =
            cards[i].Name.Contains(
                searchText,
                StringComparison.OrdinalIgnoreCase);

        bool idMatches =
            cards[i].Id.Contains(
                searchText,
                StringComparison.OrdinalIgnoreCase);

        if (nameMatches ||
            idMatches)
        {
            results.Add(
                cards[i]);
        }
    }

    return results;
}

    public static List<Card> InsertionSortByCost(
        List<Card> cards)
    {
        List<Card> sortedCards =
            new List<Card>(cards);

        for (int i = 1;
             i < sortedCards.Count;
             i++)
        {
            Card currentCard =
                sortedCards[i];

            int j =
                i - 1;

            while (j >= 0 &&
                   sortedCards[j].Cost >
                   currentCard.Cost)
            {
                sortedCards[j + 1] =
                    sortedCards[j];

                j--;
            }

            sortedCards[j + 1] =
                currentCard;
        }

        return sortedCards;
    }
}