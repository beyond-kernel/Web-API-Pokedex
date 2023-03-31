using MongoDB.Driver;

namespace pokedex_api_V2
{
    public class DbConnect : IDbConnect
    {
        public IMongoDatabase Connect()
        {
            var client = new MongoClient(
                   "mongodb://catalogdb:27017/?readPreference=primary&appname=MongoDB%20Compass&ssl=false"
               );

            return client.GetDatabase("pokemon_center");
        }
    }
}
