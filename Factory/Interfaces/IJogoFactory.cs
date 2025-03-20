using BaralhoDeCartas.Factory.Interfaces;
using BaralhoDeCartas.Models;
using BaralhoDeCartas.Models.ApiResponses;
using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Factory
{
    public interface IJogoFactory 
    {
        IJogoMaiorCarta CriarJogoMaiorCarta(IBaralho baralho);
        IJogoBlackJack CriarJogoBlackJack(IBaralho baralho);
    }
}
