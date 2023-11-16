namespace TextRPGBackend.Models
{
    public class Player
    {
        public string PlayerId { get; set; }
        public int Health { get; set; }
        public int Mana { get; set; }
    }

    public class Item
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class Spell
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ManaCost { get; set; }
    }
}
