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

        public async Task<IBaralho> CriarNovoBaralho()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/new/shuffle/");
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            BaralhoResponse baralhoResponse = JsonSerializer.Deserialize<BaralhoResponse>(content);

            return _baralhoFactory.CriarBaralho(baralhoResponse);
        }

        public async Task<IBaralho> EmbaralharBaralho(string baralhoId, bool embaralharSomenteCartasRestantes)
        {
            string url = $"{BaseUrl}/{baralhoId}/shuffle/";

            if (embaralharSomenteCartasRestantes)
            {
                url = $"{BaseUrl}/{baralhoId}/shuffle/?remaining=true";
            }

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var baralhoResponse = JsonSerializer.Deserialize<BaralhoResponse>(content);

            return _baralhoFactory.CriarBaralho(baralhoResponse);
        }

        public async Task<List<ICarta>> ComprarCartas(string baralhoId, int quantidade)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{baralhoId}/draw/?count={quantidade}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var cartasResponse = JsonSerializer.Deserialize<CartasResponse>(content);

            return _cartaFactory.CriarCartas(cartasResponse);
        }

        public async Task<bool> RetornarCartasAoBaralho(string baralhoId)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{baralhoId}/return/");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var baralhoResponse = JsonSerializer.Deserialize<BaralhoResponse>(content);

            return baralhoResponse.Success;
        }
    }
}