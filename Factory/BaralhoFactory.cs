using BaralhoDeCartas.Factory.Interfaces;
using BaralhoDeCartas.Models;
using BaralhoDeCartas.Models.ApiResponses;
using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Factory
{
    public class BaralhoFactory : IBaralhoFactory
    {
        public IBaralho CriarBaralho(BaralhoResponse response)
        {
            return new Baralho
            {
                BaralhoId = response.Deck_id,
                FoiEmbaralhado = response.Shuffled,
                QuantidadeDeCartasRestantes = response.Remaining
            };
        }
    }
}
