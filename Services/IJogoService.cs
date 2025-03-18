using BaralhoDeCartas.Models;

namespace BaralhoDeCartas.Services
{
    public interface IJogoService
    {
        Task<Baralho> IniciarNovoJogo();
        Task<List<Jogador>> DistribuirCartas(string deckId, int numeroJogadores);
        Task<Jogador> DeterminarVencedor(List<Jogador> jogadores);
        Task<bool> FinalizarJogo(string deckId);
    }
} 