using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Models
{
    public class JogoBlackJack : IJogoBlackJack
    {
        public JogoBlackJack()
        {
            Jogadores = new List<IJogadorDeBlackjack>();
        }
        public List<IJogadorDeBlackjack> Jogadores { get; set; }
        public required IBaralho Baralho { get; set; }
        public IJogadorDeBlackjack JogadorAtual { get; set; }
    }
}
