namespace RedPandaTCD_Web.Game;

public static class StarterDeckFactory
{
    public static Deck CreateQueueStarter()
    {
        Deck deck = new Deck
        {
            Name = "Blueprint Queue",
            Structure = DeckStructure.Queue
        };

        deck.Cards.Add(CardFactory.CreateSageRedPanda());
        deck.Cards.Add(CardFactory.CreateManaPulses());
        deck.Cards.Add(CardFactory.CreateEnergyPotion());
        deck.Cards.Add(CardFactory.CreateIceShards());
        deck.Cards.Add(CardFactory.CreateFireball());
        deck.Cards.Add(CardFactory.CreateSorcererRedPanda());
        deck.Cards.Add(CardFactory.CreateArcaneBullets());
        deck.Cards.Add(CardFactory.CreateMysticSpike());
        deck.Cards.Add(CardFactory.CreateManaBurst());
        deck.Cards.Add(CardFactory.CreateEnergyDrain());

        return deck;
    }

    public static Deck CreateStackStarter()
    {
        Deck deck = new Deck
        {
            Name = "The Stack Has Decided",
            Structure = DeckStructure.Stack
        };

        deck.Cards.Add(CardFactory.CreateBerserkerRedPanda());
        deck.Cards.Add(CardFactory.CreateSlash());
        deck.Cards.Add(CardFactory.CreateDoubleClaw());
        deck.Cards.Add(CardFactory.CreateDoubleThrust());
        deck.Cards.Add(CardFactory.CreatePiercingStrike());
        deck.Cards.Add(CardFactory.CreateHeavyStrike());
        deck.Cards.Add(CardFactory.CreateRampage());
        deck.Cards.Add(CardFactory.CreateAttackAmplifier());
        deck.Cards.Add(CardFactory.CreateDefensiveStance());
        deck.Cards.Add(CardFactory.CreateGuardianRedPanda());

        return deck;
    }

    public static Deck CreatePriorityStarter()
    {
        Deck deck = new Deck
        {
            Name = "Priority Protocol",
            Structure = DeckStructure.PriorityQueue
        };

        Card sorcerer =
            CardFactory.CreateSorcererRedPanda();
        sorcerer.Priority = 5;

        Card wanderer =
            CardFactory.CreateWandererRedPanda();
        wanderer.Priority = 5;

        Card fireball =
            CardFactory.CreateFireball();
        fireball.Priority = 4;

        Card manaBurst =
            CardFactory.CreateManaBurst();
        manaBurst.Priority = 4;

        Card guardBreak =
            CardFactory.CreateGuardBreak();
        guardBreak.Priority = 3;

        Card mysticSpike =
            CardFactory.CreateMysticSpike();
        mysticSpike.Priority = 3;

        Card pounce =
            CardFactory.CreatePounce();
        pounce.Priority = 2;

        Card chainLightning =
            CardFactory.CreateChainLightning();
        chainLightning.Priority = 2;

        Card shieldBooster =
            CardFactory.CreateShieldBooster();
        shieldBooster.Priority = 1;

        Card discountCoupon =
            CardFactory.CreateDiscountCoupon();
        discountCoupon.Priority = 1;

        deck.Cards.Add(sorcerer);
        deck.Cards.Add(wanderer);
        deck.Cards.Add(fireball);
        deck.Cards.Add(manaBurst);
        deck.Cards.Add(guardBreak);
        deck.Cards.Add(mysticSpike);
        deck.Cards.Add(pounce);
        deck.Cards.Add(chainLightning);
        deck.Cards.Add(shieldBooster);
        deck.Cards.Add(discountCoupon);

        return deck;
    }

    public static Deck CreateRandomStarter()
    {
        Deck deck = new Deck
        {
            Name = "Chaos Draw",
            Structure = DeckStructure.RandomList
        };

        deck.Cards.Add(CardFactory.CreateNomadRedPanda());
        deck.Cards.Add(CardFactory.CreateGuardianRedPanda());
        deck.Cards.Add(CardFactory.CreateFireball());
        deck.Cards.Add(CardFactory.CreateSlash());
        deck.Cards.Add(CardFactory.CreatePounce());
        deck.Cards.Add(CardFactory.CreateIceShards());
        deck.Cards.Add(CardFactory.CreateChainLightning());
        deck.Cards.Add(CardFactory.CreateDoubleClaw());
        deck.Cards.Add(CardFactory.CreateShieldBooster());
        deck.Cards.Add(CardFactory.CreateFortify());

        return deck;
    }

    public static Deck CreateLinkedStarter()
    {
        Deck deck = new Deck
        {
            Name = "Deck Engineer",
            Structure = DeckStructure.LinkedList
        };

        deck.Cards.Add(CardFactory.CreateWandererRedPanda());
        deck.Cards.Add(CardFactory.CreateNomadRedPanda());
        deck.Cards.Add(CardFactory.CreateRampage());
        deck.Cards.Add(CardFactory.CreateFireball());
        deck.Cards.Add(CardFactory.CreateManaBurst());
        deck.Cards.Add(CardFactory.CreateGuardBreak());
        deck.Cards.Add(CardFactory.CreateFrostNova());
        deck.Cards.Add(CardFactory.CreateMysticSpike());
        deck.Cards.Add(CardFactory.CreateDiscountCoupon());
        deck.Cards.Add(CardFactory.CreateDiscardForEnergy());

        return deck;
    }
}