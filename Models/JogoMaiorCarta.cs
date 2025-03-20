using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Models
{
    public class JogoMaiorCarta : IJogoMaiorCarta
    {
        public JogoMaiorCarta()
        {
            Jogadores = new List<IJogador>();
        }
        public List<IJogador> Jogadores { get; set; }
        public IBaralho Baralho { get; set; }
    }    
}
