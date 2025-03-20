using System.Text.Json;
using BaralhoDeCartas.Api.Interfaces;
using BaralhoDeCartas.Factory.Interfaces;
using BaralhoDeCartas.Models.ApiResponses;
using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Api
{
    public class BaralhoApiClient : IBaralhoApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IBaralhoFactory _baralhoFactory;
        private readonly ICartaFactory _cartaFactory;
        private const string BaseUrl = "https://deckofcardsapi.com/api/deck";

        public BaralhoApiClient(HttpClient httpClient, IBaralhoFactory baralhoFactory, ICartaFactory cartaFactory)
        {
            _httpClient = httpClient;
            _baralhoFactory = baralhoFactory;
            _cartaFactory = cartaFactory;
        }

        public async Task<IBaralho> CriarNovoBaralhoAsync()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/new/shuffle/");
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            var baralhoResponse = JsonSerializer.Deserialize<BaralhoResponse>(content);

            if (!baralhoResponse.Success)
            {
                throw new HttpRequestException("Falha ao criar novo baralho");
            }

            return _baralhoFactory.CriarBaralho(baralhoResponse);
        }

        public async Task<IBaralho> EmbaralharBaralhoAsync(string baralhoId, bool embaralharSomenteCartasRestantes = true)
        {
            if (string.IsNullOrEmpty(baralhoId))
            {
                throw new ArgumentException("ID do baralho não pode ser nulo ou vazio", nameof(baralhoId));
            }

            string url = $"{BaseUrl}/{baralhoId}/shuffle/";

            if (embaralharSomenteCartasRestantes)
            {
                url = $"{BaseUrl}/{baralhoId}/shuffle/?remaining=true";
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var baralhoResponse = JsonSerializer.Deserialize<BaralhoResponse>(content);

            if (!baralhoResponse.Success)
            {
                throw new HttpRequestException($"Falha ao embaralhar cartas");
            }

            return _baralhoFactory.CriarBaralho(baralhoResponse);
        }

        public async Task<List<ICarta>> ComprarCartasAsync(string baralhoId, int quantidade)
        {
            if (string.IsNullOrEmpty(baralhoId))
            {
                throw new ArgumentException("ID do baralho não pode ser nulo ou vazio", nameof(baralhoId));
            }

            if (quantidade <= 0 )
            {
                throw new ArgumentException("Quantidade de cartas deve ser maior que zero", nameof(quantidade));
            }

            var response = await _httpClient.GetAsync($"{BaseUrl}/{baralhoId}/draw/?count={quantidade}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var cartasResponse = JsonSerializer.Deserialize<CartasResponse>(content);

            if (!cartasResponse.Success)
            {
                throw new HttpRequestException($"Falha ao comprar cartas "+ cartasResponse.Error);
            }

            return _cartaFactory.CriarCartas(cartasResponse);
        }

        public async Task<IBaralho> RetornarCartasAoBaralhoAsync(string baralhoId)
        {
            if (string.IsNullOrEmpty(baralhoId))
            {
                throw new ArgumentException("ID do baralho não pode ser nulo ou vazio", nameof(baralhoId));
            }

            var response = await _httpClient.GetAsync($"{BaseUrl}/{baralhoId}/return/");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var baralhoResponse = JsonSerializer.Deserialize<BaralhoResponse>(content);

            if (!baralhoResponse.Success)
            {
                throw new HttpRequestException($"Falha ao retornar cartas ao baralho");
            }

            return _baralhoFactory.CriarBaralho(baralhoResponse);
        }
    }
}