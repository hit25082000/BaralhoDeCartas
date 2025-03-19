using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Services.Interfaces
{
    public interface IJogoService
    {
        Task<IBaralho> IniciarNovoJogo();
        Task<List<IJogador>> DistribuirCartas(string baralhoId, int numeroJogadores);
        Task<IJogador> DeterminarVencedor(List<IJogador> jogadores);
        Task<bool> FinalizarJogo(string baralhoId);
    }
} 