using System.Text.Json;
using BaralhoDeCartas.Models;

namespace BaralhoDeCartas.Api
{
    public class BaralhoApiClient : IBaralhoApiClient
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://deckofcardsapi.com/api/deck";

        public BaralhoApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Baralho> CriarNovoBaralho()
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/new/shuffle/");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var baralhoResponse = JsonSerializer.Deserialize<BaralhoResponse>(content);

            return new Baralho
            {
                DeckId = baralhoResponse.Deck_id,
                Shuffled = baralhoResponse.Shuffled,
                Remaining = baralhoResponse.Remaining
            };
        }

        public async Task<List<Carta>> ComprarCartas(string deckId, int quantidade)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{deckId}/draw/?count={quantidade}");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var cartasResponse = JsonSerializer.Deserialize<CartasResponse>(content);

            return cartasResponse.Cards;
        }

        public async Task<bool> RetornarCartasAoBaralho(string deckId)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/{deckId}/return/");
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync();
            var baralhoResponse = JsonSerializer.Deserialize<BaralhoResponse>(content);

            return baralhoResponse.Success;
        }
    }
} 