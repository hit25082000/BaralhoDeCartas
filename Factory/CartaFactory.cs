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
            .Select(c => (ICarta)new Carta
            {
                Codigo = c.Code,
                Naipe = c.Suit,
                ValorSimbolico = c.Value,
                ImagemUrl = c.Image
            })
            .ToList();
        }
    }
}
