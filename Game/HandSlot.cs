namespace RedPandaTCD_Web.Game;

public class HandSlot
{
    public int SlotNumber { get; set; }

    public Card? CardInSlot { get; set; }

    public bool IsEmpty =>
        CardInSlot == null;

    public HandSlot(int slotNumber)
    {
        SlotNumber = slotNumber;
        CardInSlot = null;
    }
}