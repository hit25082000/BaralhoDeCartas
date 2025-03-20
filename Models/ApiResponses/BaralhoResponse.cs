using System.Text.Json.Serialization;

namespace BaralhoDeCartas.Models.ApiResponses
{
    public class BaralhoResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("deck_id")]
        public string DeckId { get; set; }

        [JsonPropertyName("shuffled")]
        public bool Shuffled { get; set; }

        [JsonPropertyName("remaining")]
        public int Remaining { get; set; }
    }
}
