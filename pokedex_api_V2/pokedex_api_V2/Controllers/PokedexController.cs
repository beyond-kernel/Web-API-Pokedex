using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using System;

namespace pokedex_api_V2.Controllers
{

    [ApiController]
    [AllowAnonymous]
    [Route("api/Pokedex/[action]")]
    public class PokedexController
    {

        private IDbConnect _dbConnnect;

        public PokedexController(IDbConnect dbConnnect)
        {
            _dbConnnect = dbConnnect;
        }


        [HttpGet]
        public IEnumerable<Pokemon> GetPokemonList()
        {
            try
            {
                var database = _dbConnnect.Connect();

                var pokemonCollection = database.GetCollection<Pokemon>("pokemon").AsQueryable().ToList();

                return pokemonCollection;
            }
            catch (System.Exception e)
            {
                throw new Exception($"Não foi possivel carregar a lista de pokemons: \n {e.Message}");
            }
        }

        [HttpGet]
        public List<Pokemon> GetPokemonByTypes([FromHeader] string[] types)
        {
            try
            {
                if (types.Count() < 1 || types[0] == "")
                {
                    return this.GetPokemonList().ToList();
                }
                else if (types.Count() > 2)
                {
                    throw new Exception("Pokemons só podem ter máximo 2 tipos");
                }
                var database = _dbConnnect.Connect();
                var pokemonCollection = database.GetCollection<Pokemon>("pokemon");

                var regex1 = new BsonRegularExpression($"^{Regex.Escape(types[0])}$", "i");
                var filter1 = Builders<Pokemon>.Filter.Regex("Types", regex1);

                FilterDefinition<Pokemon> combinedFilter = filter1;

                if (types.Count() > 1 && !string.IsNullOrEmpty(types[1]))
                {
                    var regex2 = new BsonRegularExpression($"^{Regex.Escape(types[1])}$", "i");
                    var filter2 = Builders<Pokemon>.Filter.Regex("Types", regex2);
                    combinedFilter = Builders<Pokemon>.Filter.And(filter1, filter2);
                }

                var pokemons = pokemonCollection.Find(combinedFilter).ToList();

                return pokemons;
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet]
        public Pokemon GetPokemonByName([FromHeader] string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("Informar nome do Pokémon");
                }
                var database = _dbConnnect.Connect();
                var pokemonCollection = database.GetCollection<Pokemon>("pokemon");

                var filter = Builders<Pokemon>.Filter.Regex(p => p.Name, new BsonRegularExpression($"^{Regex.Escape(name)}$", "i"));
                var pokemonSelected = pokemonCollection.Find(filter).FirstOrDefault();

                return pokemonSelected;
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost]
        public StatusCodeResult RegisterPokemon(Pokemon pokemon)
        {
            var database = _dbConnnect.Connect();
            var pokemonCollection = database.GetCollection<Pokemon>("pokemon");
            pokemonCollection.InsertOneAsync(pokemon);
            return new StatusCodeResult(200);
        }
    }
}
