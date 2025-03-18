using BaralhoDeCartas.Models;
using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Services
{
    public interface IBlackjackService
    {
        Task<IBaralho> IniciarJogo();
        Task<List<IJogadorDeBlackjack>> IniciarRodada(string deckId, int numeroJogadores);
        Task<ICarta> ComprarCarta(string deckId, IJogadorDeBlackjack jogador);
        Task<bool> FinalizarJogo(string deckId);
        List<IJogadorDeBlackjack> DeterminarVencedores(List<IJogadorDeBlackjack> jogadores);
    }
} 