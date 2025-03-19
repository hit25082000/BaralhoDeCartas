using BaralhoDeCartas.Models.ApiResponses;
using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Factory.Interfaces
{
    public interface IJogadorFactory
    {
        IJogadorDeBlackjack CriarJogadorDeBlackJack(List<ICarta> cartas, int cartasIniciaisPorJogador, int indice);
        IJogador CriarJogador(List<ICarta> cartas, int cartasIniciaisPorJogador, int indice);
    }
}
