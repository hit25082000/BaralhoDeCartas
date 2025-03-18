namespace BaralhoDeCartas.Models.ApiResponses
{
    public class CartasResponse
    {
        public bool Success { get; set; }
        public string Deck_id { get; set; }
        public List<CartaListResponse> Cards { get; set; }
        public int Remaining { get; set; }
    }

    public class CartaListResponse
    {
        public string Image { get; set; }
        public string Value { get; set; }
        public string Suit { get; set; }
        public string Code { get; set; }
    }
}
