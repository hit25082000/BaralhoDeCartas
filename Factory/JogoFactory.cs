using BaralhoDeCartas.Factory.Interfaces;
using BaralhoDeCartas.Models;
using BaralhoDeCartas.Models.ApiResponses;
using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Factory
{
    public class JogoFactory : IJogoFactory
    {
        public IJogoMaiorCarta CriarJogoMaiorCarta(IBaralho baralho)
        {
            return new JogoMaiorCarta
            {
                Baralho = baralho
            };           
        }

        public IJogoBlackJack CriarJogoBlackJack(IBaralho baralho)
        {
            return new JogoBlackJack
            {
                Baralho = baralho
            };
        }
    }
}
