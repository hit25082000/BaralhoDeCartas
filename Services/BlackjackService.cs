using BaralhoDeCartas.Api.Interfaces;
using BaralhoDeCartas.Models;

namespace BaralhoDeCartas.Services
{
    public class BlackjackService : IBlackjackService
    {
        private readonly IBaralhoApiClient _baralhoApiClient;
        private const int CartasIniciaisPorJogador = 2;

        public BlackjackService(IBaralhoApiClient baralhoApiClient)
        {
            _baralhoApiClient = baralhoApiClient;
        }

        public async Task<Baralho> IniciarJogo()
        {
            return await _baralhoApiClient.CriarNovoBaralho();
        }

        public async Task<List<JogadorDeBlackjack>> IniciarRodada(string deckId, int numeroJogadores)
        {
            var jogadores = new List<JogadorDeBlackjack>();
            var totalCartas = numeroJogadores * CartasIniciaisPorJogador;

            var todasAsCartas = await _baralhoApiClient.ComprarCartas(deckId, totalCartas);
            
            for (int i = 0; i < numeroJogadores; i++)
            {
                var jogador = new JogadorDeBlackjack
                {
                    Id = i + 1,
                    Nome = $"Jogador {i + 1}",
                    Cartas = todasAsCartas
                        .Skip(i * CartasIniciaisPorJogador)
                        .Take(CartasIniciaisPorJogador)
                        .ToList()
                };
                jogadores.Add(jogador);
            }

            return jogadores;
        }

        public async Task<Carta> ComprarCarta(string deckId, JogadorDeBlackjack jogador)
        {
            if (jogador.Parou || jogador.Estourou)
            {
                throw new InvalidOperationException($"O jogador {jogador.Nome} não pode comprar mais cartas.");
            }

            var cartas = await _baralhoApiClient.ComprarCartas(deckId, 1);
            var novaCarta = cartas.FirstOrDefault();
            
            if (novaCarta != null)
            {
                jogador.Cartas.Add(novaCarta);
            }

            return novaCarta;
        }

        public List<JogadorDeBlackjack> DeterminarVencedores(List<JogadorDeBlackjack> jogadores)
        {
            var jogadoresValidos = jogadores.Where(j => !j.Estourou).ToList();

            if (!jogadoresValidos.Any())
            {
                return new List<JogadorDeBlackjack>();
            }

            var jogadoresComBlackjack = jogadoresValidos.Where(j => j.TemBlackjack()).ToList();
            if (jogadoresComBlackjack.Any())
            {
                return jogadoresComBlackjack;
            }

            var maiorPontuacao = jogadoresValidos.Max(j => j.CalcularPontuacao());

            return jogadoresValidos.Where(j => j.CalcularPontuacao() == maiorPontuacao).ToList();
        }

        public async Task<bool> FinalizarJogo(string deckId)
        {
            return await _baralhoApiClient.RetornarCartasAoBaralho(deckId);
        }
    }
} 