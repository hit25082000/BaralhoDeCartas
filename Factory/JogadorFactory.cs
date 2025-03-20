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
            int jogadorId = indice + 1;
            string nomeJogador = $"Jogador {indice + 1}";

            return new Jogador(cartas, jogadorId, nomeJogador);
        }

        public IJogadorDeBlackjack CriarJogadorDeBlackJack(List<ICarta> cartas, int cartasIniciaisPorJogador,int indice)
        {
            return new JogadorDeBlackjack(cartas, jogadorId, nomeJogador);
        }
    }
}
