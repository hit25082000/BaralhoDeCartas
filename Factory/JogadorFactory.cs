using BaralhoDeCartas.Factory.Interfaces;
using BaralhoDeCartas.Models;
using BaralhoDeCartas.Models.ApiResponses;
using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Factory
{
    public class JogadorFactory : IJogadorFactory
    {
        public IJogador CriarJogador(List<ICarta> cartas, int cartasIniciaisPorJogador, int indice)
        {
            return new Jogador
            {
                JogadorId = indice + 1,
                Nome = $"Jogador {indice + 1}",
                Cartas = cartas.Skip(indice * cartasIniciaisPorJogador).Take(cartasIniciaisPorJogador).ToList()
            };
        }

        public IJogadorDeBlackjack CriarJogadorDeBlackJack(List<ICarta> cartas, int cartasIniciaisPorJogador,int indice)
        {
            return new JogadorDeBlackjack
            {
                JogadorId = indice + 1,
                Nome = $"Jogador {indice + 1}",
                Cartas = cartas
                          .Skip(indice * cartasIniciaisPorJogador)
                          .Take(cartasIniciaisPorJogador)
                          .ToList()
            };
        }
    }
}
