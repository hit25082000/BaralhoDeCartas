using BaralhoDeCartas.Factory.Interfaces;
using BaralhoDeCartas.Models;
using BaralhoDeCartas.Models.ApiResponses;
using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Factory
{
    public class CartaFactory : ICartaFactory
    {
        public List<ICarta> CriarCartas(CartasResponse response)
        {
            return response.Cards
            .Select(c => (ICarta)new Carta(c.Code,c.Image,c.Value,c.Suit))
            .ToList();
        }
    }
}
