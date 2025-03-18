using BaralhoDeCartas.Models;

namespace BaralhoDeCartas.Services
{
    public interface IBlackjackService
    {
        Task<Baralho> IniciarJogo();
        Task<List<Jogador>> IniciarRodada(string deckId, int numeroJogadores);
        Task<Carta> ComprarCarta(string deckId, Jogador jogador);
        Task<bool> FinalizarJogo(string deckId);
        List<Jogador> DeterminarVencedores(List<Jogador> jogadores);
    }
} 