using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
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
                var pokemonCollection = database.GetCollection<Pokemon>("pokemon").AsQueryable().ToList();

                var pokemons = pokemonCollection.Where(p =>
                                 p.Types.Any(t => t.Equals(types[0], StringComparison.OrdinalIgnoreCase)) &&
                                 p.Types.Any(t => t.Equals(types.Count() > 1 ? types[1] : types[0], StringComparison.OrdinalIgnoreCase)))
                              .ToList();

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
                var pokemonCollection = database.GetCollection<Pokemon>("pokemon").AsQueryable().ToList();
                var pokemonSelected = (from pokemon in pokemonCollection
                                       where
                                       pokemon.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                                       select pokemon).FirstOrDefault();

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
