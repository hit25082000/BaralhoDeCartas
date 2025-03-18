namespace BaralhoDeCartas.Models.ApiResponses
{
    public class BaralhoResponse
    {
        public bool Success { get; set; }
        public string Deck_id { get; set; }
        public bool Shuffled { get; set; }
        public int Remaining { get; set; }
    }
}
