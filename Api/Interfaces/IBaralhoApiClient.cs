using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Api.Interfaces
{
    public interface IBaralhoApiClient
    {
        Task<IBaralho> CriarNovoBaralho();
        Task<IBaralho> EmbaralharBaralho(string baralhoId, bool embaralharSomenteCartasRestantes);
        Task<List<ICarta>> ComprarCartas(string baralhoId, int quantidade);
        Task<bool> RetornarCartasAoBaralho(string baralhoId);
    }
} 