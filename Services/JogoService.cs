using BaralhoDeCartas.Api.Interfaces;
using BaralhoDeCartas.Models;

namespace BaralhoDeCartas.Services
{
    public class JogoService : IJogoService
    {
        private readonly IBaralhoApiClient _baralhoApiClient;
        private const int CARTAS_POR_JOGADOR = 5;

        public JogoService(IBaralhoApiClient baralhoApiClient)
        {
            _baralhoApiClient = baralhoApiClient;
        }

        public async Task<Baralho> IniciarNovoJogo()
        {
            return await _baralhoApiClient.CriarNovoBaralho();
        }

        public async Task<List<Jogador>> DistribuirCartas(string deckId, int numeroJogadores)
        {
            var jogadores = new List<Jogador>();
            var totalCartas = numeroJogadores * CARTAS_POR_JOGADOR;

            var todasAsCartas = await _baralhoApiClient.ComprarCartas(deckId, totalCartas);
            
            for (int i = 0; i < numeroJogadores; i++)
            {
                var jogador = new Jogador
                {
                    Id = i + 1,
                    Nome = $"Jogador {i + 1}",
                    Cartas = todasAsCartas.Skip(i * CARTAS_POR_JOGADOR).Take(CARTAS_POR_JOGADOR).ToList()
                };
                jogadores.Add(jogador);
            }

            return jogadores;
        }

        public async Task<Jogador> DeterminarVencedor(List<Jogador> jogadores)
        {
            return jogadores
                .OrderByDescending(j => j.ObterCartaMaisAlta()?.ValorNumerico ?? 0)
                .FirstOrDefault();
        }

        public async Task<bool> FinalizarJogo(string deckId)
        {
            return await _baralhoApiClient.RetornarCartasAoBaralho(deckId);
        }
    }
} 