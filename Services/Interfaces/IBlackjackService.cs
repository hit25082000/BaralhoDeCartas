using BaralhoDeCartas.Models;
using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Services.Interfaces
{
    public interface IBlackjackService
    {
        Task<IBaralho> IniciarJogo();
        Task<List<IJogadorDeBlackjack>> IniciarRodada(string baralhoId, int numeroJogadores);
        Task<ICarta> ComprarCarta(string baralhoId, IJogadorDeBlackjack jogador);
        Task<bool> FinalizarJogo(string baralhoId);
        List<IJogadorDeBlackjack> DeterminarVencedores(List<IJogadorDeBlackjack> jogadores);
    }
} 