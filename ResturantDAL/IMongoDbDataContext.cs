using MongoDB.Driver;

namespace ResturantDAL
{
    public interface IMongoDbDataContext
    {
        IMongoClient Client { get; }
        IMongoDatabase Database { get; }
        IMongoCollection<Resturant> GetResturant { get; }
    }
}