using BaralhoDeCartas.Models;

namespace BaralhoDeCartas.Api
{
    public interface IBaralhoApiClient
    {
        Task<Baralho> CriarNovoBaralho();
        Task<List<Carta>> ComprarCartas(string deckId, int quantidade);
        Task<bool> RetornarCartasAoBaralho(string deckId);
    }
} 