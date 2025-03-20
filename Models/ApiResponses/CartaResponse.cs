﻿using System.Text.Json.Serialization;

namespace BaralhoDeCartas.Models.ApiResponses
{
    public class CartasResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("deck_id")]
        public string Deck_id { get; set; }

        [JsonPropertyName("cards")]
        public List<CartaListItemResponse> Cards { get; set; }

        [JsonPropertyName("remaining")]
        public int Remaining { get; set; }
    }

    public class CartaListItemResponse
    {
        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("suit")]
        public string Suit { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
    }
}
