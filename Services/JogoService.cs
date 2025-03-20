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

        public async Task<List<IJogador>> DistribuirCartas(string baralhoId, int numeroJogadores)
        {
            List<IJogador> jogadores = new List<IJogador>();
            int totalCartas = numeroJogadores * CARTAS_POR_JOGADOR;

            List<ICarta> todasAsCartas = await _baralhoApiClient.ComprarCartas(baralhoId, totalCartas);
            
            for (int i = 0; i < numeroJogadores; i++)
            {
                List<ICarta> cartasDoJogador = todasAsCartas.Skip(i * CARTAS_POR_JOGADOR).Take(CARTAS_POR_JOGADOR).ToList();

                IJogador jogador = _jogadorFactory.CriarJogador(cartasDoJogador, CARTAS_POR_JOGADOR, i);               
                jogadores.Add(jogador);
            }

            return jogadores;
        }

        public async Task<IJogador> DeterminarVencedor(List<IJogador> jogadores)
        {
            return jogadores
                .OrderByDescending(j => j.ObterCartaMaisAlta()?.Valor ?? 0)
                .FirstOrDefault();
        }

        public async Task<bool> FinalizarJogo(string baralhoId)
        {
            return await _baralhoApiClient.RetornarCartasAoBaralho(baralhoId);
        }
    }
} 