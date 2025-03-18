namespace BaralhoDeCartas.Models
{
    public class Baralho
    {
        public string DeckId { get; set; }
        public bool Shuffled { get; set; }
        public int Remaining { get; set; }

        public Baralho()
        {
            Shuffled = true;
        }
    }

    public class BaralhoResponse
    {
        public bool Success { get; set; }
        public string Deck_id { get; set; }
        public bool Shuffled { get; set; }
        public int Remaining { get; set; }
    }

    public class CartasResponse
    {
        public bool Success { get; set; }
        public string Deck_id { get; set; }
        public List<Carta> Cards { get; set; }
        public int Remaining { get; set; }
    }
}
