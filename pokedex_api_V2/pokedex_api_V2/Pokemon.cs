using System.Collections.Generic;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ThirdParty.Json.LitJson;

namespace pokedex_api_V2
{
    public class Pokemon
    {
        [BsonElement("_id")]
        [JsonPropertyName("_id")]
        public int Id { get; set; }
        [BsonElement("types")]
        [JsonPropertyName("types")]
        public List<string>? Types { get; set; }
        [BsonElement("name")]
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [BsonElement("legendary")]
        [JsonPropertyName("legendary")]
        public bool Legendary { get; set; }
        [BsonElement("hp")]
        [JsonPropertyName("hp")]
        public int Hp { get; set; }
        [BsonElement("attack")]
        [JsonPropertyName("attack")]
        public int Attack { get; set; }
        [BsonElement("defense")]
        [JsonPropertyName("defense")]
        public int Defense { get; set; }
        [BsonElement("speed")]
        [JsonPropertyName("speed")]
        public int Speed { get; set; }
        [BsonElement("generation")]
        [JsonPropertyName("generation")]
        public int Generation { get; set; }
    }
}
