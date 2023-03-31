using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace pokedex_api_V2
{
    public class Pokemon
    {
        [BsonElement("_id")]
        public int Id { get; set; }
        [BsonElement("types")]
        public List<string>? Types { get; set; }
        [BsonElement("name")]
        public string? Name { get; set; }
        [BsonElement("legendary")]
        public bool Legendary { get; set; }
        [BsonElement("hp")]
        public int Hp { get; set; }
        [BsonElement("attack")]
        public int Attack { get; set; }
        [BsonElement("defense")]
        public int Defense { get; set; }
        [BsonElement("speed")]
        public int Speed { get; set; }
        [BsonElement("generation")]
        public int Generation { get; set; }
    }
}
