using BaralhoDeCartas.Api.Interfaces;
using BaralhoDeCartas.Factory;
using BaralhoDeCartas.Factory.Interfaces;
using BaralhoDeCartas.Models;
using BaralhoDeCartas.Models.Interfaces;
using BaralhoDeCartas.Services.Interfaces;

namespace BaralhoDeCartas.Services
{
    public class BlackjackService : IBlackjackService
    {
        private readonly IBaralhoApiClient _baralhoApiClient;
        private readonly IJogadorFactory _jogadorFactory;
        private readonly IJogoFactory _jogoFactory;
        private const int CartasIniciaisPorJogador = 2;
     
        public BlackjackService(IBaralhoApiClient baralhoApiClient,IJogadorFactory jogadorFactory,IJogoFactory jogoFactory)
        {
            _baralhoApiClient = baralhoApiClient;
            _jogadorFactory = jogadorFactory;
            _jogoFactory = jogoFactory;
        }

        public async Task<IJogoBlackJack> CriarJogoBlackJackAsync(int numeroJogadores)
        {
            IBaralho baralho = await _baralhoApiClient.CriarNovoBaralhoAsync();
            List<IJogadorDeBlackjack> jogadores = await IniciarRodadaAsync(baralho.BaralhoId, numeroJogadores);

            return _jogoFactory.CriarJogoBlackJack(jogadores, baralho );
        }

        public async Task<IBaralho> CriarNovoBaralhoAsync()
        {
            return await _baralhoApiClient.CriarNovoBaralhoAsync();
        }

        public async Task<List<IJogadorDeBlackjack>> IniciarRodadaAsync(string baralhoId, int numeroJogadores)
        {
            List<IJogadorDeBlackjack> jogadores = new List<IJogadorDeBlackjack>();
            int totalCartas = numeroJogadores * CartasIniciaisPorJogador;

            List<ICarta> todasAsCartas = await _baralhoApiClient.ComprarCartasAsync(baralhoId, totalCartas);
            
            for (int i = 0; i < numeroJogadores; i++)
            {
                List<ICarta> cartasDoJogador = todasAsCartas.Skip(i * CartasIniciaisPorJogador)
                          .Take(CartasIniciaisPorJogador)
                          .ToList();

                int jogadorId = i + 1;
                string nomeJogador = $"Jogador {jogadorId}";

                IJogadorDeBlackjack jogador = _jogadorFactory.CriarJogadorDeBlackJack(cartasDoJogador, jogadorId, nomeJogador);
                jogadores.Add(jogador);
            }

            return jogadores;
        }

        public async Task<ICarta> ComprarCartaAsync(string baralhoId, IJogadorDeBlackjack jogador)
        {
            if (jogador.Parou || jogador.Estourou)
            {
                throw new InvalidOperationException($"O jogador {jogador.Nome} não pode comprar mais cartas.");
            }

            var cartas = await _baralhoApiClient.ComprarCartasAsync(baralhoId, 1);
            var novaCarta = cartas.FirstOrDefault();
            
            if (novaCarta != null)
            {
                jogador.Cartas.Add(novaCarta);
            }

            return novaCarta;
        }

        public List<IJogadorDeBlackjack> DeterminarVencedoresAsync(List<IJogadorDeBlackjack> jogadores)
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

        public async Task<IBaralho> RetornarCartasAoBaralhoAsync(string baralhoId)
        {
            return await _baralhoApiClient.RetornarCartasAoBaralhoAsync(baralhoId);
        }
    }
} 