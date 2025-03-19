using BaralhoDeCartas.Api.Interfaces;
using BaralhoDeCartas.Factory.Interfaces;
using BaralhoDeCartas.Models.Interfaces;
using BaralhoDeCartas.Services.Interfaces;

namespace BaralhoDeCartas.Services
{
    public class JogoService : IJogoService
    {
        private readonly IBaralhoApiClient _baralhoApiClient;
        private readonly IJogadorFactory _jogadorFactory;
        private const int CARTAS_POR_JOGADOR = 5;

        public JogoService(IBaralhoApiClient baralhoApiClient, IJogadorFactory jogadorFactory)
        {
            _baralhoApiClient = baralhoApiClient;
            _jogadorFactory = jogadorFactory;
        }

        public async Task<IBaralho> IniciarNovoJogo()
        {
            return await _baralhoApiClient.CriarNovoBaralho();
        }

        public async Task<List<IJogador>> DistribuirCartas(string deckId, int numeroJogadores)
        {
            var jogadores = new List<IJogador>();
            var totalCartas = numeroJogadores * CARTAS_POR_JOGADOR;

            var todasAsCartas = await _baralhoApiClient.ComprarCartas(deckId, totalCartas);
            
            for (int i = 0; i < numeroJogadores; i++)
            {
                IJogador jogador = _jogadorFactory.CriarJogador(todasAsCartas, CARTAS_POR_JOGADOR, i);               
                jogadores.Add(jogador);
            }

            return jogadores;
        }

        public async Task<IJogador> DeterminarVencedor(List<IJogador> jogadores)
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