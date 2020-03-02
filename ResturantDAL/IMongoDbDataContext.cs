using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

namespace ResturantDAL
{
    public interface IMongoDbDataContext
    {
        IMongoClient Client { get; }
        IMongoDatabase Database { get; }
        IMongoCollection<Resturant> GetResturant { get; }
        List<Resturant> Get();

        Resturant Get(ObjectId id);
        Resturant Create(Resturant resturant);
        void Update(ObjectId id, Resturant resturant);

        void Remove(Resturant resturant);
        void Remove(ObjectId id);

    }
}