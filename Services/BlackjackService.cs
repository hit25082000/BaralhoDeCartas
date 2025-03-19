using BaralhoDeCartas.Api.Interfaces;
using BaralhoDeCartas.Factory.Interfaces;
using BaralhoDeCartas.Models.Interfaces;
using BaralhoDeCartas.Services.Interfaces;

namespace BaralhoDeCartas.Services
{
    public class BlackjackService : IBlackjackService
    {
        private readonly IBaralhoApiClient _baralhoApiClient;
        private readonly IJogadorFactory _jogadorFactory;
        private const int CartasIniciaisPorJogador = 2;
     
        public BlackjackService(IBaralhoApiClient baralhoApiClient,IJogadorFactory jogadorFactory)
        {
            _baralhoApiClient = baralhoApiClient;
            _jogadorFactory = jogadorFactory;
        }

        public async Task<IBaralho> IniciarJogo()
        {
            return await _baralhoApiClient.CriarNovoBaralho();
        }

        public async Task<List<IJogadorDeBlackjack>> IniciarRodada(string baralhoId, int numeroJogadores)
        {
            var jogadores = new List<IJogadorDeBlackjack>();
            var totalCartas = numeroJogadores * CartasIniciaisPorJogador;

            var todasAsCartas = await _baralhoApiClient.ComprarCartas(baralhoId, totalCartas);
            
            for (int i = 0; i < numeroJogadores; i++)
            {
                IJogadorDeBlackjack jogador = _jogadorFactory.CriarJogadorDeBlackJack(todasAsCartas, CartasIniciaisPorJogador,i);
                jogadores.Add(jogador);
            }

            return jogadores;
        }

        public async Task<ICarta> ComprarCarta(string baralhoId, IJogadorDeBlackjack jogador)
        {
            if (jogador.Parou || jogador.Estourou)
            {
                throw new InvalidOperationException($"O jogador {jogador.Nome} não pode comprar mais cartas.");
            }

            var cartas = await _baralhoApiClient.ComprarCartas(baralhoId, 1);
            var novaCarta = cartas.FirstOrDefault();
            
            if (novaCarta != null)
            {
                jogador.Cartas.Add(novaCarta);
            }

            return novaCarta;
        }

        public List<IJogadorDeBlackjack> DeterminarVencedores(List<IJogadorDeBlackjack> jogadores)
        {
            var jogadoresValidos = jogadores.Where(j => !j.Estourou).ToList();

            if (!jogadoresValidos.Any())
            {
                return new List<IJogadorDeBlackjack>();
            }

            var jogadoresComBlackjack = jogadoresValidos.Where(j => j.TemBlackjack()).ToList();
            if (jogadoresComBlackjack.Any())
            {
                return jogadoresComBlackjack;
            }

            var maiorPontuacao = jogadoresValidos.Max(j => j.CalcularPontuacao());

            return jogadoresValidos.Where(j => j.CalcularPontuacao() == maiorPontuacao).ToList();
        }

        public async Task<bool> FinalizarJogo(string baralhoId)
        {
            return await _baralhoApiClient.RetornarCartasAoBaralho(baralhoId);
        }
    }
} 