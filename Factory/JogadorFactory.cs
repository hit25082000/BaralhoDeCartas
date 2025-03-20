using BaralhoDeCartas.Factory.Interfaces;
using BaralhoDeCartas.Models;
using BaralhoDeCartas.Models.ApiResponses;
using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Factory
{
    public class JogadorFactory : IJogadorFactory
    {
        public IJogador CriarJogador(List<ICarta> cartas, int jogadorId, string nomeJogador)
        {
            return new Jogador(cartas, jogadorId, nomeJogador);
        }

        public IJogadorDeBlackjack CriarJogadorDeBlackJack(List<ICarta> cartas, int jogadorId, string nomeJogador)
        {     
            return new JogadorDeBlackjack(cartas, jogadorId, nomeJogador);
        }
    }
}
