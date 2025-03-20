using BaralhoDeCartas.Api.Interfaces;
using BaralhoDeCartas.Factory;
using BaralhoDeCartas.Factory.Interfaces;
using BaralhoDeCartas.Models.Interfaces;
using BaralhoDeCartas.Services.Interfaces;

namespace BaralhoDeCartas.Services
{
    public class MaiorCartaService : IMaiorCartaService
    {
        private readonly IBaralhoApiClient _baralhoApiClient;
        private readonly IJogadorFactory _jogadorFactory;
        private readonly IJogoFactory _jogoFactory;
        private const int CARTAS_POR_JOGADOR = 5;

        public MaiorCartaService(IBaralhoApiClient baralhoApiClient, IJogadorFactory jogadorFactory, IJogoFactory jogoFactory)
        {
            _baralhoApiClient = baralhoApiClient;
            _jogadorFactory = jogadorFactory;
            _jogoFactory = jogoFactory;
        }

        public async Task<IJogoMaiorCarta> CriarJogoMaiorCartaAsync(int numeroJogadores)
        {
            IBaralho baralho = await _baralhoApiClient.CriarNovoBaralhoAsync();
            List<IJogador> jogadores = await DistribuirCartasAsync(baralho.BaralhoId, numeroJogadores);

            baralho.QuantidadeDeCartasRestantes -= jogadores.Sum((jogador) => jogador.Cartas.Count());

            return _jogoFactory.CriarJogoMaiorCarta(jogadores, baralho);
        }

        public async Task<IBaralho> CriarNovoBaralhoAsync()
        {
            return await _baralhoApiClient.CriarNovoBaralhoAsync();
        }

        public async Task<List<IJogador>> DistribuirCartasAsync(string baralhoId, int numeroJogadores)
        {
            List<IJogador> jogadores = new List<IJogador>();
            int totalCartas = numeroJogadores * CARTAS_POR_JOGADOR;

            List<ICarta> todasAsCartas = await _baralhoApiClient.ComprarCartasAsync(baralhoId, totalCartas);
            
            for (int i = 0; i < numeroJogadores; i++)
            {
                List<ICarta> cartasDoJogador = todasAsCartas.Skip(i * CARTAS_POR_JOGADOR).Take(CARTAS_POR_JOGADOR).ToList();

                int jogadorId = i + 1;
                string nomeJogador = $"Jogador {jogadorId}";

                IJogador jogador = _jogadorFactory.CriarJogador(cartasDoJogador,jogadorId,nomeJogador);               
                jogadores.Add(jogador);
            }

            return jogadores;
        }

        public async Task<IJogador> DeterminarVencedorAsync(List<IJogador> jogadores)
        {
            return jogadores
                .OrderByDescending(j => j.ObterCartaDeMaiorValor()?.Valor ?? 0)
                .FirstOrDefault();
        }

        public async Task<IBaralho> FinalizarJogoAsync(string baralhoId)
        {
            return await _baralhoApiClient.RetornarCartasAoBaralhoAsync(baralhoId);
        }
        
        public async Task<IBaralho> VerificarBaralhoAsync(string baralhoId)
        {
            try
            {
                // Primeiro, vamos tentar usar o método EmbaralharBaralho para obter informações do baralho
                var baralho = await _baralhoApiClient.EmbaralharBaralhoAsync(baralhoId, true);
                
                // Se o baralho existir mas tiver poucas cartas, devolver todas ao baralho
                if (baralho.QuantidadeDeCartasRestantes < 10) // Um número seguro para garantir cartas suficientes
                {
                    // Devolver todas as cartas ao baralho e embaralhar novamente
                    await _baralhoApiClient.RetornarCartasAoBaralhoAsync(baralhoId);
                    baralho = await _baralhoApiClient.EmbaralharBaralhoAsync(baralhoId, false);
                }
                
                return baralho;
            }
            catch
            {
                try
                {
                    // Se ocorrer um erro (baralho não existe mais ou outro problema),
                    // tente primeiro retornar as cartas, caso o baralho ainda exista
                    await _baralhoApiClient.RetornarCartasAoBaralhoAsync(baralhoId);
                    return await _baralhoApiClient.EmbaralharBaralhoAsync(baralhoId, false);
                }
                catch
                {
                    // Se ainda falhar, criar um novo baralho
                    return await CriarNovoBaralhoAsync();
                }
            }
        }
        
        public async Task<IBaralho> EmbaralharBaralhoAsync(string baralhoId, bool embaralharSomenteCartasRestantes)
        {
            return await _baralhoApiClient.EmbaralharBaralhoAsync(baralhoId, embaralharSomenteCartasRestantes);
        }
    }
} 