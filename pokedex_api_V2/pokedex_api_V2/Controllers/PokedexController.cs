using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

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

        [HttpPost]
        public List<Pokemon> GetPokemonByName([FromBody] string name)
        {

            try
            {
                var database = _dbConnnect.Connect();

                var pokemonCollection = database.GetCollection<Pokemon>("pokemon").AsQueryable().ToList();

                return pokemonCollection.FindAll(p => p.Name.Contains(name));
            }
            catch (System.Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPost]
        public List<Pokemon> GetPokemonByTypes([FromBody] string[] types)
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
                var pokemons = (from pokemon in pokemonCollection
                                where
                                pokemon.Types.Contains(types[0])
                                &&
                                pokemon.Types.Contains(types.Count() > 1 ? types[1] : types[0])
                                select pokemon).ToList();
                return pokemons;
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
