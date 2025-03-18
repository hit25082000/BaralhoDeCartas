using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Models
{
    public class JogadorDeBlackjack : IJogadorDeBlackjack
    {
        public JogadorDeBlackjack()
        {
            Parou = false;
            Cartas = new List<ICarta>();
        }

        public int JogadorId { get; set; }
        public string Nome { get; set; }
        public List<ICarta> Cartas { get; set; }
        public bool Estourou => CalcularPontuacao() > 21;
        public bool Parou { get; set; }

        public ICarta ObterCartaMaisAlta()
        {
            return Cartas.OrderByDescending(c => c.ValorNumerico).FirstOrDefault();
        }  

        public bool TemBlackjack()
        {
            return Cartas.Count == 2 && CalcularPontuacao() == 21;
        }

        public int CalcularPontuacao()
        {
            var ases = Cartas.Count(c => c.ValorSimbolico == "ACE");
            var pontuacao = Cartas.Sum(c => c.ValorBlackjack);

            while (pontuacao > 21 && ases > 0)
            {
                pontuacao -= 10;
                ases--;
            }

            return pontuacao;
        }
    }
}
