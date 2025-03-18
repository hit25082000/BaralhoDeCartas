using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Models
{
    public class Carta : ICarta
    {
        public string Codigo { get; set; }
        public string ImagemUrl { get; set; }
        public string ValorSimbolico { get; set; }
        public string Naipe { get; set; }

        public int ValorNumerico
        {
            get
            {
                return ValorSimbolico switch
                {
                    "ACE" => 14,
                    "KING" => 13,
                    "QUEEN" => 12,
                    "JACK" => 11,
                    _ => int.TryParse(ValorSimbolico, out int valor) ? valor : 0
                };
            }
        }

        public int ValorBlackjack
        {
            get
            {
                return ValorSimbolico switch
                {
                    "ACE" => 11, // Ás pode valer 1 ou 11
                    "KING" => 10,
                    "QUEEN" => 10,
                    "JACK" => 10,
                    _ => int.TryParse(ValorSimbolico, out int valor) ? valor : 0
                };
            }
        }
    }
}
