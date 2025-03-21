using BaralhoDeCartas.Api.Interfaces;
using BaralhoDeCartas.Factory;
using BaralhoDeCartas.Factory.Interfaces;
using BaralhoDeCartas.Models;
using BaralhoDeCartas.Models.Interfaces;
using BaralhoDeCartas.Services.Interfaces;
using BaralhoDeCartas.Exceptions;
using BaralhoDeCartas.Common;

namespace BaralhoDeCartas.Services
{
    public class BlackjackService : IBlackjackService
    {
        private readonly IBaralhoApiClient _baralhoApiClient;
        private readonly IJogadorFactory _jogadorFactory;
        private readonly IJogoFactory _jogoFactory;
        private const int CartasIniciaisPorJogador = 2;
     
        public BlackjackService(IBaralhoApiClient baralhoApiClient, IJogadorFactory jogadorFactory, IJogoFactory jogoFactory)
        {
            _baralhoApiClient = baralhoApiClient;
            _jogadorFactory = jogadorFactory;
            _jogoFactory = jogoFactory;
        }

        private void ValidarNumeroJogadores(int numeroJogadores)
        {
            if (numeroJogadores <= 0)
            {
                throw new ArgumentException("O número de jogadores deve ser maior que zero");
            }
        }

        private void ValidarBaralhoId(string baralhoId)
        {
            if (string.IsNullOrEmpty(baralhoId))
            {
                throw new ArgumentException("O ID do baralho não pode ser nulo ou vazio");
            }
        }

        private void ValidarJogador(IJogadorDeBlackjack jogador)
        {
            if (jogador == null)
            {
                throw new ArgumentNullException(nameof(jogador), "O jogador não pode ser nulo");
            }

            if (jogador.Parou || jogador.Estourou)
            {
                throw new InvalidOperationException($"O jogador {jogador.Nome} não pode comprar mais cartas.");
            }
        }

        private void ValidarListaJogadores(List<IJogadorDeBlackjack> jogadores)
        {
            if (jogadores == null || !jogadores.Any())
            {
                throw new ArgumentException("A lista de jogadores não pode estar vazia");
            }
        }

        public async Task<IJogoBlackJack> CriarJogoBlackJackAsync(int numeroJogadores)
        {
            ValidarNumeroJogadores(numeroJogadores);

            return await ServiceExceptionHandler.HandleServiceExceptionAsync(async () =>
            {
                IBaralho baralho = await _baralhoApiClient.CriarNovoBaralhoAsync();
                List<IJogadorDeBlackjack> jogadores = await IniciarRodadaAsync(baralho.BaralhoId, numeroJogadores);

                baralho.QuantidadeDeCartasRestantes -= jogadores.Sum((jogador) => jogador.Cartas.Count());

                return _jogoFactory.CriarJogoBlackJack(jogadores, baralho);
            });
        }

        public async Task<IBaralho> CriarNovoBaralhoAsync()
        {
            return await ServiceExceptionHandler.HandleServiceExceptionAsync(async () =>
            {
                return await _baralhoApiClient.CriarNovoBaralhoAsync();
            });
        }

        public async Task<List<IJogadorDeBlackjack>> IniciarRodadaAsync(string baralhoId, int numeroJogadores)
        {
            ValidarBaralhoId(baralhoId);
            ValidarNumeroJogadores(numeroJogadores);

            return await ServiceExceptionHandler.HandleServiceExceptionAsync(async () =>
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
            });
        }

        public async Task<ICarta> ComprarCartaAsync(string baralhoId, IJogadorDeBlackjack jogador)
        {
            ValidarBaralhoId(baralhoId);
            ValidarJogador(jogador);

            return await ServiceExceptionHandler.HandleServiceExceptionAsync(async () =>
            {
                var cartas = await _baralhoApiClient.ComprarCartasAsync(baralhoId, 1);
                var novaCarta = cartas.FirstOrDefault();
                
                if (novaCarta != null)
                {
                    jogador.Cartas.Add(novaCarta);
                }

                jogador.CalcularPontuacao();

                return novaCarta;
            });
        }

        public List<IJogadorDeBlackjack> DeterminarVencedoresAsync(List<IJogadorDeBlackjack> jogadores)
        {
            ValidarListaJogadores(jogadores);

            return ServiceExceptionHandler.HandleServiceException(() =>
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
            });
        }

        public async Task<IBaralho> RetornarCartasAoBaralhoAsync(string baralhoId)
        {
            ValidarBaralhoId(baralhoId);

            return await ServiceExceptionHandler.HandleServiceExceptionAsync(async () =>
            {
                return await _baralhoApiClient.RetornarCartasAoBaralhoAsync(baralhoId);
            });
        }
    }
} 