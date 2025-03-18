namespace BaralhoDeCartas.Models
{
    public class Carta
    {
        public string Code { get; set; }
        public string Image { get; set; }
        public string Value { get; set; }
        public string Suit { get; set; }

        public int ValorNumerico
        {
            get
            {
                return Value switch
                {
                    "ACE" => 14,
                    "KING" => 13,
                    "QUEEN" => 12,
                    "JACK" => 11,
                    _ => int.TryParse(Value, out int valor) ? valor : 0
                };
            }
        }

        public int ValorBlackjack
        {
            get
            {
                return Value switch
                {
                    "ACE" => 11, // Ás pode valer 1 ou 11
                    "KING" => 10,
                    "QUEEN" => 10,
                    "JACK" => 10,
                    _ => int.TryParse(Value, out int valor) ? valor : 0
                };
            }
        }
    }
}
