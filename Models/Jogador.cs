using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Models
{
    public class Jogador : IJogador
    {
        public int JogadorId { get; set; }
        public string Nome { get; set; }
        public List<ICarta> Cartas { get; set; }
        public ICarta ObterCartaMaisAlta()
        {
            return Cartas.OrderByDescending(c => c.ValorNumerico).FirstOrDefault();
        }
    }
}
