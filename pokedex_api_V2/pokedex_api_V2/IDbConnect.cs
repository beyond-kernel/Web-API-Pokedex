using MongoDB.Driver;

namespace pokedex_api_V2
{
    public interface IDbConnect
    {
        public IMongoDatabase Connect();
    }
}
