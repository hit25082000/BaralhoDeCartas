using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Models
{
    public class Baralho : IBaralho
    {
        public Baralho()
        {
            FoiEmbaralhado = true;
        }

        public string BaralhoId { get; set; }
        public bool FoiEmbaralhado { get; set; }
        public int QuantidadeDeCartasRestantes { get; set; }
    }
}
